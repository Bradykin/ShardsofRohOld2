﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContainer : MonoBehaviour {

	public string playerName;
	public Player player { get; set; }
	public GameObject buildToggle { get; private set; }
	public bool buildToggleActive { get; set; }
	public string buildToggleSetting { get; set; }
	private LayerMask buildToggleMask { get; set; }

	// Use this for initialization
	void Awake () {
		player = new Player (playerName);
		buildToggle = Instantiate (Resources.Load ("Prefabs/Miscellaneous/BuildToggle", typeof(GameObject)) as GameObject);
		buildToggleActive = false;
		buildToggleMask = 1 << LayerMask.NameToLayer ("Terrain");
	}
	
	// Update is called once per frame
	void Update () {
		if (buildToggle != null) {
			
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (MouseController.getMousePosition ());
			if (Physics.Raycast (ray, out hit, 1000, buildToggleMask)) {
				buildToggle.transform.position = (new Vector3 (hit.point.x, Terrain.activeTerrain.SampleHeight(hit.point), hit.point.z));
			}
		}

		player.update ();
	}

	public static IEnumerator WaitFrames (int _frames) {
		while (_frames > 0) {
			_frames--;
			yield return null;
		}


	}

	public void processRightClickUnitCommand (Vector3 _targetLoc, GameObject _clicked) {
		StartCoroutine (processRightClickUnitEnumerator (_targetLoc, _clicked));
	}

	IEnumerator processRightClickUnitEnumerator (Vector3 _targetLoc, GameObject _clicked) {
		foreach (var r in player.curUnitTarget) {
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

			if (GameManager.isEnemies (targetUnitContainer.unit.owner, GameManager.playerContainer.player)) {
				foreach (var r in GameManager.playerContainer.player.curUnitTarget) {
					r.unit.setAttackTarget (targetUnitContainer);
					r.checkAttackLogic ();
					r.removeBehaviourByType ("Idle");
					if (r.unit.isVillager == true) {
						r.unitBehaviours.Add (new IdleAttack (r));
					}
				}
			} else {
				foreach (var r in GameManager.playerContainer.player.curUnitTarget) {
					r.moveTowardCollider (targetUnitContainer.GetComponent<CapsuleCollider> ());
				}
			}
		}
		//Handle is clicked on building
		else if (_clicked.GetComponent<BuildingContainer> () != null) {
			BuildingContainer targetBuildingContainer = _clicked.GetComponent<BuildingContainer> ();

			if (targetBuildingContainer.building.isResource) {
				foreach (var r in GameManager.playerContainer.player.curUnitTarget) {
					if (r.unit.isVillager == true) {
						r.unit.setAttackTarget (targetBuildingContainer);
						r.removeBehaviourByType ("Idle");
						r.unitBehaviours.Add (new IdleGather (r));
					} else {
						r.moveTowardCollider (targetBuildingContainer.GetComponent<BoxCollider> ());
					}
				}
			} else if (GameManager.isEnemies (targetBuildingContainer.building.owner, GameManager.playerContainer.player)) {
				foreach (var r in GameManager.playerContainer.player.curUnitTarget) {
					r.unit.setAttackTarget (targetBuildingContainer);
					r.checkAttackLogic ();
					r.removeBehaviourByType ("Idle");
					if (r.unit.isVillager == true) {
						r.unitBehaviours.Add (new IdleAttack (r));
					}
				}
			} else {
				foreach (var r in GameManager.playerContainer.player.curUnitTarget) {
					if (r.unit.isVillager == true && targetBuildingContainer.building.owner == r.unit.owner && targetBuildingContainer.building.isBuilt == false) {
						r.unit.setAttackTarget (targetBuildingContainer);
						r.removeBehaviourByType ("Idle");
						r.unitBehaviours.Add (new IdleBuild (r));
					} else {
						r.moveTowardCollider (targetBuildingContainer.GetComponent<BoxCollider> ());
					}
				}
			}
		}
		//Handle if clicked on nothing
		else {
			player.processFormationMovement (_targetLoc);
		}

		yield return null;
	}
}
