using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

class BuildingConstructionStrategizer {
	Player player;


	public BuildingConstructionStrategizer (AIController _AI) {
		player = _AI.player;
	}

	public void calculateBuildingPurchases (List <Purchaseable> purchases) {
		//purchases.Sort ((x, y) => x.name.CompareTo (y.name));
		List<Purchaseable> buildingsToAdd = new List<Purchaseable> ();
		List<Purchaseable> objectsInFront = new List<Purchaseable> ();

		//Add prerequisite buildings
		string prevName = "";
		foreach (var r in purchases) {
			if (r.name != prevName) {
				bool canBuild = false;
				foreach (var b in player.buildings) {
					foreach (var a in b.building.abilities) {
						if (a.name == "Spawn " + r.name || a.name == "Research " + r.name) {
							canBuild = true;
							break;
						}
					}

					if (canBuild == true) {
						break;
					}
				}

				if (canBuild == false) {
					//Add to list
					bool foundBuilding = false;
					foreach (var b in player.playerRace.buildingTypes) {
						foreach (var a in b.abilities) {
							if (a.name == "Spawn " + r.name || a.name == "Research " + r.name) {
								foundBuilding = true;
								bool addBuilding = true;
								foreach (var u in buildingsToAdd) {
									if (u.name == b.name) {
										addBuilding = false;
										break;
									}
								}

								if (addBuilding == true) {
									buildingsToAdd.Add (ObjectFactory.createBuildingByName (b.name, player));
									objectsInFront.Add (r);
								}
								break;
							}
						}

						if (foundBuilding == true) {
							break;
						}
					}

					if (foundBuilding == false) {
						GameManager.print ("No building option for making " + r.name);
					}
				}
			}
		}

		//Add population building
		if (((float) player.population * 1.5f) >= player.maxPopulation) {
			bool hasHouseInQueue = false;
			foreach (var r in player.buildings) {
				if (r.building.isBuilt == false && r.building.name == "House") {
					hasHouseInQueue = true;
				}
			}

			if (hasHouseInQueue == false) {
				purchases.Insert (0, ObjectFactory.createBuildingByName ("House", player));
			}
		}

		bool adding = true;
		while (adding == true) {
			adding = false;
			if (objectsInFront.Count > 0) {
				for (int i = 0; i < purchases.Count; i++) {
					if (purchases [i].name == objectsInFront [0].name) {
						purchases.Insert (i, buildingsToAdd [0]);
						buildingsToAdd.RemoveAt (0);
						objectsInFront.RemoveAt (0);
						adding = true;
						break;
					}
				}
			}
		}

		purchases.AddRange (buildingsToAdd);
	}
}

