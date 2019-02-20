using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class GatherResources : Strategies {

	VillagerAllocation alloc = new VillagerAllocation ();
	VillagerAllocation currentAlloc = new VillagerAllocation ();

	public GatherResources (AIController _AI) {
		name = "GatherResources";
		active = true;
		AI = _AI; 
	}

	public override void enact () {
		float neededFood = AI.resourcePriorities.food - AI.player.resource.food;
		float neededWood = AI.resourcePriorities.wood - AI.player.resource.wood;
		float neededGold = AI.resourcePriorities.gold - AI.player.resource.gold;
		float neededMetal = AI.resourcePriorities.metal - AI.player.resource.metal;
		float neededBuild = AI.buildPriorities;

		if (neededFood < 0) {
			neededFood = 0;
		}
		if (neededWood < 0) {
			neededWood = 0;
		}
		if (neededGold < 0) {
			neededGold = 0;
		}
		if (neededMetal < 0) {
			neededMetal = 0;
		}
		if (neededBuild < 0) {
			neededBuild = 0;
		}

		float availableVillagers = 0;
		foreach (var r in AI.player.units) {
			if (r.unit.unitType == UnitType.Villager) {
				availableVillagers++;
			}
		}

		/*PLAN:
		 * Calculate the intended distribution of resources using processVillagerAllocation, and clear the current Allocation.
		 * Check for villagers that are already gathering a resource, and if the quota needs them gathering that resource, give them that assignment.
		 * Of the remaining villagers and remaining quota, calculate an equation that minimizes overall movement. 
		 * Consider prioritizing villagers that are very close to a resource getting that job
		 * Consider prioritizing villagers that are very far from other resource types getting a job other then the one they are close to
		 */

		Vector4 resourceNeeded = new Vector4 (neededFood, neededWood, neededGold, neededMetal);
		processVillagerAllocation (resourceNeeded, neededBuild, availableVillagers);
		currentAlloc.clearAllocation ();

		List<UnitContainer> unitsToAdd = AI.player.units;
		List<bool> whichUnitsAreSet = new List<bool> ();

		for (int i = 0; i < AI.player.units.Count; i++) {
			if (AI.player.units [i].unit.unitType == UnitType.Villager) {
				whichUnitsAreSet.Add (false);
			} else {
				whichUnitsAreSet.Add (true);
			}
		}

		//Frame delay is causing an error throw here, temporary measure to catch it
		try {
			for (int i = 0; i < unitsToAdd.Count; i++) {
				if (unitsToAdd [i].unit.unitType == UnitType.Villager) {
					if (unitsToAdd [i].unit.isAttacking == true) {
						if (currentAlloc.food < alloc.food && unitsToAdd [i].unit.buildingTarget.building.isResource == true && unitsToAdd [i].unit.buildingTarget.building.resourceType == ResourceType.Food) {
							unitsToAdd [i].unitBehaviours.Add (new IdleGather (unitsToAdd [i], ResourceType.Food));
							currentAlloc.food += 1;
							whichUnitsAreSet [i] = true;
						} else if (currentAlloc.wood < alloc.wood && unitsToAdd [i].unit.buildingTarget.building.isResource == true && unitsToAdd [i].unit.buildingTarget.building.resourceType == ResourceType.Wood) {
							unitsToAdd [i].unitBehaviours.Add (new IdleGather (unitsToAdd [i], ResourceType.Wood));
							currentAlloc.wood += 1;
							whichUnitsAreSet [i] = true;
						} else if (currentAlloc.gold < alloc.gold && unitsToAdd [i].unit.buildingTarget.building.isResource == true && unitsToAdd [i].unit.buildingTarget.building.resourceType == ResourceType.Gold) {
							unitsToAdd [i].unitBehaviours.Add (new IdleGather (unitsToAdd [i], ResourceType.Gold));
							currentAlloc.gold += 1;
							whichUnitsAreSet [i] = true;
						} else if (currentAlloc.metal < alloc.metal && unitsToAdd [i].unit.buildingTarget.building.isResource == true && unitsToAdd [i].unit.buildingTarget.building.resourceType == ResourceType.Metal) {
							unitsToAdd [i].unitBehaviours.Add (new IdleGather (unitsToAdd [i], ResourceType.Metal));
							currentAlloc.metal += 1;
							whichUnitsAreSet [i] = true;
						} else if (currentAlloc.build < alloc.build && unitsToAdd [i].unit.buildingTarget.building.owner == unitsToAdd [i].unit.owner && unitsToAdd [i].unit.buildingTarget.building.isBuilt == false) {
							unitsToAdd [i].unitBehaviours.Add (new IdleBuild (unitsToAdd [i]));
							currentAlloc.build += 1;
							whichUnitsAreSet [i] = true;
						}
					}
				}
			}
		} catch {

		}
			
		while (currentAlloc.food + currentAlloc.wood + currentAlloc.gold + currentAlloc.metal + currentAlloc.build < alloc.food + alloc.wood + alloc.gold + alloc.metal + alloc.build) {
			float distanceFromClosestToFarthestSqr = 0;
			ResourceType resourceToGather = ResourceType.None;
			int index = 0;

			for (int i = 0; i < unitsToAdd.Count; i++) {
				if (whichUnitsAreSet [i] == false) {
					float maxDistanceToNeededResourceSqr = 0;
					float minDistanceToNeededResourceSqr = 1000000;
					if (currentAlloc.food < alloc.food) {
						maxDistanceToNeededResourceSqr = Mathf.Max (maxDistanceToNeededResourceSqr, unitsToAdd [i].unit.visibleObjects.distanceToClosestResourceFoodSqr);
						minDistanceToNeededResourceSqr = Mathf.Min (minDistanceToNeededResourceSqr, unitsToAdd [i].unit.visibleObjects.distanceToClosestResourceFoodSqr);
					} else if (currentAlloc.wood < alloc.wood) {
						maxDistanceToNeededResourceSqr = Mathf.Max (maxDistanceToNeededResourceSqr, unitsToAdd [i].unit.visibleObjects.distanceToClosestResourceWoodSqr);
						minDistanceToNeededResourceSqr = Mathf.Min (minDistanceToNeededResourceSqr, unitsToAdd [i].unit.visibleObjects.distanceToClosestResourceWoodSqr);
					} else if (currentAlloc.gold < alloc.gold) {
						maxDistanceToNeededResourceSqr = Mathf.Max (maxDistanceToNeededResourceSqr, unitsToAdd [i].unit.visibleObjects.distanceToClosestResourceGoldSqr);
						minDistanceToNeededResourceSqr = Mathf.Min (minDistanceToNeededResourceSqr, unitsToAdd [i].unit.visibleObjects.distanceToClosestResourceGoldSqr);
					} else if (currentAlloc.metal < alloc.metal) {
						maxDistanceToNeededResourceSqr = Mathf.Max (maxDistanceToNeededResourceSqr, unitsToAdd [i].unit.visibleObjects.distanceToClosestResourceMetalSqr);
						minDistanceToNeededResourceSqr = Mathf.Min (minDistanceToNeededResourceSqr, unitsToAdd [i].unit.visibleObjects.distanceToClosestResourceMetalSqr);
					} else if (currentAlloc.build < alloc.build) {
						maxDistanceToNeededResourceSqr = Mathf.Max (maxDistanceToNeededResourceSqr, unitsToAdd [i].unit.visibleObjects.distanceToClosestPlayerUnbuiltSqr);
						minDistanceToNeededResourceSqr = Mathf.Min (minDistanceToNeededResourceSqr, unitsToAdd [i].unit.visibleObjects.distanceToClosestPlayerUnbuiltSqr);
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
						} else if (minDistanceToNeededResourceSqr == unitsToAdd [i].unit.visibleObjects.distanceToClosestResourceMetalSqr) {
							resourceToGather = ResourceType.Metal;
						} else if (minDistanceToNeededResourceSqr == unitsToAdd [i].unit.visibleObjects.distanceToClosestPlayerUnbuiltSqr) {
							resourceToGather = ResourceType.Build;
						}
					}
				}
			}
				
			if (resourceToGather == ResourceType.Food) {
				unitsToAdd [index].unit.setAttackTarget (unitsToAdd [index].unit.visibleObjects.closestResourceFood);
				unitsToAdd [index].unitBehaviours.Add (new IdleGather (unitsToAdd [index], ResourceType.Food));
				currentAlloc.food += 1;
				whichUnitsAreSet [index] = true;
			} else if (resourceToGather == ResourceType.Wood) {
				unitsToAdd [index].unit.setAttackTarget (unitsToAdd [index].unit.visibleObjects.closestResourceWood);
				unitsToAdd [index].unitBehaviours.Add (new IdleGather (unitsToAdd [index], ResourceType.Wood));
				currentAlloc.wood += 1;
				whichUnitsAreSet [index] = true;
			} else if (resourceToGather == ResourceType.Gold) {
				unitsToAdd [index].unit.setAttackTarget (unitsToAdd [index].unit.visibleObjects.closestResourceGold);
				unitsToAdd [index].unitBehaviours.Add (new IdleGather (unitsToAdd [index], ResourceType.Gold));
				currentAlloc.gold += 1;
				whichUnitsAreSet [index] = true;
			} else if (resourceToGather == ResourceType.Metal) {
				unitsToAdd [index].unit.setAttackTarget (unitsToAdd [index].unit.visibleObjects.closestResourceMetal);
				unitsToAdd [index].unitBehaviours.Add (new IdleGather (unitsToAdd [index], ResourceType.Metal));
				currentAlloc.metal += 1;
				whichUnitsAreSet [index] = true;
			} else if (resourceToGather == ResourceType.Build) {
				unitsToAdd [index].unit.setAttackTarget (unitsToAdd [index].unit.visibleObjects.closestPlayerUnbuilt);
				unitsToAdd [index].unitBehaviours.Add (new IdleBuild (unitsToAdd [index]));
				currentAlloc.build += 1;
				whichUnitsAreSet [index] = true;
			}
		}
	}
		
	private void processVillagerAllocation (Vector4 _resourceNeeded, float _neededBuild, float _availableVillagers) {
		alloc.clearAllocation ();
		float totalVillagers = _availableVillagers;
		float totalResourcesWanted = _resourceNeeded.x + _resourceNeeded.y + _resourceNeeded.z + _resourceNeeded.w + _neededBuild;


		while (_availableVillagers > 0) {
			float needMost = Mathf.Max (_resourceNeeded.x, Mathf.Max (_resourceNeeded.y, Mathf.Max (_resourceNeeded.z, Mathf.Max (_resourceNeeded.w, _neededBuild))));

			if (needMost == _resourceNeeded.x) {
				alloc.food += 1;
				_resourceNeeded.x -= (totalResourcesWanted / totalVillagers);
			} else if (needMost == _resourceNeeded.y) {
				alloc.wood += 1;
				_resourceNeeded.y -= (totalResourcesWanted / totalVillagers);
			} else if (needMost == _resourceNeeded.z) {
				alloc.gold += 1;
				_resourceNeeded.z -= (totalResourcesWanted / totalVillagers);
			} else if (needMost == _resourceNeeded.w) {
				alloc.metal += 1;
				_resourceNeeded.w -= (totalResourcesWanted / totalVillagers);
			} else if (needMost == _neededBuild) {
				alloc.build += 1;
				_neededBuild -= (totalResourcesWanted / totalVillagers);
			} else {
				GameManager.print ("NeedMost not found - processVillagerAllocation");
			}

			_availableVillagers--;
		}
	}
}
