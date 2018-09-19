using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//CreateObjects class is a Strategy that, given the instructions on what units/buildings/research to create, determines where to create them.
//Units/Research are chosen based on the available buildings that can make that unit
//Buildings are placed using an algorithm that builds them nearby existing buildings
public class CreateObjects : Strategies {

	public CreateObjects (AIController _AI) {
		name = "CreateObjects";
		active = true;
		AI = _AI; 
	}

	public override void enact () {
		if (AI.objectCreationPriorities.Count > 0) {
			if (AI.player.resource.hasEnough (AI.objectCreationPriorities [0].cost)) {
				if (AI.objectCreationPriorities [0] is Building) {
					tryPlaceBuilding (chooseBuildingLocation ());
				} else if (AI.objectCreationPriorities [0] is Unit) {
					tryMakeUnit (AI.objectCreationPriorities [0].name);
				} else {
					GameManager.print ("WRONG");
				}
			}
		}
	}

	public void tryPlaceBuilding (List<Vector3> buildingLocations) {
		if (buildingLocations.Count > 0) {
			if (AI.player.createBuildingFoundation (AI.objectCreationPriorities [0].name, buildingLocations [0]) == true) {
				AI.objectCreationPriorities.RemoveAt (0);
			} else {
				buildingLocations.RemoveAt (0);
				tryPlaceBuilding (buildingLocations);
			}
		}
	}

	public List<Vector3> chooseBuildingLocation () {
		List<Vector3> positionsAround = positionsAroundBuilding ();

		//Current problems: PositionsAround contains spots that overlap with other buildings. Checking this many buildings for whether they are colliding with something is expensive.
		//Current attempt: make a sublist of positionsAround randomly, then order them by which is closest to the current buildings.

		List<Vector3> positionsOptions = new List<Vector3> ();
		List<float> positionsDistance = new List<float> ();

		for (int i = 0; i < 20; i++) {
			positionsOptions.Add (positionsAround [Random.Range (0, positionsAround.Count - 1)]);

			float distanceToBase = 0;
			foreach (var b in AI.player.visibleObjects.visiblePlayerBuildings) {
				distanceToBase += Vector3.SqrMagnitude (positionsOptions [i] - b.building.curLoc);
			}
			positionsDistance.Add (distanceToBase);
		}

		float temp = 0;
		Vector3 vecTemp;
		for (int write = 0; write < positionsDistance.Count; write++) {
			for (int sort = 0; sort < positionsDistance.Count - 1; sort++) {
				if (positionsDistance [sort] > positionsDistance [sort + 1]) {
					temp = positionsDistance [sort + 1];
					positionsDistance [sort + 1] = positionsDistance [sort];
					positionsDistance [sort] = temp;

					vecTemp = positionsOptions [sort + 1];
					positionsOptions [sort + 1] = positionsOptions [sort];
					positionsOptions [sort] = vecTemp;
				}
			}
		}

		return positionsOptions;
	}

	public List<Vector3> positionsAroundBuilding () {
		List<Vector3> positionsAround = new List<Vector3> ();

		foreach (var r in AI.player.buildings) {
			for (int i = 0; i < 360; i += Random.Range (10, 20)) {
				Vector3 pos = new Vector3 ();
				float dist = Random.Range (15.0f, 25.0f);
				float angle = i; //degrees
				float a = angle * Mathf.PI / 180f;
				pos.x = Mathf.Sin (a) * dist + r.building.curLoc.x;
				pos.z = Mathf.Cos (a) * dist + r.building.curLoc.z;
				pos.y = Terrain.activeTerrain.SampleHeight (pos);
				positionsAround.Add (pos);
			}
		}

		return positionsAround;
	}
		
	public void tryMakeUnit (string _unitName) {
		if (AI.player.hasPopulationSpace (ObjectFactory.createUnitByName (_unitName, AI.player).populationCost)) {

			List<Building> possibleSpawnPoints = new List<Building> ();

			foreach (var r in AI.player.buildings) {
				if (r.building.hasAbility ("Spawn " + _unitName) && r.building.isBuilt) {
					possibleSpawnPoints.Add (r.building);
				}
			}

			//If there is a possible spawn point
			if (possibleSpawnPoints.Count > 0) {
				List<Building> spawnPoints = new List<Building> ();
				//Check if there is an unfinished batch to add new unit to
				foreach (var r in possibleSpawnPoints) {
					foreach (var u in r.unitQueue) {
						if (u.unit.name == _unitName && u.getFull () == false) {
							spawnPoints.Add (r);
							break;
						}
					}
				}

				if (spawnPoints.Count > 0) {
					spawnPoints [Random.Range (0, spawnPoints.Count - 1)].useAbility ("Spawn " + _unitName);
				} else {
					//If no unfinished batch to add new unit to, instead choose building with shortest existing queue
					int smallestQueue = 1000000;
					foreach (var r in possibleSpawnPoints) {
						if (r.unitQueue.Count < smallestQueue) {
							smallestQueue = r.unitQueue.Count;
						}
					}

					foreach (var r in possibleSpawnPoints) {
						if (r.unitQueue.Count == smallestQueue) {
							spawnPoints.Add (r);
						}
					}
					if (spawnPoints.Count == 0) {
						
					} else {
						spawnPoints [Random.Range (0, spawnPoints.Count - 1)].useAbility ("Spawn " + _unitName);
					}
				}
				AI.objectCreationPriorities.RemoveAt (0);
			}
		}
	}
}
