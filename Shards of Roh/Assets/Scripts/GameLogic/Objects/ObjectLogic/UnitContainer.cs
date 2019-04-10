using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class UnitContainer : ObjectContainer {

	public Unit unit { get; set; }
	public List<Behaviours> unitBehaviours { get; private set; }

	//public CapsuleCollider collider { get; private set; }
	public UnityEngine.AI.NavMeshAgent agent { get; private set; }
	public UnityEngine.AI.NavMeshObstacle obstacle { get; private set; }

	void Start () {
		//GameManager.print ("Start Worker");
		//Set values for preset units
		setup ();
		setCleanUnitBehaviours ();

		//Set values of preset units on map
		if (presetOwnerName != "") {
			Player newPlayer = GameManager.addPlayerToGame (presetOwnerName);
			unit = ObjectFactory.createUnitByName (gameObject.name, newPlayer);
			if (newPlayer.units.Contains (this) == false) {
				newPlayer.units.Add (this);
			}
		}

		//Set values of MinimapIcon
		foreach (Transform child in gameObject.transform) {
			if (child.name == "MinimapIcon") {
				child.gameObject.SetActive (true);
				child.gameObject.transform.position = new Vector3 (child.gameObject.transform.position.x, 20, child.gameObject.transform.position.z);
				child.gameObject.transform.localScale = new Vector3 (5.0f, 0.01f, 5.0f);
			}
		}

		setNavMeshProperties ();

		//Set values of Textures
		foreach (var r in gameObject.GetComponentsInChildren<MeshRenderer> ()) {
			if (r.material.name.Contains("WK_Standard")) {
				r.material = Resources.Load (PlayerMaterial.getMaterial (unit.owner.name), typeof (Material)) as Material;
			}
			if (r.material.name.Contains ("MinimapDefault")) {
				r.material = Resources.Load (PlayerMaterial.getMinimapColour (unit.owner.name), typeof(Material)) as Material;
			}
		}
		foreach (var r in gameObject.GetComponentsInChildren<SkinnedMeshRenderer> ()) {
			if (r.material.name.Contains("WK_Standard")) {
				r.material = Resources.Load (PlayerMaterial.getMaterial (unit.owner.name), typeof (Material)) as Material;
			}
			if (r.material.name.Contains ("MinimapDefault")) {
				r.material = Resources.Load (PlayerMaterial.getMinimapColour (unit.owner.name), typeof(Material)) as Material;
			}
		}

		//Set values of Behaviours
		if (unit.unitType == UnitType.Villager) {
			unitBehaviours.Add (new PassiveFlee (this));
		} else {
			unitBehaviours.Add (new PassiveAttack (this));
			unitBehaviours.Add (new PassiveRetaliate (this));
		}
	}

	void Update () {

		unit.update ();

		checkMoveLogic ();
		checkAttackLogic ();
		checkBehaviourLogic ();

		if (unit.isDead == true) {
			unit.owner.remCurUnitTarget (this);
			foreach (var r in GameManager.playersInGame) {
				r.visibleObjects.rememberedEnemyUnits.Remove (this);
				r.visibleObjects.rememberedResourceUnits.Remove (this);
			}
			gameObject.GetComponent<CapsuleCollider> ().enabled = false;
			agent.enabled = false;
			gameObject.GetComponent<Animator> ().SetBool ("isDead", true);
			gameObject.GetComponent<Animator> ().SetInteger ("deathAnimation", Random.Range (0, 2));
			if (gameObject.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("PostDeath")) {
				GameManager.destroyUnit (this, unit.owner);
			}
		}
	}

	public void checkMoveLogic () {

		//Update curLoc and isMoving
		if (agent != null) {
			unit.curLoc = gameObject.GetComponent<CapsuleCollider> ().transform.position;
			if (gameObject.GetComponent<Animator> () != null) {
				if (Vector3.Distance (unit.curLoc, agent.destination) <= 0.1 || agent.enabled == false) {
					if (unit.moveDestinations.Count > 0) {
						unit.moveDestinations.RemoveAt (0);
						if (unit.moveDestinations.Count > 0) {
							agent.SetDestination (unit.moveDestinations [0]);
						} else {
							gameObject.GetComponent<Animator> ().SetBool ("isMoving", false);
							unit.isMoving = false;
						}
					} else {
						gameObject.GetComponent<Animator> ().SetBool ("isMoving", false);
						unit.isMoving = false;
					}
						
					setWaypointFlagActive (false);
				} else {
					gameObject.GetComponent<Animator> ().SetBool ("isMoving", true);
					navMeshToggle ("Agent");
					unit.isMoving = true;
				}

				if (agent.enabled == true && unit.isMoving == false && ((unit.unitTarget == null && unit.buildingTarget == null) || unit.isAttacking == true)) {
					navMeshToggle ("Obstacle");
				}

				if (unit.isCombatTimer > 0) {
					gameObject.GetComponent<Animator> ().SetBool ("isCombat", true);
				} else {
					gameObject.GetComponent<Animator> ().SetBool ("isCombat", false);
				}
			} else {
				print ("Missing Animator - UnitContainer");
			}

			setWaypointFlagLocation (unit.flagPosition);
		} else {
			print ("Missing NavMeshAgent - UnitContainer");
		}

		//gameObject.transform.position += pathfinder.transform.localPosition;
		//GameManager.print (pathfinder.transform.position);
	}

	public void checkAttackLogic () {
		if (unit.unitTarget != null) {
			if (unit.unitTarget.unit.isDead == true) {
				unit.dropAttackTarget ();
			}
		}
		if (unit.buildingTarget != null) {
			if (unit.buildingTarget.building.isDead == true) {
				unit.dropAttackTarget ();
			}
		}

		//Check for distance to targets, update isAttacking
		if (unit.unitTarget != null && unit.unitTarget.unit.isDead == false) {

			Vector3 point1 = gameObject.GetComponent<CapsuleCollider> ().bounds.center;
			Vector3 point2 = unit.unitTarget.GetComponent<CapsuleCollider> ().bounds.center;
			point1.y = 0;
			point2.y = 0;
			float distanceToTarget = Vector3.Distance (point1, point2) - gameObject.GetComponent<CapsuleCollider> ().radius - unit.unitTarget.GetComponent<CapsuleCollider> ().radius;
			if (distanceToTarget <= unit.attackRange || (unit.isAttacking == true && unit.hasHit == false && distanceToTarget <= unit.attackRange + 3.0f) || unit.hasHit == true) {

				//I don't remember the purpose of this line, and it's causing a bug related to moving units while they are attacking. Leaving it in, commented out, in case we discover what it was solving.
				if (unit.isAttacking == false) {
					moveToLocation (false, agent.transform.position);
				}

				lookDirection (GetComponent<CapsuleCollider> ().bounds.center, unit.unitTarget.GetComponent<CapsuleCollider> ().bounds.center);
				gameObject.GetComponent<Animator> ().SetBool ("isAttacking", true);
				unit.isAttacking = true;
				if (unit.attackUnit () == true) {
					unit.unitTarget.unit.getHit (this, unit.attack);
				}
			} else {
				moveTowardCollider (false, unit.unitTarget.GetComponent<CapsuleCollider> ());
				gameObject.GetComponent<Animator> ().SetBool ("isAttacking", false);
				unit.isAttacking = false;
			}
		} else if (unit.buildingTarget != null && unit.buildingTarget.building.isDead == false) {
			Vector3 point1 = GetComponent<CapsuleCollider> ().bounds.center;
			Vector3 point2 = unit.buildingTarget.GetComponent<BoxCollider> ().ClosestPoint (GetComponent<CapsuleCollider> ().bounds.center);
			point1.y = 0;
			point2.y = 0;
			if (Vector3.Distance (point1, point2) - GetComponent<CapsuleCollider> ().radius <= unit.attackRange) {
				if (agent.enabled == true) {
					agent.ResetPath ();
				}
				lookDirection (GetComponent<CapsuleCollider> ().bounds.center, unit.buildingTarget.GetComponent<BoxCollider> ().bounds.center);
				gameObject.GetComponent<Animator> ().SetBool ("isAttacking", true);
				unit.isAttacking = true;
				if (unit.attackBuilding () == true) {
					unit.buildingTarget.building.getHit (this, unit.attack);
				}
			} else {
				moveTowardCollider (false, unit.buildingTarget.GetComponent<BoxCollider> ());
				gameObject.GetComponent<Animator> ().SetBool ("isAttacking", false);
				unit.isAttacking = false;
			}
		} else {
			gameObject.GetComponent<Animator> ().SetBool ("isAttacking", false);
			unit.isAttacking = false;
			unit.dropAttackTarget ();
			unit.passiveAttackTimer ();
		}
	}

	private void checkBehaviourLogic () {
		if (unit.isCommandedRecently == 0) {
			for (int i = 0; i < unitBehaviours.Count; i++) {
				if (unitBehaviours [i].active == false) {
					unitBehaviours.RemoveAt (i);
					i--;
				} else {
					unitBehaviours [i].enact ();
				}
			}
		}

		//Reset values tracked for behaviour logic
		unit.gotHit = false;
		unit.gotHitBy = null;
	}

	public void moveTowardCollider (bool _activeMove, Collider collider, bool _isWayPointing = false) {
		//Using closestPoint has units actually go to the closest point on the collider,
		//but provides much worse results for pathing around other units who are also going
		//for that same point compared to just targeting the object. Think of a middle ground!

		moveToLocation (_activeMove, collider.ClosestPoint (unit.curLoc), _isWayPointing);
		//moveToLocation (collider.transform.position, _isWayPointing);
	}

	public void moveToLocation (bool _activeMove, Vector3 targetLoc, bool _isWayPointing = false) {
		if (agent.enabled == true && obstacle.enabled == false) {
			if (_isWayPointing == true) {
				unit.moveDestinations.Add (targetLoc);
				agent.SetDestination (unit.moveDestinations [0]);
			} else {
				unit.moveDestinations.Clear ();
				unit.moveDestinations.Add (targetLoc);
				agent.SetDestination (targetLoc);
			}
		} else {
			StartCoroutine (moveAfterBeingObstacle (_activeMove, targetLoc, _isWayPointing));
		}
	}

	IEnumerator moveAfterBeingObstacle (bool _activeMove, Vector3 _targetLoc, bool _isWayPointing = false) {
		obstacle.enabled = false;

		yield return null; 

		if (obstacle.enabled == false) {
			agent.enabled = true;

			if (_isWayPointing == true) {
				unit.moveDestinations.Add (_targetLoc);
				agent.SetDestination (unit.moveDestinations [0]);
			} else {
				unit.moveDestinations.Clear ();
				unit.moveDestinations.Add (_targetLoc);
				agent.SetDestination (_targetLoc);
			}
		}

		yield return null;
	}

	private void lookDirection (Vector3 _point1, Vector3 _point2) {
		Vector3 direction = (_point2 - _point1).normalized;
		direction.y = 0;
		Quaternion lookRotation = Quaternion.LookRotation (direction);
		gameObject.transform.rotation = Quaternion.Slerp (gameObject.transform.rotation, lookRotation, Time.deltaTime * 10);
	}

	public bool hasBehaviour (string _name) {
		foreach (var r in unitBehaviours) {
			if (r.name == _name) {
				return true;
			}
		}

		return false;
	}

	public void navMeshToggle (string _mode) {
		/*if (_mode == "Agent") {
			if (obstacle != null) {
				obstacle.enabled = false;
			}
			if (agent != null) {
				agent.enabled = true;
			}
		}*/
		if (_mode == "Obstacle") {
			if (agent != null) {
				agent.enabled = false;
			}
			if (obstacle != null) {
				obstacle.enabled = true;
			}
		}
	}

	public void removeBehaviourByType (string _type, Behaviours _exception = null) {
		foreach (var r in unitBehaviours) {
			if (r.behaviourType == _type && r != _exception) {
				r.active = false;
			}
		}

		for (int i = 0; i < unitBehaviours.Count; i++) {
			if (unitBehaviours [i].active == false) {
				unitBehaviours.RemoveAt (i);
				i--;
			}
		}
	}

	public void setCleanUnitBehaviours () {
		unitBehaviours = new List<Behaviours> ();
	}

	public void setNavMeshProperties () {
		//Set values of NavMeshAgent
		if (gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> () != null) {
			agent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ();
			agent.speed = unit.moveSpeed;
			unit.avoidanceValue = Random.Range (10, 90);
			agent.avoidancePriority = unit.avoidanceValue;
			unit.curLoc = agent.transform.position;
		}

		if (gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle> () != null) {
			obstacle = gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle> ();
			obstacle.transform.position = gameObject.transform.position;
		}
	}
}
