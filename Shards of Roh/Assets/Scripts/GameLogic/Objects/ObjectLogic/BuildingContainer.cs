﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingContainer : ObjectContainer {

	private Building building;

	// Use this for initialization
	void Start () {
		if (presetOwnerName != "") {
			Player newPlayer = GameManager.addPlayerToGame (presetOwnerName);
			setBuilding (ObjectFactory.createBuildingByName (gameObject.name, newPlayer));
			if (newPlayer.getBuildings ().Contains (this) == false) {
				newPlayer.addBuildingToPlayer (this);
			}
		}

		if (gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle> () != null) {
			building.setCurLoc (gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle> ().transform.position);
			building.setColliderSize (gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle> ().size);
		}
	}

	// Update is called once per frame
	void Update () {
		building.update ();

		if (building.getDead () == true) {
			GameManager.print ("Destroy building");
			GameManager.destroyBuilding (this, building.getOwner ());
		}

		if (building.getToBuild () == true) {
			GameManager.print ("Build building");

			building.setIsBuilt (true);
			building.setToBuild (false);

			gameObject.transform.GetChild (1).gameObject.SetActive (false);
			gameObject.transform.GetChild (2).gameObject.SetActive (true);
			gameObject.GetComponent<BoxCollider> ().center = gameObject.transform.GetChild (2).GetComponent <BoxCollider> ().center;
			gameObject.GetComponent<BoxCollider> ().size = gameObject.transform.GetChild (2).GetComponent <BoxCollider> ().size;
			gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle> ().center = gameObject.transform.GetChild (2).GetComponent<UnityEngine.AI.NavMeshObstacle> ().center;
			gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle> ().size = gameObject.transform.GetChild (2).GetComponent<UnityEngine.AI.NavMeshObstacle> ().size;

			foreach (var r in building.getOwner ().getUnits ()) {
				if (r.getUnit ().getBuildingTarget () == this) {
					r.getUnit ().dropAttackTarget ();
				}
			}
		}
	}

	public void setBuilding (Building _building) {
		building = _building;
	}

	public Building getBuilding () {
		return building;
	}

	public void toggleBuilt (bool _built) {
		int toggleInt;
		building.setIsBuilt (_built);
		building.setToBuild (!_built);

		gameObject.transform.GetChild (1).gameObject.SetActive (!_built);
		gameObject.transform.GetChild (2).gameObject.SetActive (_built);

		if (_built == true) {
			toggleInt = 2;
		} else {
			toggleInt = 1;
		}

		gameObject.GetComponent<BoxCollider> ().center = gameObject.transform.GetChild (toggleInt).GetComponent <BoxCollider> ().center;
		gameObject.GetComponent<BoxCollider> ().size = gameObject.transform.GetChild (toggleInt).GetComponent <BoxCollider> ().size;
		gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle> ().center = gameObject.transform.GetChild (toggleInt).GetComponent<UnityEngine.AI.NavMeshObstacle> ().center;
		gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle> ().size = gameObject.transform.GetChild (toggleInt).GetComponent<UnityEngine.AI.NavMeshObstacle> ().size;

		if (_built == true) {
			foreach (var r in building.getOwner ().getUnits ()) {
				if (r.getUnit ().getBuildingTarget () == this) {
					r.getUnit ().dropAttackTarget ();
				}
			}
		}
	}
}