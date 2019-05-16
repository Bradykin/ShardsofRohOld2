using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignScoutsToResources : Strategies {

	public AssignScoutsToResources (AIController _AI) {
		name = "AssignScoutsToResources";
		active = true;
		AI = _AI;
		interval = 0.0f;
	}

	public override void enact () {
		interval += Time.deltaTime;
		if (interval >= 2.0f) {
			interval = 0;

			Vector4 resourceScoutingPriorities = generateResourceScoutingNeeds ();
			List<UnitContainer> scouts = chooseScouts (resourceScoutingPriorities);
			chooseSquares (resourceScoutingPriorities, scouts);
		}
	}

	public Vector4 generateResourceScoutingNeeds () {
		Resource visibleSafeResources = new Resource (0, 0, 0, 0);
		Vector4 scoutingNeeds = new Vector4 (0, 0, 0, 0);

		foreach (var r in AI.scoutingGrid.grid) {
			foreach (var u in r) {
				if (u.isTileSafe == true) {
					visibleSafeResources.add (u.squareResources);
				}
			}
		}

		scoutingNeeds.x = Mathf.Max (Mathf.Floor (8 - visibleSafeResources.food / 2500), 0);
		scoutingNeeds.y = Mathf.Max (Mathf.Floor (8 - visibleSafeResources.wood / 2500), 0);
		scoutingNeeds.z = Mathf.Max (Mathf.Floor (8 - visibleSafeResources.gold / 2500), 0);
		scoutingNeeds.w = Mathf.Max (Mathf.Floor (8 - visibleSafeResources.metal / 2500), 0);
		GameManager.print (scoutingNeeds);

		//Create a formula using currently visible resources + average rate of gathering that resource in the past while + projected future needs of resource
		//to determine how much each resource needs to be explored for to find. Each value in the Vector4 will be represented by a 0-10 value, where 0 is
		//not needed at all, and 10 is must be found RIGHT now

		return scoutingNeeds;
	}

	public List<UnitContainer> chooseScouts (Vector4 scouting) {
		List<UnitContainer> scouts = new List<UnitContainer> ();
		//Assign scoutingUrgency between 0 and 10 - 10 is urgent, 0 is not needed
		int scoutingUrgency = (int) Mathf.Max (scouting.x, Mathf.Max (scouting.y, Mathf.Max (scouting.z, scouting.w)));
		//Use ??? process to decide the total # of scouts needed. For now, # of scouts = scoutingUrgency, probably a placeholder system
		int scoutsNeeded = scoutingUrgency;

		//Return empty list if no scouts are needed
		if (scoutsNeeded == 0) {
			return scouts;
		}

		//Go through the scouting values 0 to 10 inclusively, checking the unit list for members with that scouting value. 
		//Exit when you have enough scouts, reach the scoutingUrgency value, or reach the end of the list.
		for (int i = 0; i <= scoutingUrgency; i++) {
			foreach (var r in AI.player.units) {
				if (r.unit.scoutingValue == i) {
					scouts.Add (r);
					if (scoutsNeeded <= scouts.Count) {
						break;
					}
				}
			}

			if (scoutsNeeded <= scouts.Count) {
				break;
			}
		}
		//Reminder: scoutingValue is currently hardcoded to 0 for all units for testing purposes. Implement real system for this later.

		//Return the generated list of scouts
		return scouts;
	}

	public void chooseSquares (Vector4 scouting, List<UnitContainer> scouts) {
		if (scouts.Count > 0) {
			foreach (var r in AI.scoutingGrid.grid) {
				foreach (var u in r) {
					u.scoutingValue = generateScoutingValue (scouting, u);
				}
			}

			//For each scout, find the optimal square
			foreach (var w in scouts) {
				float highestValue = -1;
				int highestValueX = -1;
				int highestValueZ = -1;
				int curX = -2;
				int curZ = -2;

				foreach (var l in w.unitBehaviours) {
					if (l is ScoutSquare) {
						curX = (l as ScoutSquare).square.squareXArray;
						curZ = (l as ScoutSquare).square.squareZArray;
					}
				}

				foreach (var r in AI.scoutingGrid.grid) {
					foreach (var u in r) {
						float travelCost = u.scoutingValue / generateTravelCost (w, u);
						if (travelCost > highestValue) {
							//GameManager.print ("NewHighestSquare");
							highestValue = travelCost;
							highestValueX = u.squareXArray;
							highestValueZ = u.squareZArray;
						} else if (travelCost == highestValue) {
							if (u.squareXArray == curX && u.squareZArray == curZ) {
								highestValueX = u.squareXArray;
								highestValueZ = u.squareZArray;
							} else if (highestValueX == curX && highestValueZ == curZ) {

							} else {
								int i = Random.Range (0, 2);
								if (i == 0) {
									highestValueX = u.squareXArray;
									highestValueZ = u.squareZArray;
								}
							}
						}
					}
				}
					
				bool needNew = true;
				foreach (var l in w.unitBehaviours) {
					if (l is ScoutSquare) {
						if ((l as ScoutSquare).square.squareXArray == highestValueX && (l as ScoutSquare).square.squareZArray == highestValueZ) {
							needNew = false;
							break;
						} else {
							GameManager.print ("Moving scout off of " + (l as ScoutSquare).square.squareXArray + " - " + (l as ScoutSquare).square.squareZArray);
							break;
						}
					}
				}

				if (needNew == true) {
					GameManager.print ("Game time: " + GameManager.gameClock + ", Assign scout to " + highestValueX + " - " + highestValueZ);
					w.removeBehaviourByType ("Scout");
					w.removeBehaviourByType ("Idle");
					w.unitBehaviours.Add (new ScoutSquare (w));
					(w.unitBehaviours [w.unitBehaviours.Count - 1] as ScoutSquare).addSquare (AI.scoutingGrid.grid [highestValueX] [highestValueZ]);
				}
			}
		}
	}

	public float generateTravelCost (UnitContainer scout, ScoutingGridSquare square) {
		//Lazy implementation for now. Measure horizontal and vertical distance by square
		ScoutingGridSquare unitPosition = AI.scoutingGrid.getGridSpot (scout.unit.curLoc);

		return Mathf.Max (1.0f, 1.0f + (float) (Mathf.Abs (unitPosition.squareXArray - square.squareXArray) + Mathf.Abs (unitPosition.squareZArray - square.squareZArray)) / 2);
	}

	public float generateScoutingValue (Vector4 scouting, ScoutingGridSquare square) {
		if (square.explorationRemaining () == 0) {
			return 0.0f;
		} else {
			float value = 0;

			//Add estimation of value of scouting for each resource.
			//Methodology: value of scouting for resource = scouting need for that resource * (estimated amount of that resource in that square - amount known to be in that square)
			//Forseen flaw in this methodology - assumes an even distribution, not just in resources per square, but equal representation of each resource overall
			//Should be expanded in future to include distance to travel to square
			value += scouting.x * Mathf.Max (0, (square.predictedSquareResources.food - square.squareResources.food));
			value += scouting.y * Mathf.Max (0, (square.predictedSquareResources.wood - square.squareResources.wood));
			value += scouting.z * Mathf.Max (0, (square.predictedSquareResources.gold - square.squareResources.gold));
			value += scouting.w * Mathf.Max (0, (square.predictedSquareResources.metal - square.squareResources.metal));

			//GameManager.print ("Woodvalue = " + square.squareResources.wood);

			if (value > 0) {
				//GameManager.print ("Tile Value = " + value);
			} else {
				//GameManager.print ("NO VALUE: " + square.squareXArray + " - " + square.squareZArray);
			}

			return value * square.explorationRemaining ();
		}
	}
		
}

