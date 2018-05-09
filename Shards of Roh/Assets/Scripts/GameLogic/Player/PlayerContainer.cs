using System.Collections;
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

	public void processRightClickUnitCommand (Vector3 _targetLoc, GameObject _clicked) {
		//Handle if clicked on unit
		if (_clicked.GetComponent<UnitContainer> () != null) {
			UnitContainer targetUnit = _clicked.GetComponent<UnitContainer> ();
			Vector3 targetLoc = _clicked.GetComponent<UnitContainer> ().unit.curLoc;
			foreach (var r in GameManager.player.player.curUnitTarget) {
				if (GameManager.isEnemies (clicked.GetComponent<UnitContainer> ().unit.owner, GameManager.player.player)) {
					r.unit.setAttackTarget (clicked.GetComponent<UnitContainer> ());
					r.checkAttackLogic ();
					r.removeBehaviourByType ("Idle");
				}
			}
		}
		//Handle is clicked on building
		else if (clicked.GetComponent<BuildingContainer> () != null) {
			targetLoc = clicked.GetComponent<BuildingContainer> ().building.curLoc;
			foreach (var r in GameManager.player.player.curUnitTarget) {
				if (r.unit.isVillager == true) {
					//This should have expanded logic
					if (clicked.GetComponent<BuildingContainer> ().building.isResource) {
						r.unit.setAttackTarget (clicked.GetComponent<BuildingContainer> ());
						r.removeBehaviourByType ("Idle");
						r.unitBehaviours.Add (new IdleGather ());
					} else if (clicked.GetComponent<BuildingContainer> ().building.owner.name == r.unit.owner.name) {
						r.unit.setAttackTarget (clicked.GetComponent<BuildingContainer> ());
						r.removeBehaviourByType ("Idle");
						r.unitBehaviours.Add (new IdleBuild ());
					} else if (GameManager.isEnemies (clicked.GetComponent<BuildingContainer> ().building.owner, GameManager.player.player) == true) {
						r.unit.setAttackTarget (clicked.GetComponent<BuildingContainer> ());
						r.removeBehaviourByType ("Idle");
						r.unitBehaviours.Add (new IdleAttack ());
					}
				} else {
					if (clicked.GetComponent<BuildingContainer> ().building.isResource == false) {
						if (GameManager.isEnemies (clicked.GetComponent<BuildingContainer> ().building.owner, GameManager.player.player)) {
							r.unit.setAttackTarget (clicked.GetComponent<BuildingContainer> ());
							r.checkAttackLogic ();
							r.removeBehaviourByType ("Idle");
							r.unitBehaviours.Add (new IdleAttack ());
						}
					}
				}
			}
		}
		//Handle if clicked on nothing
		else {
			player.processFormationMovement (targetLoc);
		}
	}
}
