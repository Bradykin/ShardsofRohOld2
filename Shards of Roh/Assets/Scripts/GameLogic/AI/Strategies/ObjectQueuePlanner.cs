using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class ObjectQueuePlanner : Strategies {

	SpendingPrioritizer sp;
	CombatConstructionStrategizer ccs;
	EconomicConstructionStrategizer ecs;
	BuildingConstructionStrategizer bcs;

	public ObjectQueuePlanner (AIController _AI) {
		name = "ObjectQueuePlanner";
		active = true;
		AI = _AI;
		sp = new SpendingPrioritizer (AI);
		ccs = new CombatConstructionStrategizer (AI);
		ecs = new EconomicConstructionStrategizer (AI);
		bcs = new BuildingConstructionStrategizer (AI);
		interval = 15;
	}

	public override void enact () {
		if (GameManager.gameClock >= 0.5) {
			interval += Time.deltaTime;
			if (interval >= 15 || AI.creationQueue.Count == 0) {
				interval = 0;
				ccs.calculateOptimalPurchaseValues ();
				ecs.calculateOptimalPurchaseValues ();

				float totalRange = 0;
				foreach (var r in AI.player.playerRace.unitTypes) {
					r.AITotalScore = (r.AICombatScore * sp.combatPriority) + (r.AIEconomicTotalScore * sp.economicPriority);
					totalRange += r.AITotalScore;
				}

				foreach (var r in AI.player.playerRace.researchTypes) {
					r.AITotalScore = (r.AICombatScore * sp.combatPriority) + (r.AIEconomicTotalScore * sp.economicPriority);
					totalRange += r.AITotalScore;
				}
				
				if (float.IsNaN (totalRange) == false) {
					List<Purchaseable> objectList = generateList (totalRange, 250 * Mathf.Min (10, 1 + (int)GameManager.gameClock / 60));

					sortList (objectList);
					bcs.calculateBuildingPurchases (objectList);

					foreach (var r in objectList) {
						GameManager.print ("Purchase: " + r.name);
						string type = "";
						if (r is Unit) {
							type = "Unit";
						} else if (r is Building) {
							type = "Building";
						} else if (r is Research) {
							type = "Research";
						} else {
							GameManager.print ("Can't match object Type - ObjectQueuePlanner");
						}

						AI.creationQueue = objectList;
					}
				}
			}
		}

		//Consult each miniStrategy on the objects they would like to create

		//Combine these values into a new list, and update creationPriorities to reflect that list
	}

	public void sortList (List <Purchaseable> list) {
		for (int i = 0; i < list.Count; i++) {
			Purchaseable temp = list [i];
			int randomIndex = Random.Range (i, list.Count);
			list [i] = list [randomIndex];
			list [randomIndex] = temp;
		}
	}

	public List <Purchaseable> generateList (float randomDrawRange, float resourceBudget) {
		List <Purchaseable> purchases = new List<Purchaseable> ();
		resetDraws ();

		while (resourceBudget > 0) {
			Purchaseable toPurchase = null;
			float matchDraw = Random.Range (0, randomDrawRange);
			bool hasMatch = false;

			float matchCompare = 0;
			for (int i = 0; i < AI.player.playerRace.unitTypes.Count; i++) {
				if (AI.player.playerRace.unitTypes [i].viableDraw == true && hasMatch == false) {
					matchCompare += AI.player.playerRace.unitTypes [i].AITotalScore;
					if (matchDraw <= matchCompare) {
						toPurchase = ObjectFactory.createUnitByName (AI.player.playerRace.unitTypes [i].name, AI.player);
						hasMatch = true;
					}
				}
			}

			for (int i = 0; i < AI.player.playerRace.researchTypes.Count; i++) {
				if (AI.player.playerRace.researchTypes [i].viableDraw == true && hasMatch == false) {
					matchCompare += AI.player.playerRace.researchTypes [i].AITotalScore;
					if (matchDraw <= matchCompare) {
						AI.player.playerRace.researchTypes [i].viableDraw = false;
						randomDrawRange -= AI.player.playerRace.researchTypes [i].AITotalScore;
						toPurchase = ResearchFactory.createResearchByName (AI.player.playerRace.researchTypes [i].name, AI.player);
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
		foreach (var r in AI.player.playerRace.unitTypes) {
			r.viableDraw = true;
		}

		foreach (var r in AI.player.playerRace.researchTypes) {
			r.viableDraw = true;
		}
	}
}
