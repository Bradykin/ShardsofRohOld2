using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class BuildingContainer : ObjectContainer {

	public Building building { get; set; }
	private float unitQueueTimer { get; set; }
	private float researchQueueTimer { get; set; }
	public BoxCollider boxCollider { get; private set; }

	// Use this for initialization
	void Start () {
		setup ();

		unitQueueTimer = 5.0f;
		researchQueueTimer = 5.0f;

		if (presetOwnerName != "") {
			Player newPlayer = GameManager.addPlayerToGame (presetOwnerName);
			building = ObjectFactory.createBuildingByName (gameObject.name, newPlayer);
			if (newPlayer.buildings.Contains (this) == false) {
				newPlayer.buildings.Add (this);
			}
		}

		if (gameObject.transform.GetChild (3).name == "MinimapIcon") {
			gameObject.transform.GetChild (3).gameObject.SetActive (true);
			gameObject.transform.GetChild (3).gameObject.transform.position = new Vector3 (gameObject.transform.GetChild (3).gameObject.transform.position.x, 20, gameObject.transform.GetChild (3).gameObject.transform.position.z);
			gameObject.transform.GetChild (3).gameObject.transform.localScale = new Vector3 (10.0f, 0.01f, 10.0f);
		}

		if (gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle> () != null) {
			building.curLoc = gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle> ().transform.position;
			building.navColliderSize = gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle> ().size;
		}

		foreach (var r in gameObject.GetComponentsInChildren<MeshRenderer> ()) {
			if (r.material.name.Contains ("MinimapDefault")) {
				r.material = Resources.Load (PlayerMaterial.getMinimapColour (building.owner.name), typeof(Material)) as Material;
			}
		}
		foreach (var r in gameObject.GetComponentsInChildren<SkinnedMeshRenderer> ()) {
			if (r.material.name.Contains ("MinimapDefault")) {
				r.material = Resources.Load (PlayerMaterial.getMinimapColour (building.owner.name), typeof(Material)) as Material;
			}
		}

		boxCollider = GetComponent<BoxCollider> ();

		building.wayPoint = building.curLoc;
	}

	// Update is called once per frame
	void Update () {
		if (building.owner.name == "Player" && building.wayPoint == building.curLoc) {
			setWaypointFlagActive (false);
		}

		building.update ();
		checkQueues ();

		if (building.isDead == true) {
			foreach (var r in GameManager.playersInGame) {
				r.visibleObjects.rememberedResourceBuildings.Remove (this);
			}
			GameManager.destroyBuilding (this, building.owner);
		}

		if (building.toBuild == true) {
			building.isBuilt = true;
			building.toBuild = false;

			if (gameObject.transform.GetChild (0).gameObject.name == "Model" && gameObject.transform.GetChild (1).gameObject.name == "Foundation") {
				gameObject.transform.GetChild (0).gameObject.SetActive (true);
				gameObject.transform.GetChild (1).gameObject.SetActive (false);
			} else {
				GameManager.print ("Model Child Problem - BuildingContainer-1");
			}

			foreach (var r in building.owner.units) {
				if (r.unit.buildingTarget == this) {
					r.unit.dropAttackTarget ();
				}
			}
		}
	}

	protected void checkQueues () {
		//Update the progress of the building's unitQueue
		if (building.unitQueue.Count > 0) {
			unitQueueTimer += Time.deltaTime;
		} else {
			unitQueueTimer = 0.0f;
		}

		if (building.unitQueue.Count > 0) {
			if (unitQueueTimer >= building.unitQueue [0].unit.queueTime) {
				createUnit ();
				unitQueueTimer = 0.0f;
			}
		}

		//Update the progress of the building's researchQueue
		if (building.researchQueue.Count > 0) {
			researchQueueTimer += Time.deltaTime;
		} else {
			researchQueueTimer = 0.0f;
		}

		if (building.researchQueue.Count > 0) {
			if (researchQueueTimer >= building.researchQueue [0].research.queueTime) {
				building.createResearch ();
				researchQueueTimer = 0.0f;
			}
		}
	}

	public void createUnit () {
		//Set unit spawn location
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Camera.main.WorldToScreenPoint (building.wayPoint));
		if (Physics.Raycast (ray, out hit, 1000, GlobalVariables.defaultMask)) {
			if (building.unitQueue.Count > 0) {
				//Spawn units according to the size of the next unitQueue
				for (int i = 0; i < building.unitQueue [0].size; i++) {
					Unit newUnit = ObjectFactory.createUnitByName (building.unitQueue [0].unit.name, building.owner);
					//GameManager.print (newUnit.prefabPath);
					GameObject instance = GameManager.Instantiate (Resources.Load (newUnit.prefabPath, typeof(GameObject)) as GameObject);
					GameObject temp = instance.transform.GetChild (0).gameObject;

					temp.transform.position = GetComponent<BoxCollider> ().ClosestPoint (building.wayPoint);
					temp.GetComponent<UnitContainer> ().unit = newUnit;
					if (temp.GetComponent<UnitContainer> ().started == false) {
						temp.GetComponent<UnitContainer> ().setCleanUnitBehaviours ();
						temp.GetComponent<UnitContainer> ().setNavMeshProperties ();
					}
					StartCoroutine (sendUnitToDestination (temp.GetComponent<UnitContainer> (), building.wayPoint, hit.collider.gameObject));

					GameManager.addPlayerToGame (building.owner.name).units.Add (temp.GetComponent<UnitContainer> ());
				}
				building.unitQueue.RemoveAt (0);
			}
		}
	}

	IEnumerator sendUnitToDestination (UnitContainer _unit, Vector3 _targetLoc, GameObject _target) {
		yield return null;

		//Handle if waypoint on unit
		if (building.unitWayPointTarget != null) {
			_unit.setWaypointFlagLocation (building.unitWayPointTarget.unit.curLoc);
			if (GameManager.isEnemies (building.unitWayPointTarget.unit.owner, GameManager.playerContainer.player)) {
				_unit.unit.setAttackTarget (building.unitWayPointTarget);
				_unit.checkAttackLogic ();
				if (_unit.unit.unitType == UnitType.Villager) {
					_unit.unitBehaviours.Add (new IdleAttack (_unit));
				}
			} else {
				_unit.moveTowardCollider (false, building.unitWayPointTarget.GetComponent<CapsuleCollider> ());
			}
		}
		//Handle if waypoint on building
		else if (building.buildingWayPointTarget != null) {
			_unit.setWaypointFlagLocation (building.buildingWayPointTarget.building.curLoc);
			if (building.buildingWayPointTarget.building.isResource) {
				if (_unit.unit.unitType == UnitType.Villager) {
					_unit.unit.setAttackTarget (building.buildingWayPointTarget);
					_unit.unitBehaviours.Add (new IdleGather (_unit));
				} else {
					_unit.moveTowardCollider (false, building.buildingWayPointTarget.GetComponent<BoxCollider> ());
				}
			} else if (GameManager.isEnemies (building.buildingWayPointTarget.building.owner, GameManager.playerContainer.player)) {
				_unit.unit.setAttackTarget (building.buildingWayPointTarget);
				_unit.checkAttackLogic ();
				if (_unit.unit.unitType == UnitType.Villager) {
					_unit.unitBehaviours.Add (new IdleAttack (_unit));
				}
			} else {
				if (_unit.unit.unitType == UnitType.Villager && building.buildingWayPointTarget.building.owner == _unit.unit.owner && building.buildingWayPointTarget.building.isBuilt == false) {
					_unit.unit.setAttackTarget (building.buildingWayPointTarget);
					_unit.unitBehaviours.Add (new IdleBuild (_unit));
				} else {
					_unit.moveTowardCollider (false, building.buildingWayPointTarget.GetComponent<BoxCollider> ());
				}
			}
		}
		//Handle if waypoint on nothing 
		else {
			_unit.setWaypointFlagLocation (building.wayPoint);
			_unit.moveToLocation (false, building.wayPoint);
		}
	}

	public void toggleBuilt (bool _built) {
		int toggleInt;
		building.isBuilt = _built;
		building.toBuild = !_built;

		if (gameObject.transform.GetChild (0).gameObject.name == "Model" && gameObject.transform.GetChild (1).gameObject.name == "Foundation") {
			gameObject.transform.GetChild (0).gameObject.SetActive (_built);
			gameObject.transform.GetChild (1).gameObject.SetActive (!_built);

			if (_built == true) {
				toggleInt = 0;
			} else {
				toggleInt = 1;
			}

			gameObject.GetComponent<BoxCollider> ().center = gameObject.transform.GetChild (toggleInt).GetComponent <BoxCollider> ().center;
			gameObject.GetComponent<BoxCollider> ().size = gameObject.transform.GetChild (toggleInt).GetComponent <BoxCollider> ().size;
			gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle> ().center = gameObject.transform.GetChild (toggleInt).GetComponent<UnityEngine.AI.NavMeshObstacle> ().center;
			gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle> ().size = gameObject.transform.GetChild (toggleInt).GetComponent<UnityEngine.AI.NavMeshObstacle> ().size;
		} else {
			GameManager.print ("Model Child Problem - BuildingContainer-2");
		}

		if (_built == true) {
			foreach (var r in building.owner.units) {
				if (r.unit.buildingTarget == this) {
					r.unit.dropAttackTarget ();
				}
			}
		}
	}
}
