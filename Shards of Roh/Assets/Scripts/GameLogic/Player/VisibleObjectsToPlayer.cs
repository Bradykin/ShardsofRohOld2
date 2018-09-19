using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleObjectsToPlayer {

	private float timer { get; set; }
	private Player player { get; set; }
	public List<UnitContainer> visiblePlayerUnits { get; protected set; }
	public List<BuildingContainer> visiblePlayerBuildings { get; protected set; }
	public List<UnitContainer> visibleResourceUnits { get; protected set; }
	public List<BuildingContainer> visibleResourceBuildings { get; protected set; }
	public List<UnitContainer> visibleEnemyUnits { get; protected set; }
	public List<BuildingContainer> visibleEnemyBuildings { get; protected set; }

	public VisibleObjectsToPlayer (Player _player) {
		player = _player;
		timer = 0.0f;
		visiblePlayerUnits = new List<UnitContainer> ();
		visiblePlayerBuildings = new List<BuildingContainer> ();
		visibleResourceUnits = new List<UnitContainer> ();
		visibleResourceBuildings = new List<BuildingContainer> ();
		visibleEnemyUnits = new List<UnitContainer> ();
		visibleEnemyBuildings = new List<BuildingContainer> ();
	}

	public void updateVisible () {

		timer += Time.deltaTime;
		if (timer > 0.25f) {
			visiblePlayerUnits.Clear ();
			visiblePlayerBuildings.Clear ();
			visibleResourceUnits.Clear ();
			visibleResourceBuildings.Clear ();
			visibleEnemyUnits.Clear ();
			visibleEnemyBuildings.Clear ();
			timer = 0;

			for (int i = 0; i < player.units.Count; i++) {
				player.units [i].unit.visibleObjects.resetVisibleObjects ();
			}

			foreach (var r in GameManager.playersInGame) {

				for (int i = 0; i < player.units.Count; i++) {
					float sightRadius = player.units [i].unit.sightRadius;
					float sightRadiusSqr = sightRadius * sightRadius;
					for (int k = 0; k < r.units.Count; k++) {

						float distance = Vector3.SqrMagnitude (r.units [k].unit.curLoc - player.units [i].unit.curLoc);
						if (r.name == "Nature") {
							if (visibleResourceUnits.Contains (r.units [k]) == false && distance <= sightRadiusSqr) {
								visibleResourceUnits.Add (r.units [k]);
							}
						} else if (GameManager.isEnemies (player, r) == true) {
							if (visibleEnemyUnits.Contains (r.units [k]) == false && distance <= sightRadiusSqr) {
								visibleEnemyUnits.Add (r.units [k]);
							}
						} else {
							if (visiblePlayerUnits.Contains (r.units [k]) == false) {
								visiblePlayerUnits.Add (r.units [k]);
							}
						}
					}

					for (int k = 0; k < r.buildings.Count; k++) {
						float distance = Vector3.SqrMagnitude (r.buildings [k].building.curLoc - player.units [i].unit.curLoc);
						if (r.name == "Nature") {
							if (visibleResourceBuildings.Contains (r.buildings [k]) == false && distance <= sightRadiusSqr) {
								visibleResourceBuildings.Add (r.buildings [k]);
							}
						} else if (GameManager.isEnemies (player, r) == true) {
							if (visibleEnemyBuildings.Contains (r.buildings [k]) == false && distance <= sightRadiusSqr) {
								visibleEnemyBuildings.Add (r.buildings [k]);
							}
						} else {
							if (visiblePlayerBuildings.Contains (r.buildings [k]) == false) {
								visiblePlayerBuildings.Add (r.buildings [k]);
							}
						}
					}
				}
			}

			foreach (var u in player.units) {
				float sightRadius = u.unit.sightRadius;
				float sightRadiusSqr = sightRadius * sightRadius;

				foreach (var r in visiblePlayerUnits) {
					if (u != r) {
						bool inSight;
						float distance = Vector3.SqrMagnitude (u.unit.curLoc - r.unit.curLoc);
						if (distance <= sightRadiusSqr) {
							inSight = true;
						} else {
							inSight = false;
						}
						u.unit.visibleObjects.addPlayerUnit (r, distance, inSight);
					}
				}

				foreach (var r in visiblePlayerBuildings) {
					if (u != r) {
						bool inSight;
						float distance = Vector3.SqrMagnitude (u.unit.curLoc - r.building.curLoc);
						if (distance <= sightRadiusSqr) {
							inSight = true;
						} else {
							inSight = false;
						}
						u.unit.visibleObjects.addPlayerBuilding (r, distance, inSight);
					}
				}

				foreach (var r in visibleResourceUnits) {
					if (u != r) {
						bool inSight;
						float distance = Vector3.SqrMagnitude (u.unit.curLoc - r.unit.curLoc);
						if (distance <= sightRadiusSqr) {
							inSight = true;
						} else {
							inSight = false;
						}
						u.unit.visibleObjects.addResourceUnit (r, distance, inSight);
					}
				}

				foreach (var r in visibleResourceBuildings) {
					if (u != r) {
						bool inSight;
						float distance = Vector3.SqrMagnitude (u.unit.curLoc - r.building.curLoc);
						if (distance <= sightRadiusSqr) {
							inSight = true;
						} else {
							inSight = false;
						}
						u.unit.visibleObjects.addResourceBuilding (r, distance, inSight);
					}
				}

				foreach (var r in visibleEnemyUnits) {
					if (u != r) {
						bool inSight;
						float distance = Vector3.SqrMagnitude (u.unit.curLoc - r.unit.curLoc);
						if (distance <= sightRadiusSqr) {
							inSight = true;
						} else {
							inSight = false;
						}
						u.unit.visibleObjects.addEnemyUnit (r, distance, inSight);
					}
				}

				foreach (var r in visibleEnemyBuildings) {
					if (u != r) {
						bool inSight;
						float distance = Vector3.SqrMagnitude (u.unit.curLoc - r.building.curLoc);
						if (distance <= sightRadiusSqr) {
							inSight = true;
						} else {
							inSight = false;
						}
						u.unit.visibleObjects.addEnemyBuilding (r, distance, inSight);
					}
				}
			}

			for (int i = 0; i < player.units.Count; i++) {
				player.units [i].unit.visibleObjects.doCalculations ();
			}
		}
	}
}
