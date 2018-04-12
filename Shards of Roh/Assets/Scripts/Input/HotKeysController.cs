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

		if (Input.GetKeyDown (KeyCode.Alpha1)) {
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

		if (Input.GetKeyDown (KeyCode.Alpha2)) {
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
				Building newBuilding = ObjectFactory.createBuildingByName ("WatchTower", GameManager.addPlayerToGame ("Player"), false);
				GameObject instance = Instantiate (Resources.Load (newBuilding.getPrefabPath (), typeof(GameObject)) as GameObject);
				instance.GetComponent<BuildingContainer> ().setBuilding (newBuilding);
				if (instance.GetComponent<UnityEngine.AI.NavMeshObstacle> () != null) {
					instance.GetComponent<UnityEngine.AI.NavMeshObstacle> ().transform.position = (new Vector3 (hit.point.x, Terrain.activeTerrain.SampleHeight(hit.point), hit.point.z));
				}

				instance.transform.GetChild (1).gameObject.SetActive (true);
				instance.transform.GetChild (2).gameObject.SetActive (false);
				instance.GetComponent<BoxCollider> ().center = instance.transform.GetChild (1).GetComponent <BoxCollider> ().center;
				instance.GetComponent<BoxCollider> ().size = instance.transform.GetChild (1).GetComponent <BoxCollider> ().size;
				instance.GetComponent<UnityEngine.AI.NavMeshObstacle> ().center = instance.transform.GetChild (1).GetComponent<UnityEngine.AI.NavMeshObstacle> ().center;
				instance.GetComponent<UnityEngine.AI.NavMeshObstacle> ().size = instance.transform.GetChild (1).GetComponent<UnityEngine.AI.NavMeshObstacle> ().size;

				GameManager.addPlayerToGame ("Player").addBuildingToPlayer (instance.GetComponent<BuildingContainer> ());

			} else {
				print ("Click off map - HotKeysController");
			}
		}

		if (Input.GetKeyDown (KeyCode.Alpha5)) {
			if (GameManager.player.buildToggleActive == true) {
				GameManager.player.buildToggleActive = false;
				GameManager.player.buildToggle.transform.GetChild (0).gameObject.SetActive (false);
			} else {
				GameManager.player.buildToggleActive = true;
				GameManager.player.buildToggle.transform.GetChild (0).gameObject.SetActive (true);
			}
		}

		if (Input.GetKeyDown (KeyCode.Alpha6)) {
			if (GameManager.player.getPlayer ().getCurUnitTarget ().Count > 0) {
				if (GameManager.player.getPlayer ().getCurUnitTarget (0).getUnit ().getAbility (0) != null) {
					GameManager.player.getPlayer ().getCurUnitTarget (0).getUnit ().getAbility (0).enact (GameManager.player.getPlayer ());
				} else {
					print ("Check1");
				}
			} else {
				print ("Check2");
			}
		}
	}
}
