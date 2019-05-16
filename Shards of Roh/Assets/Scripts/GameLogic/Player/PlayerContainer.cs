using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class PlayerContainer : MonoBehaviour {

	public string playerName;
	public string buildToggleSetting { get; set; }
	public bool buildToggleActive { get; set; }
	public Player player { get; private set; }
	public GameObject buildToggle { get; private set; }
	public ObjectBase tooltipTarget { get; set; }
	private LayerMask buildToggleMask { get; set; }

	//Establish all playerContainer variables
	void Awake () {
		player = new Player (playerName, "Humans");
		buildToggle = Instantiate (Resources.Load ("Prefabs/Miscellaneous/BuildToggle", typeof(GameObject)) as GameObject);
		buildToggleActive = false;
		buildToggleMask = 1 << LayerMask.NameToLayer ("Terrain");
	}
	
	//Calls various update functions
	void Update () {
		buildToggleUpdate ();
		player.update ();
	}

	//Updates the location of the hovered building placement if buildToggle isn't null
	private void buildToggleUpdate () {
		if (buildToggle != null) {

			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (MouseController.getMousePosition ());
			if (Physics.Raycast (ray, out hit, 1000, buildToggleMask)) {
				buildToggle.transform.position = (new Vector3 (hit.point.x, Terrain.activeTerrain.SampleHeight(hit.point), hit.point.z));
			}
		}
	}

	//Process if a right click command is sent while buildings are selected
	public void processRightClickBuildingCommand (Vector3 _targetLoc, GameObject _clicked) {
		foreach (var r in player.curBuildingTarget) {
			//Handle if clicked on unit
			if (_clicked.GetComponent<UnitContainer> () != null) {
				r.building.wayPoint = _clicked.GetComponent<CapsuleCollider> ().ClosestPoint (r.building.curLoc);
				r.building.unitWayPointTarget = _clicked.GetComponent<UnitContainer> ();
				r.building.buildingWayPointTarget = null;
				r.setWaypointFlagActive (true);
			}
			//Handle if clicked on building
			else if (_clicked.GetComponent<BuildingContainer> () != null) {
				r.building.wayPoint = _clicked.GetComponent<BoxCollider> ().ClosestPoint (r.building.curLoc);
				r.building.unitWayPointTarget = null;
				r.building.buildingWayPointTarget = _clicked.GetComponent<BuildingContainer> ();
				if (r == _clicked.GetComponent<BuildingContainer> ()) {
					r.setWaypointFlagActive (false);
				} else {
					r.setWaypointFlagActive (true);
				}
			}
			//Handle if clicked on nothing
			else {
				r.building.wayPoint = _targetLoc;
				r.building.unitWayPointTarget = null;
				r.building.buildingWayPointTarget = null;
				r.setWaypointFlagActive (true);
			}
				
			r.setWaypointFlagLocation (r.building.wayPoint);
		}
	}

	//Start Coroutine for processRightClickUnitEnumerator
	public void processRightClickUnitCommand (Vector3 _targetLoc, GameObject _clicked, bool _isWayPointing) {

		StartCoroutine (processRightClickUnitEnumerator (_targetLoc, _clicked, _isWayPointing));
	}

	//Process if a right click command is sent while units are selected
	IEnumerator processRightClickUnitEnumerator (Vector3 _targetLoc, GameObject _clicked, bool _isWayPointing) {
		foreach (var r in player.curUnitTarget) {
			r.unit.isCommandedRecently = 0.25f;

			if (r.obstacle.enabled == true) {
				r.obstacle.enabled = false;
			}
		}

		yield return null;

		foreach (var r in player.curUnitTarget) {
			if (r.agent.enabled == false) {
				r.agent.enabled = true;
			}
		}

		//Handle if clicked on unit
		if (_clicked.GetComponent<UnitContainer> () != null) {
			UnitContainer targetUnitContainer = _clicked.GetComponent<UnitContainer> ();

			foreach (var r in GameManager.playerContainer.player.curUnitTarget) {
				r.setWaypointFlagLocation (targetUnitContainer.unit.curLoc);
				r.setWaypointFlagActive (true);
			}

			if (GameManager.isEnemies (targetUnitContainer.unit.owner, GameManager.playerContainer.player)) {
				foreach (var r in GameManager.playerContainer.player.curUnitTarget) {
					r.unit.setAttackTarget (targetUnitContainer);
					r.checkAttackLogic ();
					if (r.unit.unitType == UnitType.Villager) {
						r.unitBehaviours.Add (new IdleAttack (r));
					}
				}
			} else {
				foreach (var r in GameManager.playerContainer.player.curUnitTarget) {
					r.moveTowardCollider (true, targetUnitContainer.GetComponent<CapsuleCollider> ());
				}
			}
		}
		//Handle if clicked on building
		else if (_clicked.GetComponent<BuildingContainer> () != null) {
			BuildingContainer targetBuildingContainer = _clicked.GetComponent<BuildingContainer> ();

			foreach (var r in GameManager.playerContainer.player.curUnitTarget) {
				r.setWaypointFlagLocation (targetBuildingContainer.building.curLoc);
				r.setWaypointFlagActive (true);
			}

			if (targetBuildingContainer.building.isResource) {
				foreach (var r in GameManager.playerContainer.player.curUnitTarget) {
					if (r.unit.unitType == UnitType.Villager) {
						r.unit.setAttackTarget (targetBuildingContainer);
						r.unitBehaviours.Add (new IdleGather (r));
					} else {
						r.moveTowardCollider (true, targetBuildingContainer.GetComponent<BoxCollider> ());
					}
				}
			} else if (GameManager.isEnemies (targetBuildingContainer.building.owner, GameManager.playerContainer.player)) {
				foreach (var r in GameManager.playerContainer.player.curUnitTarget) {
					r.unit.setAttackTarget (targetBuildingContainer);
					r.checkAttackLogic ();
					if (r.unit.unitType == UnitType.Villager) {
						r.unitBehaviours.Add (new IdleAttack (r));
					}
				}
			} else {
				foreach (var r in GameManager.playerContainer.player.curUnitTarget) {
					if (r.unit.unitType == UnitType.Villager && targetBuildingContainer.building.owner == r.unit.owner && targetBuildingContainer.building.isBuilt == false) {
						r.unit.setAttackTarget (targetBuildingContainer);
						r.unitBehaviours.Add (new IdleBuild (r));
					} else {
						r.moveTowardCollider (true, targetBuildingContainer.GetComponent<BoxCollider> ());
					}
				}
			}
		}
		//Handle if clicked on nothing
		else {
			player.processFormationMovement (_targetLoc, _isWayPointing);
		}

		yield return null;
	}
}
