using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class EconomicConstructionStrategizer {

	AIController ai;

	public EconomicConstructionStrategizer (AIController _AI) {
		ai = _AI;
	}

	public void calculateOptimalPurchaseValues () {
		//Step 1 - ensure that the variables are cleared
		Resource predictedNeededResources = pastResourcesGathered ();
		//GameManager.print (predictedNeededResources.toString ());

		float bestFoodValue = 0;
		float bestWoodValue = 0;
		float bestGoldValue = 0;
		float bestMetalValue = 0;

		foreach (var r in ai.player.playerRace.unitTypes) {
			r.AIEconomicFoodScore = 0;
			r.AIEconomicWoodScore = 0;
			r.AIEconomicGoldScore = 0;
			r.AIEconomicMetalScore = 0;
			r.AIEconomicTotalScore = 0;
		}

		foreach (var r in ai.player.playerRace.researchTypes) {
			r.AIEconomicFoodScore = 0;
			r.AIEconomicWoodScore = 0;
			r.AIEconomicGoldScore = 0;
			r.AIEconomicMetalScore = 0;
			r.AIEconomicTotalScore = 0;
		}

		//Step 2 - generate a starting value for each unit and research
		foreach (var r in ai.player.playerRace.unitTypes) {
			if (r.unitType == UnitType.Villager) {
				float bestUnitFood = 0;

				if (r.foodAnimalGatherRate * r.attackSpeed > bestUnitFood && ai.player.visibleObjects.canAccessFoodAnimal == true) {
					bestUnitFood = r.foodAnimalGatherRate * r.attackSpeed;
				}

				if (r.foodForageGatherRate * r.attackSpeed > bestUnitFood && ai.player.visibleObjects.canAccessFoodForage == true) {
					bestUnitFood = r.foodForageGatherRate * r.attackSpeed;
				}

				if (r.foodFarmGatherRate * r.attackSpeed > bestUnitFood && ai.player.visibleObjects.canAccessFoodFarm == true) {
					bestUnitFood = r.foodFarmGatherRate * r.attackSpeed;
				}
				r.AIEconomicFoodScore = bestUnitFood;

				if (r.AIEconomicFoodScore > bestFoodValue) {
					bestFoodValue = bestUnitFood;
				}

				if (ai.player.visibleObjects.canAccessWood == true) {
					r.AIEconomicWoodScore = r.woodGatherRate * r.attackSpeed;
					if (r.AIEconomicWoodScore > bestWoodValue) {
						bestWoodValue = r.AIEconomicWoodScore;
					}
				}

				if (ai.player.visibleObjects.canAccessGold == true) {
					r.AIEconomicGoldScore = r.goldGatherRate * r.attackSpeed;
					if (r.AIEconomicGoldScore > bestGoldValue) {
						bestGoldValue = r.AIEconomicGoldScore;
					}
				}

				if (ai.player.visibleObjects.canAccessMetal == true) {
					r.AIEconomicMetalScore = r.metalGatherRate * r.attackSpeed;
					if (r.AIEconomicMetalScore > bestMetalValue) {
						bestMetalValue = r.AIEconomicMetalScore;
					}
				}
			}
			//GameManager.print (r.name + " SCORES: " + r.AIEconomicFoodScore + " " + r.AIEconomicWoodScore + " " + r.AIEconomicGoldScore + " " + r.AIEconomicMetalScore);

			//r.AIEconomicFoodScore = r.AIEconomicFoodScore / bestFoodValue;
			//r.AIEconomicWoodScore = r.AIEconomicWoodScore / bestWoodValue;
			//r.AIEconomicGoldScore = r.AIEconomicGoldScore / bestGoldValue;
			//r.AIEconomicMetalScore = r.AIEconomicMetalScore / bestMetalValue;
			r.AIEconomicTotalScore = (r.AIEconomicFoodScore * predictedNeededResources.food) + (r.AIEconomicWoodScore * predictedNeededResources.wood) + (r.AIEconomicGoldScore * predictedNeededResources.gold) + (r.AIEconomicMetalScore * predictedNeededResources.metal);

			if (float.IsNaN (r.AIEconomicTotalScore)) {
				GameManager.print (r.name + " NaN ERROR");
				r.AIEconomicTotalScore = 0;
			}
		}

		foreach (var r in ai.player.playerRace.researchTypes) {
			r.AIEconomicTotalScore = 0;
			if (ai.player.hasResearch (r.name) == false) {
				bool isResearchRelevant = false;
				foreach (var e in r.effects) {
					if (e.researchEffectPurpose == ResearchPurpose.Economic || e.researchEffectPurpose == ResearchPurpose.CombatEconomic) {
						isResearchRelevant = true;
					}
				}

				if (isResearchRelevant == true) {
					simulateResearchEconomicEffect (r);

					if (r.AIEconomicFoodScore > bestFoodValue) {
						bestFoodValue = r.AIEconomicFoodScore;
					}

					if (r.AIEconomicWoodScore > bestWoodValue) {
						bestWoodValue = r.AIEconomicWoodScore;
					}

					if (r.AIEconomicGoldScore > bestGoldValue) {
						bestGoldValue = r.AIEconomicGoldScore;
					}

					if (r.AIEconomicMetalScore > bestMetalValue) {
						bestMetalValue = r.AIEconomicMetalScore;
					}
				}
			}

			//r.AIEconomicFoodScore = r.AIEconomicFoodScore / bestFoodValue;
			//r.AIEconomicWoodScore = r.AIEconomicWoodScore / bestWoodValue;
			//r.AIEconomicGoldScore = r.AIEconomicGoldScore / bestGoldValue;
			//r.AIEconomicMetalScore = r.AIEconomicMetalScore / bestMetalValue;
			r.AIEconomicTotalScore = (r.AIEconomicFoodScore * predictedNeededResources.food) + (r.AIEconomicWoodScore * predictedNeededResources.wood) + (r.AIEconomicGoldScore * predictedNeededResources.gold) + (r.AIEconomicMetalScore * predictedNeededResources.metal);

			if (float.IsNaN (r.AIEconomicTotalScore)) {
				GameManager.print (r.name + " NaN ERROR");
				r.AIEconomicTotalScore = 0;
			}
		}

		//Step 3 - Apply AIPersonality to the values generated in step 2
		if (ai.personality != null) {
			foreach (var r in ai.personality.personalityTraits) {
				bool processed = false;
				foreach (var v in ai.player.playerRace.unitTypes) {
					if (v.name == r.target) {
						v.AICombatScore *= r.modifier;
						processed = true;
						break;
					}
				}

				if (processed == false) {
					foreach (var v in ai.player.playerRace.researchTypes) {
						if (v.name == r.target) {
							v.AICombatScore *= r.modifier;
							break;
						}
					}
				}
			}
		}


		//Step 4 - Convert values for unprerequisited objects into values for their prereqs
		bool prereqDistributionDone = false;
		while (prereqDistributionDone == false) {
			prereqDistributionDone = true;

			foreach (var r in ai.player.playerRace.unitTypes) {
				if (r.AIEconomicTotalScore > 0) {
					int numPrereqsMissing = 0;

					foreach (var v in r.neededResearch) {
						if (ai.player.hasResearch (v) == false) {
							numPrereqsMissing++;
						}
					}

					if (numPrereqsMissing > 0) {
						prereqDistributionDone = false;
						foreach (var v in r.neededResearch) {
							if (ai.player.hasResearch (v) == false) {
								foreach (var q in ai.player.playerRace.researchTypes) {
									if (q.name == v) {
										// Dividing by 1.5 is to reduce the amount of the AIEconomicScore that gets passed on in each wave such that prereq's for other prereqs don't supersede purchases that actually do something
										q.AIEconomicTotalScore += ((r.AIEconomicTotalScore / numPrereqsMissing) / 1.5f);
									}
								}
							}
						}
						r.AIEconomicTotalScore = 0;
					}
				}
			}

			foreach (var r in ai.player.playerRace.researchTypes) {
				if (r.AIEconomicTotalScore > 0) {
					int numPrereqsMissing = 0;

					foreach (var v in r.neededResearch) {
						if (ai.player.hasResearch (v) == false) {
							numPrereqsMissing++;
						}
					}

					if (numPrereqsMissing > 0) {
						prereqDistributionDone = false;
						foreach (var v in r.neededResearch) {
							if (ai.player.hasResearch (v) == false) {
								foreach (var q in ai.player.playerRace.researchTypes) {
									if (q.name == v) {
										// Dividing by 1.5 is to reduce the amount of the AIEconomicScore that gets passed on in each wave such that prereq's for other prereqs don't supersede purchases that actually do something
										q.AIEconomicTotalScore += ((r.AIEconomicTotalScore / numPrereqsMissing) / 1.5f);
									}
								}
							}
						}
						r.AIEconomicTotalScore = 0;
					}
				}
			}
		}

		//Step 5 - Determine optimalValue and divide each value by it
		Purchaseable optimalPurchase = ai.player.playerRace.unitTypes [0];
		float optimalPurchaseScore = 0;

		foreach (var r in ai.player.playerRace.unitTypes) {
			if (r.AIEconomicTotalScore >= optimalPurchaseScore) {
				optimalPurchase = r;
				optimalPurchaseScore = optimalPurchase.AIEconomicTotalScore;
			}
		}

		foreach (var r in ai.player.playerRace.researchTypes) {
			if (r.AIEconomicTotalScore >= optimalPurchaseScore) {
				optimalPurchase = r;
				optimalPurchaseScore = optimalPurchase.AIEconomicTotalScore;
			}
		}

		foreach (var r in ai.player.playerRace.unitTypes) {
			if (float.IsNaN (r.AIEconomicTotalScore) == false) {
				r.AIEconomicTotalScore /= optimalPurchaseScore;
			}
		}

		foreach (var r in ai.player.playerRace.researchTypes) {
			if (float.IsNaN (r.AIEconomicTotalScore) == false) {
				r.AIEconomicTotalScore /= optimalPurchaseScore;
			}
		}
	}

	//This is likely going to deserve a redo at some point
	public void simulateResearchEconomicEffect (Research research) {
		float baseFoodEvaluation = 0;
		float baseWoodEvaluation = 0;
		float baseGoldEvaluation = 0;
		float baseMetalEvaluation = 0;
		float newFoodEvaluation = 0;
		float newWoodEvaluation = 0;
		float newGoldEvaluation = 0;
		float newMetalEvaluation = 0;

		foreach (var r in ai.player.units) {
			if (r.unit.unitType == UnitType.Villager) {
				float tempGather = 0;
				float tempSpeed = 0;
				string identifier = "";

				if (r.unit.buildingTarget != null) {
					if (r.unit.buildingTarget.building.resourceType == ResourceType.Food) {
						if (r.unit.buildingTarget.building.foodType == FoodType.Animal) {
							tempGather = r.unit.foodAnimalGatherRate;
							tempSpeed = r.unit.attackSpeed;
							identifier = "foodAnimalGatherRate";
							baseFoodEvaluation += tempGather * tempSpeed;
						} else if (r.unit.buildingTarget.building.foodType == FoodType.Forage) {
							tempGather = r.unit.foodForageGatherRate;
							tempSpeed = r.unit.attackSpeed;	
							identifier = "foodForageGatherRate";
							baseFoodEvaluation += tempGather * tempSpeed;
						} else if (r.unit.buildingTarget.building.foodType == FoodType.Farm) {
							tempGather = r.unit.foodFarmGatherRate;
							tempSpeed = r.unit.attackSpeed;
							identifier = "foodFarmGatherRate";
							baseFoodEvaluation += tempGather * tempSpeed;
						}
					} else if (r.unit.buildingTarget.building.resourceType == ResourceType.Wood) {
						tempGather = r.unit.woodGatherRate;
						tempSpeed = r.unit.attackSpeed;
						identifier = "woodGatherRate";
						baseWoodEvaluation += tempGather * tempSpeed;
					} else if (r.unit.buildingTarget.building.resourceType == ResourceType.Gold) {
						tempGather = r.unit.goldGatherRate;
						tempSpeed = r.unit.attackSpeed;
						identifier = "goldGatherRate";
						baseGoldEvaluation += tempGather * tempSpeed;
					} else if (r.unit.buildingTarget.building.resourceType == ResourceType.Metal) {
						tempGather = r.unit.metalGatherRate;
						tempSpeed = r.unit.attackSpeed;
						identifier = "metalGatherRate";
						baseMetalEvaluation += tempGather * tempSpeed;
					}
				}

				if (identifier != "") {
					foreach (var e in research.effects) {
						if (e.targetObjectType == "Unit") {
							if (r.unit.GetType ().GetProperty (e.targetVariableIdentifier) != null) {
								if ((string)r.unit.GetType ().GetProperty (e.targetVariableIdentifier).GetValue (r.unit, null) == e.targetVariableValue) {
									if (e.effectVariableIdentifier == identifier) {
										if (e.effectVariableModifier == "+") {
											tempGather += e.effectVariableAmount;
										} else if (e.effectVariableModifier == "*") {
											tempGather = tempGather * e.effectVariableAmount;
										}
									} else if (e.effectVariableIdentifier == "attackSpeed") {
										if (e.effectVariableModifier == "+") {
											tempSpeed += e.effectVariableAmount;
										} else if (e.effectVariableModifier == "*") {
											tempSpeed = tempSpeed * e.effectVariableAmount;
										}
									}
								}
							}
						}
					}

					if (r.unit.buildingTarget.building.resourceType == ResourceType.Food) {
						if (r.unit.buildingTarget.building.foodType == FoodType.Animal) {
							newFoodEvaluation += tempGather * tempSpeed;
						} else if (r.unit.buildingTarget.building.foodType == FoodType.Forage) {
							newFoodEvaluation += tempGather * tempSpeed;
						} else if (r.unit.buildingTarget.building.foodType == FoodType.Farm) {
							newFoodEvaluation += tempGather * tempSpeed;
						}
					} else if (r.unit.buildingTarget.building.resourceType == ResourceType.Wood) {
						newWoodEvaluation += tempGather * tempSpeed;
					} else if (r.unit.buildingTarget.building.resourceType == ResourceType.Gold) {
						newGoldEvaluation += tempGather * tempSpeed;
					} else if (r.unit.buildingTarget.building.resourceType == ResourceType.Metal) {
						newMetalEvaluation += tempGather * tempSpeed;
					}
				}
			}
		}

		research.AIEconomicFoodScore = newFoodEvaluation - baseFoodEvaluation;
		research.AIEconomicWoodScore = newWoodEvaluation - baseWoodEvaluation;
		research.AIEconomicGoldScore = newGoldEvaluation - baseGoldEvaluation;
		research.AIEconomicMetalScore = newMetalEvaluation - baseMetalEvaluation;
	}

	public Resource pastResourcesGathered () {
		// Add up resources gathered in the past 2 minutes
		Resource pastResources = new Resource (0, 0, 0, 0);

		for (int i = ai.player.pastResourcesGathered.Count - 1; i >= 0 && i > ai.player.pastResourcesGathered.Count - 9; i--) {
			pastResources.add (ai.player.pastResourcesGathered [i]);
		}

		return pastResources;
	}
}

