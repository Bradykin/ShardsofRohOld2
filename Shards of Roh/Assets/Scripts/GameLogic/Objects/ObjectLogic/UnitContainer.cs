using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitContainer : ObjectContainer {

	public Unit unit { get; set; }
	public List<Behaviours> unitBehaviours { get; private set; }

	void Start () {
		//Set values for preset units
		setup ();
		unitBehaviours = new List<Behaviours> ();

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

		//Set values of NavMeshAgent
		if (gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> () != null) {
			gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().speed = unit.moveSpeed;
			gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().Warp (gameObject.transform.position);
			//unit.avoidanceValue = Random.Range (0, 100);
			unit.avoidanceValue = 50;
			gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().avoidancePriority = unit.avoidanceValue;
			unit.curLoc = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().transform.position;
		}

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
		if (unit.isVillager == true) {
			unitBehaviours.Add (new HitFlee (this));
		} else {
			unitBehaviours.Add (new PassiveAttack (this));
		}
	}

	void Update () {
		unit.update ();

		checkMoveLogic ();
		checkAttackLogic ();
		checkBehaviourLogic ();

		if (unit.isDead == true) {
			gameObject.GetComponent<CapsuleCollider> ().enabled = false;
			gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().enabled = false;
			gameObject.GetComponent<Animator> ().SetBool ("isDead", true);
			gameObject.GetComponent<Animator> ().SetInteger ("deathAnimation", Random.Range (0, 2));
			if (gameObject.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("PostDeath")) {
				GameManager.destroyUnit (this, unit.owner);
			}
		}
	}

	public void checkMoveLogic () {
		//Update curLoc and isMoving
		if (gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> () != null) {
			unit.curLoc = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().transform.position;
			if (gameObject.GetComponent<Animator> () != null) {
				if (Vector3.Distance (gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().transform.position, gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().destination) <= 0.1 || gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().enabled == false) {
					gameObject.GetComponent<Animator> ().SetBool ("isMoving", false);
					gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().avoidancePriority = 0;
					navMeshToggle ("Obstacle");
					unit.isMoving = false;
				} else {
					gameObject.GetComponent<Animator> ().SetBool ("isMoving", true);
					gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().avoidancePriority = unit.avoidanceValue;
					navMeshToggle ("Agent");
					unit.isMoving = true;
				}

				if (unit.isCombatTimer > 0) {
					gameObject.GetComponent<Animator> ().SetBool ("isCombat", true);
				} else {
					gameObject.GetComponent<Animator> ().SetBool ("isCombat", false);
				}
			} else {
				print ("Missing Animator - UnitContainer");
			}
		} else {
			print ("Missing NavMeshAgent - UnitContainer");
		}
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
				if (gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().enabled == true) {
					gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().destination = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().transform.position;
				}
				lookDirection (GetComponent<CapsuleCollider> ().bounds.center, unit.unitTarget.GetComponent<CapsuleCollider> ().bounds.center);
				gameObject.GetComponent<Animator> ().SetBool ("isAttacking", true);
				unit.isAttacking = true;
				if (unit.attackUnit () == true) {
					unit.unitTarget.unit.getHit (this, unit.attack);
				}
			} else {
				navMeshToggle ("Agent");
				if (gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().enabled == true) {
					gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().destination = unit.unitTarget.GetComponent<CapsuleCollider> ().ClosestPoint (point1);
				}
				gameObject.GetComponent<Animator> ().SetBool ("isAttacking", false);
				unit.isAttacking = false;
			}
		} else if (unit.buildingTarget != null && unit.buildingTarget.building.isDead == false) {
			Vector3 point1 = GetComponent<CapsuleCollider> ().bounds.center;
			Vector3 point2 = unit.buildingTarget.GetComponent<BoxCollider> ().ClosestPoint (GetComponent<CapsuleCollider> ().bounds.center);
			point1.y = 0;
			point2.y = 0;
			if (Vector3.Distance (point1, point2) - GetComponent<CapsuleCollider> ().radius <= unit.attackRange) {
				if (gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().enabled == true) {
					gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().ResetPath ();
				}
				lookDirection (GetComponent<CapsuleCollider> ().bounds.center, unit.buildingTarget.GetComponent<BoxCollider> ().bounds.center);
				gameObject.GetComponent<Animator> ().SetBool ("isAttacking", true);
				unit.isAttacking = true;
				if (unit.attackBuilding () == true) {
					unit.buildingTarget.building.getHit (this, unit.attack);
				}
			} else {
				if (gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().enabled == true) {
					gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().destination = point2;
				}
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
		for (int i = 0; i < unitBehaviours.Count; i++) {
			if (unitBehaviours [i].active == false) {
				unitBehaviours.RemoveAt (i);
				i--;
			} else {
				unitBehaviours [i].enact ();
			}
		}

		//Reset values tracked for behaviour logic
		unit.gotHit = false;
		unit.gotHitBy = null;
	}

	public void moveToLocation (Vector3 targetLoc) {
		gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().SetDestination (targetLoc);
	}

	public void moveTowardCollider (Collider collider) {
		moveToLocation (collider.ClosestPoint (unit.curLoc));
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
		if (_mode == "Agent") {
			if (gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle> () != null) {
				gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle> ().enabled = false;
			}
			if (gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> () != null) {
				gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().enabled = true;
			}
		} else if (_mode == "Obstacle") {
			if (gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> () != null) {
				gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().enabled = false;
			}
			if (gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle> () != null) {
				gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle> ().enabled = true;
			}
		}
	}

	public void removeBehaviourByType (string _type) {
		foreach (var r in unitBehaviours) {
			if (r.name.Contains (_type)) {
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
}
