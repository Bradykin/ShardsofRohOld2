using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class GatherResources : Strategies {

	public GatherResources (AIController _AI) {
		name = "GatherResources";
		active = true;
		AI = _AI; 
	}

	public override void enact () {
		float neededFood = AI.resourcePriorities.getFood () - AI.player.resource.getFood ();
		float neededWood = AI.resourcePriorities.getWood () - AI.player.resource.getWood ();
		float neededGold = AI.resourcePriorities.getGold () - AI.player.resource.getGold ();
		float neededBuild = AI.buildPriorities;

		if (neededFood < 0) {
			neededFood = -1000000;
		}
		if (neededWood < 0) {
			neededWood = -1000000;
		}
		if (neededGold < 0) {
			neededGold = -1000000;
		}
		if (neededBuild < 0) {
			neededBuild = -1000000;
		}

		float availableVillagers = 0;
		foreach (var r in AI.player.units) {
			if (r.unit.isVillager == true) {
				availableVillagers++;
			}
		}

		/*PLAN:
		 * Check for villagers that are already gathering a resource, and if the quota needs them gathering that resource, give them that assignment.
		 * Of the remaining villagers and remaining quota, calculate an equation that minimizes overall movement. 
		 * Consider prioritizing villagers that are very close to a resource getting that job
		 * Consider prioritizing villagers that are very far from other resource types getting a job other then the one they are close to
		 */

		/*Vector3 resourceNeeded = new Vector3 (neededFood, neededWood, neededGold);
		Vector3 villagerAllocation = processVillagerAllocation (resourceNeeded, availableVillagers);

		Vector3 currentAllocation = new Vector3 (0, 0, 0);

		List<UnitContainer> unitsToAdd = AI.player.units;
		List<bool> whichUnitsAreSet = new List<bool> ();

		for (int i = 0; i < AI.player.units.Count; i++) {
			if (AI.player.units [i].unit.isVillager == true) {
				whichUnitsAreSet.Add (false);
			} else {
				whichUnitsAreSet.Add (true);
			}
		}*/

		Vector3 resourceNeeded = new Vector3 (neededFood, neededWood, neededGold);
		Vector4 villagerAllocation = processVillagerAllocation (resourceNeeded, neededBuild, availableVillagers);

		Vector4 currentAllocation = new Vector4 (0, 0, 0, 0);

		List<UnitContainer> unitsToAdd = AI.player.units;
		List<bool> whichUnitsAreSet = new List<bool> ();

		for (int i = 0; i < AI.player.units.Count; i++) {
			if (AI.player.units [i].unit.isVillager == true) {
				whichUnitsAreSet.Add (false);
			} else {
				whichUnitsAreSet.Add (true);
			}
		}

		//Frame delay is causing an error throw here, temporary measure to catch it
		try {
			for (int i = 0; i < unitsToAdd.Count; i++) {
				if (unitsToAdd [i].unit.isVillager == true) {
					if (unitsToAdd [i].unit.isAttacking == true) {
						if (currentAllocation.x < villagerAllocation.x && unitsToAdd [i].unit.buildingTarget.building.isResource == true && unitsToAdd [i].unit.buildingTarget.building.resourceType == ResourceType.Food) {
							unitsToAdd [i].unitBehaviours.Add (new IdleGather (unitsToAdd [i], ResourceType.Food));
							currentAllocation.x = currentAllocation.x + 1;
							whichUnitsAreSet [i] = true;
						} else if (currentAllocation.y < villagerAllocation.y && unitsToAdd [i].unit.buildingTarget.building.isResource == true && unitsToAdd [i].unit.buildingTarget.building.resourceType == ResourceType.Wood) {
							unitsToAdd [i].unitBehaviours.Add (new IdleGather (unitsToAdd [i], ResourceType.Wood));
							currentAllocation.y = currentAllocation.y + 1;
							whichUnitsAreSet [i] = true;
						} else if (currentAllocation.z < villagerAllocation.z && 
							unitsToAdd [i].unit.buildingTarget.building.isResource == true && 
							unitsToAdd [i].unit.buildingTarget.building.resourceType == ResourceType.Gold) {
							unitsToAdd [i].unitBehaviours.Add (new IdleGather (unitsToAdd [i], ResourceType.Gold));
							currentAllocation.z = currentAllocation.z + 1;
							whichUnitsAreSet [i] = true;
						} else if (currentAllocation.w < villagerAllocation.w && unitsToAdd [i].unit.buildingTarget.building.owner == unitsToAdd [i].unit.owner && unitsToAdd [i].unit.buildingTarget.building.isBuilt == false) {
							unitsToAdd [i].unitBehaviours.Add (new IdleBuild (unitsToAdd [i]));
							currentAllocation.w = currentAllocation.w + 1;
							whichUnitsAreSet [i] = true;
						}
					}
				}
			}
		} catch {

		}
			
		while (currentAllocation.x + currentAllocation.y + currentAllocation.z + currentAllocation.w < villagerAllocation.x + villagerAllocation.y + villagerAllocation.z + villagerAllocation.w) {
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
					} else if (currentAllocation.w < villagerAllocation.w) {
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
						} else if (minDistanceToNeededResourceSqr == unitsToAdd [i].unit.visibleObjects.distanceToClosestPlayerUnbuiltSqr) {
							resourceToGather = ResourceType.Build;
						}
					}
				}
			}
				
			if (resourceToGather == ResourceType.Food) {
				unitsToAdd [index].unit.setAttackTarget (unitsToAdd [index].unit.visibleObjects.closestResourceFood);
				unitsToAdd [index].unitBehaviours.Add (new IdleGather (unitsToAdd [index], ResourceType.Food));
				currentAllocation.x = currentAllocation.x + 1;
				whichUnitsAreSet [index] = true;
			} else if (resourceToGather == ResourceType.Wood) {
				unitsToAdd [index].unit.setAttackTarget (unitsToAdd [index].unit.visibleObjects.closestResourceWood);
				unitsToAdd [index].unitBehaviours.Add (new IdleGather (unitsToAdd [index], ResourceType.Wood));
				currentAllocation.y = currentAllocation.y + 1;
				whichUnitsAreSet [index] = true;
			} else if (resourceToGather == ResourceType.Gold) {
				unitsToAdd [index].unit.setAttackTarget (unitsToAdd [index].unit.visibleObjects.closestResourceGold);
				unitsToAdd [index].unitBehaviours.Add (new IdleGather (unitsToAdd [index], ResourceType.Gold));
				currentAllocation.z = currentAllocation.z + 1;
				whichUnitsAreSet [index] = true;
			} else if (resourceToGather == ResourceType.Build) {
				unitsToAdd [index].unit.setAttackTarget (unitsToAdd [index].unit.visibleObjects.closestPlayerUnbuilt);
				unitsToAdd [index].unitBehaviours.Add (new IdleBuild (unitsToAdd [index]));
				currentAllocation.w = currentAllocation.w + 1;
				whichUnitsAreSet [index] = true;
			}
		}
	}
		
	private Vector4 processVillagerAllocation (Vector3 _resourceNeeded, float _neededBuild, float _availableVillagers) {
		Vector4 allocatedVillagers = new Vector4 (0, 0, 0, 0);
		float totalVillagers = _availableVillagers;
		float totalResourcesWanted = _resourceNeeded.x + _resourceNeeded.y + _resourceNeeded.z + _neededBuild;

		while (_availableVillagers > 0) {
			float needMost = Mathf.Max (_resourceNeeded.x, Mathf.Max (_resourceNeeded.y, Mathf.Max (_resourceNeeded.z, _neededBuild)));

			if (needMost == _resourceNeeded.x) {
				allocatedVillagers.x += 1;
				_resourceNeeded.x -= (totalResourcesWanted / totalVillagers);
			} else if (needMost == _resourceNeeded.y) {
				allocatedVillagers.y += 1;
				_resourceNeeded.y -= (totalResourcesWanted / totalVillagers);
			} else if (needMost == _resourceNeeded.z) {
				allocatedVillagers.z += 1;
				_resourceNeeded.z -= (totalResourcesWanted / totalVillagers);
			} else {
				allocatedVillagers.w += 1;
				_neededBuild -= (totalResourcesWanted / totalVillagers);
			}

			_availableVillagers--;
		}

		return allocatedVillagers;
	}
}
