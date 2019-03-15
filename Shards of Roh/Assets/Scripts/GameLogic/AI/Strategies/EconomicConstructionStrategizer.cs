using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class EconomicConstructionStrategizer {

	Player player;

	public EconomicConstructionStrategizer (AIController _AI) {
		player = _AI.player;
	}

	public List<Purchaseable> creationProposal (float _priority, AIPersonality _personality) {
		float randomDrawRange = calculateOptimalPurchaseValues (_personality);
		float resourceBudget = _priority * 1000 * Mathf.Min (10, 1 + (int)GameManager.gameClock / 60);

		List <Purchaseable> proposal = new List <Purchaseable> ();

		if (float.IsNaN (randomDrawRange) == false && randomDrawRange > 0) {
			proposal = generateList (randomDrawRange, resourceBudget);
		} else {
			//GameManager.print (randomDrawRange);
		}

		return proposal;
	}

	public float calculateOptimalPurchaseValues (AIPersonality _personality) {
		Resource predictedNeededResources = pastResourcesGathered ();

		float bestFoodValue = 0;
		float bestWoodValue = 0;
		float bestGoldValue = 0;
		float bestMetalValue = 0;

		foreach (var r in player.playerRace.unitTypes) {
			r.AIEconomicFoodScore = 0;
			r.AIEconomicWoodScore = 0;
			r.AIEconomicGoldScore = 0;
			r.AIEconomicMetalScore = 0;
			r.AIEconomicTotalScore = 0;
		}

		foreach (var r in player.playerRace.researchTypes) {
			r.AIEconomicFoodScore = 0;
			r.AIEconomicWoodScore = 0;
			r.AIEconomicGoldScore = 0;
			r.AIEconomicMetalScore = 0;
			r.AIEconomicTotalScore = 0;
		}

		foreach (var r in player.playerRace.unitTypes) {
			if (r.unitType == UnitType.Villager) {
				float bestUnitFood = 0;

				if (r.foodAnimalGatherRate * r.attackSpeed > bestUnitFood && player.visibleObjects.canAccessFoodAnimal == true) {
					bestUnitFood = r.foodAnimalGatherRate * r.attackSpeed;
				}

				if (r.foodForageGatherRate * r.attackSpeed > bestUnitFood && player.visibleObjects.canAccessFoodForage == true) {
					bestUnitFood = r.foodForageGatherRate * r.attackSpeed;
				}

				if (r.foodFarmGatherRate * r.attackSpeed > bestUnitFood && player.visibleObjects.canAccessFoodFarm == true) {
					bestUnitFood = r.foodFarmGatherRate * r.attackSpeed;
				}
				r.AIEconomicFoodScore = bestUnitFood;

				if (r.AIEconomicFoodScore > bestFoodValue) {
					bestFoodValue = bestUnitFood;
				}

				if (player.visibleObjects.canAccessWood == true) {
					r.AIEconomicWoodScore = r.woodGatherRate * r.attackSpeed;
					if (r.AIEconomicWoodScore > bestWoodValue) {
						bestWoodValue = r.AIEconomicWoodScore;
					}
				}

				if (player.visibleObjects.canAccessGold == true) {
					r.AIEconomicGoldScore = r.goldGatherRate * r.attackSpeed;
					if (r.AIEconomicGoldScore > bestGoldValue) {
						bestGoldValue = r.AIEconomicGoldScore;
					}
				}
				if (player.visibleObjects.canAccessMetal == true) {
					r.AIEconomicMetalScore = r.metalGatherRate * r.attackSpeed;
					if (r.AIEconomicMetalScore > bestMetalValue) {
						bestMetalValue = r.AIEconomicMetalScore;
					}
				}

				//GameManager.print (r.name + " STATS: " + r.AIEconomicFoodScore + ", " + r.AIEconomicWoodScore + ", " + r.AIEconomicGoldScore + ", " + r.AIEconomicMetalScore);
			}
		}

		foreach (var r in player.playerRace.researchTypes) {
			r.AIEconomicTotalScore = 0;
			if (player.hasResearch (r) == false) {
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

					//GameManager.print (r.name + " STATS: " + r.AIEconomicFoodScore + ", " + r.AIEconomicWoodScore + ", " + r.AIEconomicGoldScore + ", " + r.AIEconomicMetalScore);
				}
			}
		}

		//GameManager.print ("BEST VALUES: " + bestFoodValue + ", " + bestWoodValue + ", " + bestGoldValue + ", " + bestMetalValue);

		Purchaseable optimalPurchase = player.playerRace.unitTypes [0];
		float optimalPurchaseScore = 0;
		float totalEconomicScoreSums = 0;

		foreach (var r in player.playerRace.unitTypes) {
			r.AIEconomicFoodScore = r.AIEconomicFoodScore / bestFoodValue;
			r.AIEconomicWoodScore = r.AIEconomicWoodScore / bestWoodValue;
			r.AIEconomicGoldScore = r.AIEconomicGoldScore / bestGoldValue;
			r.AIEconomicMetalScore = r.AIEconomicMetalScore / bestMetalValue;
			r.AIEconomicTotalScore = (r.AIEconomicFoodScore * predictedNeededResources.food) + (r.AIEconomicWoodScore * predictedNeededResources.wood) + (r.AIEconomicGoldScore * predictedNeededResources.gold) + (r.AIEconomicMetalScore * predictedNeededResources.metal);

			if (float.IsNaN (r.AIEconomicTotalScore)) {
				GameManager.print (r.name + "NaN ERROR");
				r.AIEconomicTotalScore = 0;
			}

			if (r.AIEconomicTotalScore >= optimalPurchaseScore) {
				optimalPurchase = r;
				optimalPurchaseScore = optimalPurchase.AIEconomicTotalScore;
			}
		}

		foreach (var r in player.playerRace.researchTypes) {
			r.AIEconomicFoodScore = r.AIEconomicFoodScore / bestFoodValue;
			r.AIEconomicWoodScore = r.AIEconomicWoodScore / bestWoodValue;
			r.AIEconomicGoldScore = r.AIEconomicGoldScore / bestGoldValue;
			r.AIEconomicMetalScore = r.AIEconomicMetalScore / bestMetalValue;
			r.AIEconomicTotalScore = (r.AIEconomicFoodScore * predictedNeededResources.food) + (r.AIEconomicWoodScore * predictedNeededResources.wood) + (r.AIEconomicGoldScore * predictedNeededResources.gold) + (r.AIEconomicMetalScore * predictedNeededResources.metal);

			if (float.IsNaN (r.AIEconomicTotalScore)) {
				GameManager.print (r.name + "NaN ERROR");
				r.AIEconomicTotalScore = 0;
			}

			if (r.AIEconomicTotalScore >= optimalPurchaseScore) {
				optimalPurchase = r;
				optimalPurchaseScore = optimalPurchase.AIEconomicTotalScore;
			}
		}
			
		//GameManager.print (optimalPurchaseScore + "OPTIMAL");
		foreach (var r in player.playerRace.unitTypes) {
			if (float.IsNaN (r.AIEconomicTotalScore) == false) {
				r.AIEconomicTotalScore /= optimalPurchaseScore;
				r.AIEconomicTotalScore = r.AIEconomicTotalScore * r.AIEconomicTotalScore;
				totalEconomicScoreSums += r.AIEconomicTotalScore;
				//GameManager.print ("TICK: " + r.name + " " + totalEconomicScoreSums);
			}
		}

		foreach (var r in player.playerRace.researchTypes) {
			if (float.IsNaN (r.AIEconomicTotalScore) == false) {
				r.AIEconomicTotalScore /= optimalPurchaseScore;
				r.AIEconomicTotalScore = r.AIEconomicTotalScore * r.AIEconomicTotalScore;
				totalEconomicScoreSums += r.AIEconomicTotalScore;
				//GameManager.print ("TICK: " + r.name + " " + totalEconomicScoreSums);
			}
		}

		player.playerRace.unitTypes.Sort ((x, y) => y.AIEconomicTotalScore.CompareTo (x.AIEconomicTotalScore));
		player.playerRace.researchTypes.Sort ((x, y) => y.AIEconomicTotalScore.CompareTo (x.AIEconomicTotalScore));

		return totalEconomicScoreSums;
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

		foreach (var r in player.units) {
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

	public List <Purchaseable> generateList (float randomDrawRange, float resourceBudget) {
		List <Purchaseable> purchases = new List<Purchaseable> ();
		resetDraws ();

		while (resourceBudget > 0) {
			Purchaseable toPurchase = null;
			float matchDraw = Random.Range (0, randomDrawRange);
			bool hasMatch = false;

			float matchCompare = 0;
			for (int i = 0; i < player.playerRace.unitTypes.Count; i++) {
				if (player.playerRace.unitTypes [i].viableDraw == true && hasMatch == false) {
					matchCompare += player.playerRace.unitTypes [i].AIEconomicTotalScore;
					if (matchDraw <= matchCompare) {
						toPurchase = ObjectFactory.createUnitByName (player.playerRace.unitTypes [i].name, player);
						hasMatch = true;
					}
				}
			}

			for (int i = 0; i < player.playerRace.researchTypes.Count; i++) {
				if (player.playerRace.researchTypes [i].viableDraw == true && hasMatch == false) {
					matchCompare += player.playerRace.researchTypes [i].AIEconomicTotalScore;
					if (matchDraw <= matchCompare) {
						player.playerRace.researchTypes [i].viableDraw = false;
						randomDrawRange -= player.playerRace.researchTypes [i].AIEconomicTotalScore;
						toPurchase = ResearchFactory.createResearchByName (player.playerRace.researchTypes [i].name, player);
						hasMatch = true;
					}
				}
			}
				
			//Purchaseable toPurchase = matchDraw (Random.Range (0, randomDrawRange));
			if (toPurchase != null) {
				resourceBudget -= toPurchase.cost.getTotal ();
				purchases.Add (toPurchase);
			} else {
				GameManager.print ("MatchDraw - Out of Bounds");
			}
		}

		return purchases;
	}

	public void resetDraws () {
		foreach (var r in player.playerRace.unitTypes) {
			r.viableDraw = true;
		}

		foreach (var r in player.playerRace.researchTypes) {
			r.viableDraw = true;
		}
	}

	public Resource pastResourcesGathered () {
		// Add up resources gathered in the past 2 minutes
		Resource pastResources = new Resource (0, 0, 0, 0);

		for (int i = player.pastResourcesGathered.Count - 1; i >= 0 && i > player.pastResourcesGathered.Count - 9; i--) {
			pastResources.add (player.pastResourcesGathered [i]);
		}

		return pastResources;
	}
}

