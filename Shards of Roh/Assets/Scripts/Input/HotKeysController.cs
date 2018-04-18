using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class HotKeysController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//Send CameraController information on WASD input
		cameraMovement ();

		//Check for using the clicked thing's selected ability
		selectedAbilities ();

		//For any mid-implementation or testing hotkeys
		testKeys ();
	}

	//Process camera moveDirection for CameraController based on WASD
	private void cameraMovement () {
		bool wKey = false;
		bool aKey = false;
		bool sKey = false;
		bool dKey = false;
		if (Input.GetKey (KeyCode.W)) {
			wKey = true;
		}

		if (Input.GetKey (KeyCode.A)) {
			aKey = true;
		}

		if (Input.GetKey (KeyCode.S)) {
			sKey = true;
		}

		if (Input.GetKey (KeyCode.D)) {
			dKey = true;
		}

		if (wKey && sKey) {
			wKey = false;
			sKey = false;
		}

		if (aKey && dKey) {
			aKey = false;
			dKey = false;
		}

		CameraDirection key = CameraDirection.None;
		if (wKey && !aKey && !sKey && !dKey) {
			key = CameraDirection.Front;
		} else if (wKey && aKey && !sKey && !dKey) {
			key = CameraDirection.FrontLeft;
		} else if (!wKey && aKey && !sKey && !dKey) {
			key = CameraDirection.Left;
		} else if (!wKey && aKey && sKey && !dKey) {
			key = CameraDirection.BackLeft;
		} else if (!wKey && !aKey && sKey && !dKey) {
			key = CameraDirection.Back;
		} else if (!wKey && !aKey && sKey && dKey) {
			key = CameraDirection.BackRight;
		} else if (!wKey && !aKey && !sKey && dKey) {
			key = CameraDirection.Right;
		} else if (wKey && !aKey && !sKey && dKey) {
			key = CameraDirection.FrontRight;
		}
		CameraController.setMoveDirection (key);
	}

	//Activate abilities of whatever is selected
	private void selectedAbilities () {
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			if (GameManager.player.getPlayer ().getCurUnitTarget ().Count > 0) {
				//This shouldn't check first in list, eventually should use some other logic system
				if (GameManager.player.getPlayer ().getCurUnitTarget (0).getUnit ().getAbility (0) != null) {
					GameManager.player.getPlayer ().getCurUnitTarget (0).getUnit ().getAbility (0).enact (GameManager.player.getPlayer ());
				}
			} else if (GameManager.player.getPlayer ().getCurBuildingTarget ().Count > 0) {
				//This shouldn't check first in list, eventually should use some other logic system
				if (GameManager.player.getPlayer ().getCurBuildingTarget (0).getBuilding ().getAbility (0) != null) {
					GameManager.player.getPlayer ().getCurBuildingTarget (0).getBuilding ().getAbility (0).enact (GameManager.player.getPlayer ());
				}
			}
		}

		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			if (GameManager.player.getPlayer ().getCurUnitTarget ().Count > 0) {
				//This shouldn't check first in list, eventually should use some other logic system
				if (GameManager.player.getPlayer ().getCurUnitTarget (0).getUnit ().getAbility (1) != null) {
					GameManager.player.getPlayer ().getCurUnitTarget (0).getUnit ().getAbility (1).enact (GameManager.player.getPlayer ());
				}
			} else if (GameManager.player.getPlayer ().getCurBuildingTarget ().Count > 0) {
				//This shouldn't check first in list, eventually should use some other logic system
				if (GameManager.player.getPlayer ().getCurBuildingTarget (0).getBuilding ().getAbility (1) != null) {
					GameManager.player.getPlayer ().getCurBuildingTarget (0).getBuilding ().getAbility (1).enact (GameManager.player.getPlayer ());
				}
			}
		}
	}

	//Any test hotkeys or mid-implementation features go here as to not get interwoven with other systems
	private void testKeys () {
		if (Input.GetKeyDown (KeyCode.I)) {
			FormationController.formationMode = 0;
		}
		if (Input.GetKeyDown (KeyCode.O)) {
			foreach (var r in GameManager.player.getPlayer ().getCurUnitTarget ()) {
				r.GetComponent<Animator> ().SetBool ("isCharge", true);
			}
		}
		if (Input.GetKeyDown (KeyCode.P)) {
			foreach (var r in GameManager.player.getPlayer ().getCurUnitTarget ()) {
				r.GetComponent<Animator> ().SetBool ("isCharge", false);
			}
		}
		if (Input.GetKeyDown (KeyCode.L)) {
			foreach (var r in GameManager.player.getPlayer ().getCurUnitTarget ()) {
				r.GetComponent<Animator> ().SetBool ("isCombat", true);
			}
		}
		if (Input.GetKeyDown (KeyCode.K)) {
			foreach (var r in GameManager.player.getPlayer ().getCurUnitTarget ()) {
				r.GetComponent<Animator> ().SetBool ("isCombat", false);
			}
		}

		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (MouseController.getMousePosition());
			if (Physics.Raycast (ray, out hit, 1000)) {
				Unit newUnit = ObjectFactory.createUnitByName ("Villager", GameManager.addPlayerToGame("Player"));
				GameObject instance = Instantiate (Resources.Load (newUnit.getPrefabPath (), typeof(GameObject)) as GameObject);
				instance.GetComponent<UnitContainer> ().setUnit (newUnit);
				if (instance.GetComponent<UnityEngine.AI.NavMeshAgent> () != null) {
					instance.GetComponent<UnityEngine.AI.NavMeshAgent> ().Warp (new Vector3 (hit.point.x, Terrain.activeTerrain.SampleHeight(hit.point), hit.point.z));
				}

				GameManager.addPlayerToGame ("Player").addUnitToPlayer (instance.GetComponent<UnitContainer> ());

			} else {
				print ("Click off map - HotKeysController");
			}
		}

		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (MouseController.getMousePosition());
			if (Physics.Raycast (ray, out hit, 1000)) {
				Unit newUnit = ObjectFactory.createUnitByName ("LightCavalry", GameManager.addPlayerToGame("Player"));
				GameObject instance = Instantiate (Resources.Load (newUnit.getPrefabPath (), typeof(GameObject)) as GameObject);

				instance.GetComponent<UnitContainer> ().setUnit (newUnit);
				if (instance.GetComponent<UnityEngine.AI.NavMeshAgent> () != null) {
					instance.GetComponent<UnityEngine.AI.NavMeshAgent> ().Warp (new Vector3 (hit.point.x, Terrain.activeTerrain.SampleHeight(hit.point), hit.point.z));
				}

				GameManager.addPlayerToGame ("Player").addUnitToPlayer (instance.GetComponent<UnitContainer> ());

			} else {
				print ("Click off map - HotKeysController");
			}
		}

		if (Input.GetKeyDown (KeyCode.Alpha5)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (MouseController.getMousePosition());
			if (Physics.Raycast (ray, out hit, 1000)) {
				Building newBuilding = ObjectFactory.createBuildingByName ("Gold", GameManager.addPlayerToGame("Nature"));
				GameObject instance = Instantiate (Resources.Load (newBuilding.getPrefabPath (), typeof(GameObject)) as GameObject);
				instance.GetComponent<BuildingContainer> ().setBuilding (newBuilding);
				if (instance.GetComponent<UnityEngine.AI.NavMeshObstacle> () != null) {
					instance.GetComponent<UnityEngine.AI.NavMeshObstacle> ().transform.position = (new Vector3 (hit.point.x, Terrain.activeTerrain.SampleHeight(hit.point), hit.point.z));
				}

				GameManager.addPlayerToGame ("Nature").addBuildingToPlayer (instance.GetComponent<BuildingContainer> ());

			} else {
				print ("Click off map - HotKeysController");
			}
		}

		if (Input.GetKeyDown (KeyCode.Alpha6)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (MouseController.getMousePosition());
			if (Physics.Raycast (ray, out hit, 1000)) {
				Unit newUnit = ObjectFactory.createUnitByName ("Swordsman", GameManager.addPlayerToGame("Player"));
				GameObject instance = Instantiate (Resources.Load (newUnit.getPrefabPath (), typeof(GameObject)) as GameObject);

				instance.GetComponent<UnitContainer> ().setUnit (newUnit);
				if (instance.GetComponent<UnityEngine.AI.NavMeshAgent> () != null) {
					instance.GetComponent<UnityEngine.AI.NavMeshAgent> ().Warp (new Vector3 (hit.point.x, Terrain.activeTerrain.SampleHeight(hit.point), hit.point.z));
				}

				GameManager.addPlayerToGame ("Player").addUnitToPlayer (instance.GetComponent<UnitContainer> ());

			} else {
				print ("Click off map - HotKeysController");
			}
		}
	}
}
