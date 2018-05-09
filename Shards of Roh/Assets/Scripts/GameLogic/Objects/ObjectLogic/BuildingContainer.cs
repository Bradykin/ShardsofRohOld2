using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingContainer : ObjectContainer {

	public Building building { get; set; }
	private float unitQueueTimer { get; set; }
	private float researchQueueTimer { get; set; }

	// Use this for initialization
	void Start () {
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
	}

	// Update is called once per frame
	void Update () {
		building.update ();
		checkQueues ();

		if (building.isDead == true) {
			GameManager.destroyBuilding (this, building.owner);
		}

		if (building.toBuild == true) {
			building.isBuilt = true;
			building.toBuild = false;

			if (gameObject.transform.GetChild (0).gameObject.name == "Model" && gameObject.transform.GetChild (1).gameObject.name == "Foundation") {
				gameObject.transform.GetChild (0).gameObject.SetActive (true);
				gameObject.transform.GetChild (1).gameObject.SetActive (false);
				gameObject.GetComponent<BoxCollider> ().center = gameObject.transform.GetChild (0).GetComponent <BoxCollider> ().center;
				gameObject.GetComponent<BoxCollider> ().size = gameObject.transform.GetChild (0).GetComponent <BoxCollider> ().size;
				gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle> ().center = gameObject.transform.GetChild (0).GetComponent<UnityEngine.AI.NavMeshObstacle> ().center;
				gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle> ().size = gameObject.transform.GetChild (0).GetComponent<UnityEngine.AI.NavMeshObstacle> ().size;
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
			unitQueueTimer -= Time.deltaTime;
		} else {
			unitQueueTimer = 5.0f;
		}

		if (unitQueueTimer <= 0.0f && building.unitQueue.Count > 0) {
			building.createUnit ();
			unitQueueTimer = 5.0f;
		}

		//Update the progress of the building's researchQueue
		if (building.researchQueue.Count > 0) {
			researchQueueTimer -= Time.deltaTime;
		} else {
			researchQueueTimer = 5.0f;
		}

		if (researchQueueTimer <= 0.0f && building.researchQueue.Count > 0) {
			building.createResearch ();
			researchQueueTimer = 5.0f;
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
