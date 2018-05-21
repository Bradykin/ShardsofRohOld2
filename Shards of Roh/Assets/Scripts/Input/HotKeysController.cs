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

		if (Input.GetKeyDown (KeyCode.Space)) {
			if (GameManager.playerContainer.player.curUnitTarget.Count > 0) {
				CameraController.moveToSelected (GameManager.playerContainer.player.curUnitTarget [0].unit);
			} else if (GameManager.playerContainer.player.curBuildingTarget.Count > 0) {
				CameraController.moveToSelected (GameManager.playerContainer.player.curBuildingTarget [0].building);
			}
		}
	}

	//Activate abilities of whatever is selected
	private void selectedAbilities () {
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			GameManager.playerContainer.player.useCurTargetAbility (0);
		}

		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			GameManager.playerContainer.player.useCurTargetAbility (1);
		}

		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			GameManager.playerContainer.player.useCurTargetAbility (2);
		}

		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			GameManager.playerContainer.player.useCurTargetAbility (3);
		}

		if (Input.GetKeyDown (KeyCode.Alpha5)) {
			GameManager.playerContainer.player.useCurTargetAbility (4);
		}

		if (Input.GetKeyDown (KeyCode.Alpha6)) {
			GameManager.playerContainer.player.useCurTargetAbility (5);
		}

		if (Input.GetKeyDown (KeyCode.Q)) {
			GameManager.playerContainer.player.useCurTargetAbility (6);
		}

		if (Input.GetKeyDown (KeyCode.E)) {
			GameManager.playerContainer.player.useCurTargetAbility (7);
		}

		if (Input.GetKeyDown (KeyCode.R)) {
			GameManager.playerContainer.player.useCurTargetAbility (8);
		}

		if (Input.GetKeyDown (KeyCode.T)) {
			GameManager.playerContainer.player.useCurTargetAbility (9);
		}
	}

	//Any test hotkeys or mid-implementation features go here as to not get interwoven with other systems
	private void testKeys () {

		if (Input.GetKeyDown (KeyCode.I)) {
			FormationController.formationMode = 0;
		}

		if (Input.GetKeyDown (KeyCode.O)) {
			FormationController.formationMode = 1;
		}

		if (Input.GetKeyDown (KeyCode.P)) {
			FormationController.formationMode = 2;
		}

		if (Input.GetKeyDown (KeyCode.Alpha7)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (MouseController.getMousePosition());
			if (Physics.Raycast (ray, out hit, 1000, GlobalVariables.defaultMask)) {
				Unit newUnit = ObjectFactory.createUnitByName ("Worker", GameManager.addPlayerToGame("Player"));
				GameObject instance = Instantiate (Resources.Load (newUnit.prefabPath, typeof(GameObject)) as GameObject);
				instance.transform.position = new Vector3 (hit.point.x, Terrain.activeTerrain.SampleHeight (hit.point), hit.point.z);

				instance.GetComponent<UnitContainer> ().unit = newUnit;

				GameManager.addPlayerToGame ("Player").units.Add (instance.GetComponent<UnitContainer> ());
					
			} else {
				print ("Click off map - HotKeysController");
			}
		}

		if (Input.GetKeyDown (KeyCode.Alpha8)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (MouseController.getMousePosition());
			if (Physics.Raycast (ray, out hit, 1000, GlobalVariables.defaultMask)) {
				Building newBuilding = ObjectFactory.createBuildingByName ("Food", GameManager.addPlayerToGame("Nature"));
				GameObject instance = Instantiate (Resources.Load (newBuilding.prefabPath, typeof(GameObject)) as GameObject);
				instance.GetComponent<BuildingContainer> ().building = newBuilding;
				if (instance.GetComponent<UnityEngine.AI.NavMeshObstacle> () != null) {
					instance.GetComponent<UnityEngine.AI.NavMeshObstacle> ().transform.position = (new Vector3 (hit.point.x, Terrain.activeTerrain.SampleHeight(hit.point), hit.point.z));
				}

				GameManager.addPlayerToGame ("Nature").buildings.Add (instance.GetComponent<BuildingContainer> ());

			} else {
				print ("Click off map - HotKeysController");
			}
		}

		if (Input.GetKeyDown (KeyCode.Alpha9)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (MouseController.getMousePosition());
			if (Physics.Raycast (ray, out hit, 1000, GlobalVariables.defaultMask)) {
				Building newBuilding = ObjectFactory.createBuildingByName ("Wood", GameManager.addPlayerToGame("Nature"));
				GameObject instance = Instantiate (Resources.Load (newBuilding.prefabPath, typeof(GameObject)) as GameObject);
				instance.GetComponent<BuildingContainer> ().building = newBuilding;
				if (instance.GetComponent<UnityEngine.AI.NavMeshObstacle> () != null) {
					instance.GetComponent<UnityEngine.AI.NavMeshObstacle> ().transform.position = (new Vector3 (hit.point.x, Terrain.activeTerrain.SampleHeight(hit.point), hit.point.z));
				}

				GameManager.addPlayerToGame ("Nature").buildings.Add (instance.GetComponent<BuildingContainer> ());

			} else {
				print ("Click off map - HotKeysController");
			}
		}

		if (Input.GetKeyDown (KeyCode.Alpha0)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (MouseController.getMousePosition());
			if (Physics.Raycast (ray, out hit, 1000, GlobalVariables.defaultMask)) {
				Building newBuilding = ObjectFactory.createBuildingByName ("Gold", GameManager.addPlayerToGame("Nature"));
				GameObject instance = Instantiate (Resources.Load (newBuilding.prefabPath, typeof(GameObject)) as GameObject);
				instance.GetComponent<BuildingContainer> ().building = newBuilding;
				if (instance.GetComponent<UnityEngine.AI.NavMeshObstacle> () != null) {
					instance.GetComponent<UnityEngine.AI.NavMeshObstacle> ().transform.position = (new Vector3 (hit.point.x, Terrain.activeTerrain.SampleHeight(hit.point), hit.point.z));
				}

				GameManager.addPlayerToGame ("Nature").buildings.Add (instance.GetComponent<BuildingContainer> ());

			} else {
				print ("Click off map - HotKeysController");
			}
		}

		if (Input.GetKeyDown (KeyCode.N)) {
			StartCoroutine ("Test");
		}
	}

	IEnumerator Test () {
		for (int i = 0; i < 20000; i++) {
			GameManager.print ("Number: " + i);
			yield return null;
		}
	}
}
