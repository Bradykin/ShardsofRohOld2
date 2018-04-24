using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContainer : MonoBehaviour {

	public string playerName;
	public GameObject buildToggle;
	public bool buildToggleActive;
	public string buildToggleSetting;
	LayerMask buildToggleMask;

	private Player player;

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

		player.updatePopulation ();
		player.updatemaxPopulation ();
	}

	public void processRightClickUnitCommand (Vector3 targetLoc, GameObject clicked) {
		//Handle if clicked on unit
		if (clicked.GetComponent<UnitContainer> () != null) {
			targetLoc = clicked.GetComponent<UnitContainer> ().getUnit ().getCurLoc ();
			foreach (var r in GameManager.player.getPlayer ().getCurUnitTarget ()) {
				if (GameManager.isEnemies (clicked.GetComponent<UnitContainer> ().getUnit ().getOwner (), GameManager.player.getPlayer ())) {
					r.getUnit ().setAttackTarget (clicked.GetComponent<UnitContainer> ());
					r.checkAttackLogic ();
				}
			}
		}
		//Handle is clicked on building
		else if (clicked.GetComponent<BuildingContainer> () != null) {
			targetLoc = clicked.GetComponent<BuildingContainer> ().getBuilding ().getCurLoc ();
			foreach (var r in GameManager.player.getPlayer ().getCurUnitTarget ()) {
				if (r.getUnit ().getVillager () == true) {
					//This should have expanded logic
					r.getUnit ().setAttackTarget (clicked.GetComponent<BuildingContainer> ());
				} else {
					if (clicked.GetComponent<BuildingContainer> ().getBuilding ().getIsResource () == false) {
						if (GameManager.isEnemies (clicked.GetComponent<BuildingContainer> ().getBuilding ().getOwner (), GameManager.player.getPlayer ())) {
							r.getUnit ().setAttackTarget (clicked.GetComponent<BuildingContainer> ());
							r.checkAttackLogic ();
						}
					}
				}
			}
		}
		//Handle if clicked on nothing
		else {
			getPlayer ().processFormationMovement (targetLoc);
		}
	}

	public Player getPlayer () {
		return player;
	}
}
