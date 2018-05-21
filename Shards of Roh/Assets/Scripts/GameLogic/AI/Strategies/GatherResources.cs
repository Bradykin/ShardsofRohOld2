using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class GatherResources : Strategies {

	public GatherResources (AIController _AI) {
		name = "HitFlee";
		active = true;
		AI = _AI; 
	}

	public override void enact () {
		float neededFood = AI.resourcePriorities.getFood () - AI.player.resource.getFood ();
		float neededWood = AI.resourcePriorities.getWood () - AI.player.resource.getWood ();
		float neededGold = AI.resourcePriorities.getGold () - AI.player.resource.getGold ();

		if (neededFood < 0) {
			neededFood = -1000000;
		}
		if (neededWood < 0) {
			neededWood = -1000000;
		}
		if (neededGold < 0) {
			neededGold = -1000000;
		}

		float availableVillagers = 0;
		foreach (var r in AI.player.units) {
			if (r.unit.isVillager == true) {
				availableVillagers++;
			}
		}
			
		Vector3 resourceNeeded = new Vector3 (neededFood, neededWood, neededGold);
		Vector3 villagerAllocation = processVillagerAllocation (resourceNeeded, availableVillagers);

		/*PLAN:
		 * Check for villagers that are already gathering a resource, and if the quota needs them gathering that resource, give them that assignment.
		 * Of the remaining villagers and remaining quota, calculate an equation that minimizes overall movement. 
		 * Consider prioritizing villagers that are very close to a resource getting that job
		 * Consider prioritizing villagers that are very far from other resource types getting a job other then the one they are close to
		 */
		Vector3 currentAllocation = new Vector3 (0, 0, 0);

		List<UnitContainer> unitsToAdd = AI.player.units;
		List<bool> whichUnitsAreSet = new List<bool> ();

		for (int i = 0; i < AI.player.units.Count; i++) {
			if (AI.player.units [i].unit.isVillager == true) {
				whichUnitsAreSet.Add (false);
			} else {
				whichUnitsAreSet.Add (true);
			}
		}

		//GameManager.print (availableVillagers + " - " + villagerAllocation + " - " + unitsToAdd.Count);

		for (int i = 0; i < unitsToAdd.Count; i++) {
			if (unitsToAdd [i].unit.isVillager == true) {
				if (unitsToAdd [i].unit.isAttacking == true) {
					if (currentAllocation.x < villagerAllocation.x && unitsToAdd [i].unit.buildingTarget.building.resourceType == ResourceType.Food) {
						unitsToAdd [i].removeBehaviourByType ("Idle");
						unitsToAdd [i].unitBehaviours.Add (new IdleGather (unitsToAdd [i], ResourceType.Food));
						currentAllocation.x = currentAllocation.x + 1;
						whichUnitsAreSet [i] = true;
					} else if (currentAllocation.y < villagerAllocation.y && unitsToAdd [i].unit.buildingTarget.building.resourceType == ResourceType.Wood) {
						unitsToAdd [i].removeBehaviourByType ("Idle");
						unitsToAdd [i].unitBehaviours.Add (new IdleGather (unitsToAdd [i], ResourceType.Wood));
						currentAllocation.y = currentAllocation.y + 1;
						whichUnitsAreSet [i] = true;
					} else if (currentAllocation.z < villagerAllocation.z && unitsToAdd [i].unit.buildingTarget.building.resourceType == ResourceType.Gold) {
						unitsToAdd [i].removeBehaviourByType ("Idle");
						unitsToAdd [i].unitBehaviours.Add (new IdleGather (unitsToAdd [i], ResourceType.Gold));
						currentAllocation.z = currentAllocation.z + 1;
						whichUnitsAreSet [i] = true;
					}
				}
			}
		}

		/*for (int i = 0; i < unitsToAdd.Count; i++) {

			if (unitsToAdd [i].unit.isVillager == true && whichUnitsAreSet [i] == false) {
				if (currentAllocation.x < villagerAllocation.x) {
					unitsToAdd [i].removeBehaviourByType ("Idle");
					//unitsToAdd [i].unit.setAttackTarget (unitsToAdd [i].unit.visibleObjects.closestResourceFood);
					unitsToAdd [i].unitBehaviours.Add (new IdleGather (unitsToAdd [i], ResourceType.Food));
					currentAllocation.x = currentAllocation.x + 1;
					whichUnitsAreSet [i] = true;
				} else if (currentAllocation.y < villagerAllocation.y) {
					unitsToAdd [i].removeBehaviourByType ("Idle");
					//unitsToAdd [i].unit.setAttackTarget (unitsToAdd [i].unit.visibleObjects.closestResourceWood);
					unitsToAdd [i].unitBehaviours.Add (new IdleGather (unitsToAdd [i], ResourceType.Wood));
					currentAllocation.y = currentAllocation.y + 1;
					whichUnitsAreSet [i] = true;
				} else if (currentAllocation.z < villagerAllocation.z) {
					unitsToAdd [i].removeBehaviourByType ("Idle");
					//unitsToAdd [i].unit.setAttackTarget (unitsToAdd [i].unit.visibleObjects.closestResourceGold);
					unitsToAdd [i].unitBehaviours.Add (new IdleGather (unitsToAdd [i], ResourceType.Gold));
					currentAllocation.z = currentAllocation.z + 1;
					whichUnitsAreSet [i] = true;
				}
			}
		}*/
			
		while (currentAllocation.x + currentAllocation.y + currentAllocation.z < villagerAllocation.x + villagerAllocation.y + villagerAllocation.z) {
			float distanceFromClosestToFarthestSqr = 0;
			ResourceType resourceToGather = ResourceType.None;
			int index = 0;

			for (int i = 0; i < unitsToAdd.Count; i++) {
				if (whichUnitsAreSet [i] == false) {
					float maxDistanceToNeededResourceSqr = 0;
					float minDistanceToNeededResourceSqr = 1000000;
					if (currentAllocation.x < villagerAllocation.x) {
						maxDistanceToNeededResourceSqr = Mathf.Max (maxDistanceToNeededResourceSqr, unitsToAdd [i].unit.visibleObjects.distanceToClosestResourceFoodSqr);
						minDistanceToNeededResourceSqr = Mathf.Min (minDistanceToNeededResourceSqr, unitsToAdd [i].unit.visibleObjects.distanceToClosestResourceFoodSqr);
					} else if (currentAllocation.y < villagerAllocation.y) {
						maxDistanceToNeededResourceSqr = Mathf.Max (maxDistanceToNeededResourceSqr, unitsToAdd [i].unit.visibleObjects.distanceToClosestResourceWoodSqr);
						minDistanceToNeededResourceSqr = Mathf.Min (minDistanceToNeededResourceSqr, unitsToAdd [i].unit.visibleObjects.distanceToClosestResourceWoodSqr);
					} else if (currentAllocation.z < villagerAllocation.z) {
						maxDistanceToNeededResourceSqr = Mathf.Max (maxDistanceToNeededResourceSqr, unitsToAdd [i].unit.visibleObjects.distanceToClosestResourceGoldSqr);
						minDistanceToNeededResourceSqr = Mathf.Min (minDistanceToNeededResourceSqr, unitsToAdd [i].unit.visibleObjects.distanceToClosestResourceGoldSqr);
					}

					if (maxDistanceToNeededResourceSqr - minDistanceToNeededResourceSqr >= distanceFromClosestToFarthestSqr) {
						distanceFromClosestToFarthestSqr = maxDistanceToNeededResourceSqr - minDistanceToNeededResourceSqr;
						index = i;
						if (minDistanceToNeededResourceSqr == unitsToAdd [i].unit.visibleObjects.distanceToClosestResourceFoodSqr) {
							resourceToGather = ResourceType.Food;
						} else if (minDistanceToNeededResourceSqr == unitsToAdd [i].unit.visibleObjects.distanceToClosestResourceWoodSqr) {
							resourceToGather = ResourceType.Wood;
						} else if (minDistanceToNeededResourceSqr == unitsToAdd [i].unit.visibleObjects.distanceToClosestResourceGoldSqr) {
							resourceToGather = ResourceType.Gold;
						}
					}
				}
			}
				
			if (resourceToGather == ResourceType.Food) {
				unitsToAdd [index].removeBehaviourByType ("Idle");
				unitsToAdd [index].unit.setAttackTarget (unitsToAdd [index].unit.visibleObjects.closestResourceFood);
				unitsToAdd [index].unitBehaviours.Add (new IdleGather (unitsToAdd [index], ResourceType.Food));
				currentAllocation.x = currentAllocation.x + 1;
				GameManager.print ("1");
				whichUnitsAreSet [index] = true;
			} else if (resourceToGather == ResourceType.Wood) {
				unitsToAdd [index].removeBehaviourByType ("Idle");
				unitsToAdd [index].unit.setAttackTarget (unitsToAdd [index].unit.visibleObjects.closestResourceWood);
				unitsToAdd [index].unitBehaviours.Add (new IdleGather (unitsToAdd [index], ResourceType.Wood));
				currentAllocation.y = currentAllocation.y + 1;
				whichUnitsAreSet [index] = true;
				GameManager.print ("2");
			} else if (resourceToGather == ResourceType.Gold) {
				unitsToAdd [index].removeBehaviourByType ("Idle");
				unitsToAdd [index].unit.setAttackTarget (unitsToAdd [index].unit.visibleObjects.closestResourceGold);
				unitsToAdd [index].unitBehaviours.Add (new IdleGather (unitsToAdd [index], ResourceType.Gold));
				currentAllocation.z = currentAllocation.z + 1;
				whichUnitsAreSet [index] = true;
				GameManager.print ("3");
			}
		}
	}

	private Vector3 processVillagerAllocation (Vector3 resourceNeeded, float availableVillagers) {
		Vector3 allocatedVillagers = new Vector3 (0, 0, 0);
		float totalVillagers = availableVillagers;
		float totalResourcesWanted = resourceNeeded.x + resourceNeeded.y + resourceNeeded.z;

		while (availableVillagers > 0) {
			float needMost = Mathf.Max (resourceNeeded.x, Mathf.Max (resourceNeeded.y, resourceNeeded.z));

			if (needMost == resourceNeeded.x) {
				allocatedVillagers.x += 1;
				resourceNeeded.x -= (totalResourcesWanted / totalVillagers);
			} else if (needMost == resourceNeeded.y) {
				allocatedVillagers.y += 1;
				resourceNeeded.y -= (totalResourcesWanted / totalVillagers);
			} else {
				allocatedVillagers.z += 1;
				resourceNeeded.z -= (totalResourcesWanted / totalVillagers);
			}

			availableVillagers--;
		}

		return allocatedVillagers;
	}
}
