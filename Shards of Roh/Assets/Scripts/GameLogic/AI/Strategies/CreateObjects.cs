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

	//Check if the next object in the queue is affordable. If it is, determine what type of object it is, 
	public override void enact () {
		if (AI.creationQueue.Count > 0) {
			if (AI.player.resource.hasEnough (AI.creationQueue [0].getCost ())) {
				if (AI.creationQueue [0].type == "Unit") {
					tryMakeUnit (AI.creationQueue [0].objectValue.name);
				} else if (AI.creationQueue [0].type == "Building") {
					tryPlaceBuilding (chooseBuildingLocation ());
				} else if (AI.creationQueue [0].type == "Research") {
					tryQueueResearch (AI.creationQueue [0].research.name);
				} else {
					GameManager.print ("WRONG");
				}
			}
		}
	}

	//Find a place to make the unit, if one is available.
	//Compile a list of possible spawn points, then check if any of them have an active unfinished queue group
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

				//If there is an unfinished batch to add new unit to
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
						GameManager.print ("Something went wrong, I cut all my spawnPoints");
					} else {
						spawnPoints [Random.Range (0, spawnPoints.Count - 1)].useAbility ("Spawn " + _unitName);
					}
				}
				AI.creationQueue.RemoveAt (0);
			}
		}
	}

	//Find a place to put a new building
	//Use chooseBuildingLocations to generate a random list of locations, then try the locations one by one.
	public void tryPlaceBuilding (List<Vector3> buildingLocations) {
		if (buildingLocations.Count > 0) {
			if (AI.player.createBuildingFoundation (AI.creationQueue [0].objectValue.name, buildingLocations [0]) == true) {
				AI.creationQueue.RemoveAt (0);
			} else {
				buildingLocations.RemoveAt (0);
				tryPlaceBuilding (buildingLocations);
			}
		}
	}

	//Make a sublist of positionsAroundBuilding, then sort it based on distance from the current structures.
	public List<Vector3> chooseBuildingLocation () {
		List<Vector3> positionsAround = positionsAroundBuilding ();

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

	//Generate a large list of random building locations around current buildings
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

	//Choose a location to research the building.
	public void tryQueueResearch (string _researchName) {
		List<Building> possibleSpawnPoints = new List<Building> ();

		foreach (var r in AI.player.buildings) {
			if (r.building.hasAbility ("Research " + _researchName) && r.building.isBuilt) {
				possibleSpawnPoints.Add (r.building);
			}
		}

		//If there is a possible spawn point
		if (possibleSpawnPoints.Count > 0) {
			List<Building> spawnPoints = new List<Building> ();
			//Add research to the building with the shortest research queue
			int smallestQueue = 1000000;
			foreach (var r in possibleSpawnPoints) {
				if (r.researchQueue.Count < smallestQueue) {
					smallestQueue = r.researchQueue.Count;
				}
			}

			foreach (var r in possibleSpawnPoints) {
				if (r.researchQueue.Count == smallestQueue) {
					spawnPoints.Add (r);
				}
			}
			if (spawnPoints.Count == 0) {
				GameManager.print ("Something went wrong, I cut all my spawnPoints");
			} else {
				spawnPoints [Random.Range (0, spawnPoints.Count - 1)].useAbility ("Research " + _researchName);
				AI.creationQueue.RemoveAt (0);
			}
		}
	}
}
