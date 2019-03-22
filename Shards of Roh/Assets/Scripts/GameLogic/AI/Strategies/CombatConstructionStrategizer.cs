using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class CombatConstructionStrategizer {

	AIController ai;

	public CombatConstructionStrategizer (AIController _AI) {
		ai = _AI;
	}

	public void calculateOptimalPurchaseValues () {
		//Notes: Currently, this formula takes into account each unit's attack, attack speed, and armour values. It does not account for the following variables that it likely should eventually:
		//Attack range		- Ranged units should have some kind of multiplier on their attack calculating, for both the enemy unit and picking out the optimal one to build, in the formula based on their range
		//Unit Types 		- When infantry/cavalry/etc get involved in the combat math formula, they need to be included here
		//Population cost 	- Units that cost multiple population should probably be adjusted so that their stats are tracked per population number, instead of counting the same
		//Resource cost		- When picking the best unit to build, the formula should take into account their relative costs: an 80 resource 0.9 is better then a 200 resource 1.0
		//Unit health		- At some point, the defense metric should likely be expanded to include unit health
		//ALSO - this entire equation, while returning valid results, greatly overvalues options that are balanced between offensive and defensive options, and should receive a tweak for that bias        

		//Step 1 - ensure that the variables are cleared
		foreach (var r in ai.player.playerRace.unitTypes) {
			r.AIOffensiveScore = 0;
			r.AIDefensiveScore = 0;
			r.AICombatScore = 0;
		}

		foreach (var r in ai.player.playerRace.researchTypes) {
			r.AIOffensiveScore = 0;
			r.AIDefensiveScore = 0;
			r.AICombatScore = 0;
		}

		//Step 1.5 - Handling if the AI has no knowledge of enemy team. Process should be changed from "Doing nothing" at some point
		if (ai.player.visibleObjects.rememberedEnemyUnits.Count == 0) {

		} else {
			//Step 2 - generate a starting value for each unit and research

			//Analyze the enemy military makeup based on VisibleObjectsToPlayer.rememberedEnemyUnits. Eventually this should be replaced with "RememberedEnemyUnits", that's for a future change.
			//Using this analysis, determine what the weakest armour types on the enemy team are, and the least represented attack types.
			float[] attackTypesObserved = attackTypesOnEnemyUnits ();
			float[] armourTypesObserved = armourTypesOnEnemyUnits ();

			//Parse through the player.race.unitTypes list to see what units would best counter the armour types observed
			float bestOffenseEvaluation = 0;
			foreach (var r in ai.player.playerRace.unitTypes) {
				float offenseEvaluation = (r.attack * r.attackSpeed) * (1 - (armourTypesObserved [(int)r.attackType - 1] / 100)); // Higher score = better offensivre evaluation
				r.AIOffensiveScore = offenseEvaluation;

				if (offenseEvaluation > bestOffenseEvaluation) {
					bestOffenseEvaluation = offenseEvaluation;
				}
			}

			foreach (var r in ai.player.playerRace.researchTypes) {
				if (ai.player.hasResearch (r.name) == false) {
					bool isResearchRelevant = false;
					foreach (var e in r.effects) {
						if (e.researchEffectPurpose == ResearchPurpose.Offense || e.researchEffectPurpose == ResearchPurpose.Combat || e.researchEffectPurpose == ResearchPurpose.CombatEconomic) {
							isResearchRelevant = true;
							break;
						}
					}

					if (isResearchRelevant == true) {
						float offenseEvaluation = simulateResearchOffenseEffect (r, armourTypesObserved);
						r.AIOffensiveScore = offenseEvaluation;

						if (offenseEvaluation > bestOffenseEvaluation) {
							bestOffenseEvaluation = offenseEvaluation;
						}
					} else {
						r.AIOffensiveScore = 0;
					}
				} else {
					r.AIOffensiveScore = 0;
				}
			}

			//Parse through the player.race.unitTypes list to see what units would best counter the attack types observed
			float bestDefenseEvaluation = 0;
			foreach (var r in ai.player.playerRace.unitTypes) {
				float defenseEvaluation = (r.armourSlashing * attackTypesObserved [0]) + (r.armourPiercing * attackTypesObserved [1]) + (r.armourBludgeoning * attackTypesObserved [2]) +
				                         (r.armourRanged * attackTypesObserved [3]) + (r.armourSiege * attackTypesObserved [4]) + (r.armourMagic * attackTypesObserved [5]);
				r.AIDefensiveScore = defenseEvaluation;

				if (defenseEvaluation > bestDefenseEvaluation) {
					bestDefenseEvaluation = defenseEvaluation;
				}
			}

			foreach (var r in ai.player.playerRace.researchTypes) {
				if (ai.player.hasResearch (r.name) == false) {
					bool isResearchRelevant = false;
					foreach (var e in r.effects) {
						if (e.researchEffectPurpose == ResearchPurpose.Defense || e.researchEffectPurpose == ResearchPurpose.Combat || e.researchEffectPurpose == ResearchPurpose.CombatEconomic) {
							isResearchRelevant = true;
							break;
						}
					}

					if (isResearchRelevant == true) {
						float defenseEvaluation = simulateResearchDefenseEffect (r, attackTypesObserved);
						r.AIDefensiveScore = defenseEvaluation;

						if (defenseEvaluation > bestDefenseEvaluation) {
							bestDefenseEvaluation = defenseEvaluation;
						}
					} else {
						r.AIDefensiveScore = 0;
					}
				} else {
					r.AIDefensiveScore = 0;
				}
			}

			//Modify each offense/defense value to be in a range from 0 to 1 relative to the best option: 1 means you are the best option, 0 means you have 0 offense to enemies / defense from enemies
			foreach (var r in ai.player.playerRace.unitTypes) {
				r.AIOffensiveScore = r.AIOffensiveScore / bestOffenseEvaluation;
				r.AIDefensiveScore = r.AIDefensiveScore / bestDefenseEvaluation;
				r.AICombatScore = r.AIOffensiveScore + r.AIDefensiveScore;
			}

			foreach (var r in ai.player.playerRace.researchTypes) {
				r.AIOffensiveScore = r.AIOffensiveScore / bestOffenseEvaluation;
				r.AIDefensiveScore = r.AIDefensiveScore / bestDefenseEvaluation;
				r.AICombatScore = r.AIOffensiveScore + r.AIDefensiveScore;
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
											// Dividing by 1.5 is to reduce the amount of the AICombatScore that gets passed on in each wave such that prereq's for other prereqs don't supersede purchases that actually do something
											q.AICombatScore += ((r.AICombatScore / numPrereqsMissing) / 1.5f);
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
											// Dividing by 1.5 is to reduce the amount of the AICombatScore that gets passed on in each wave such that prereq's for other prereqs don't supersede purchases that actually do something
											q.AICombatScore += ((r.AICombatScore / numPrereqsMissing) / 1.5f);
										}
									}
								}
							}
							r.AICombatScore = 0;
						}
					}
				}
			}

			//Step 5 - Determine optimalValue and divide each value by it

			//Compare each potential unit option to see which one has the best stats for the scenario.
			Purchaseable optimalPurchase = ai.player.playerRace.unitTypes [0];
			float totalCombatScoreSums = 0;

			foreach (var r in ai.player.playerRace.unitTypes) {
				totalCombatScoreSums += r.AICombatScore;

				if (r.AICombatScore > optimalPurchase.AICombatScore) {
					optimalPurchase = r;
				}
			}

			foreach (var r in ai.player.playerRace.researchTypes) {
				totalCombatScoreSums += r.AICombatScore;

				if (r.AICombatScore > optimalPurchase.AICombatScore) {
					optimalPurchase = r;
				}
			}

			foreach (var r in ai.player.playerRace.unitTypes) {
				r.AICombatScore /= optimalPurchase.AICombatScore;
			}

			foreach (var r in ai.player.playerRace.researchTypes) {
				r.AICombatScore /= optimalPurchase.AICombatScore;
			}
		}
	}

	//This is likely going to deserve a redo at some point
	public float simulateResearchOffenseEffect (Research research, float[] armourTypesObserved) {
		float baseTotalOffenseEvaluation = 0;
		float newTotalOffenseEvaluation = 0;

		//Parse through the player.race.unitTypes list to see what units would best counter the armour types observed
		foreach (var r in ai.player.units) {
			float tempAttack = r.unit.attack;
			float tempAttackSpeed = r.unit.attackSpeed;

			float baseOffenseEvaluation = (tempAttack * tempAttackSpeed) * (1 - (armourTypesObserved [(int)r.unit.attackType - 1] / 100)); // Higher score = better offensive evaluation

			foreach (var e in research.effects) {
				if (e.targetObjectType == "Unit") {
					if (r.unit.GetType ().GetProperty (e.targetVariableIdentifier) != null) {
						if ((string)r.unit.GetType ().GetProperty (e.targetVariableIdentifier).GetValue (r.unit, null) == e.targetVariableValue) {
							if (e.effectVariableIdentifier == "attack") {
								if (e.effectVariableModifier == "+") {
									tempAttack += e.effectVariableAmount;
								} else if (e.effectVariableModifier == "*") {
									tempAttack = tempAttack * e.effectVariableAmount;
								}
							} else if (e.effectVariableIdentifier == "attackSpeed") {
								if (e.effectVariableModifier == "+") {
									tempAttackSpeed += e.effectVariableAmount;
								} else if (e.effectVariableModifier == "*") {
									tempAttackSpeed = tempAttackSpeed * e.effectVariableAmount;
								}
							}
						}
					}
				}
			}

			float newOffenseEvaluation = (tempAttack * tempAttackSpeed) * (1 - (armourTypesObserved [(int)r.unit.attackType - 1] / 100)); // Higher score = better offensive evaluation

			baseTotalOffenseEvaluation += baseOffenseEvaluation;
			newTotalOffenseEvaluation += newOffenseEvaluation;
		}

		return newTotalOffenseEvaluation - baseTotalOffenseEvaluation;
	}

	//This is likely going to deserve a redo at some point
	public float simulateResearchDefenseEffect (Research research, float[] attackTypesObserved) {
		float baseTotalDefenseEvaluation = 0;
		float newTotalDefenseEvaluation = 0;

		//Parse through the player.race.unitTypes list to see what units would best counter the armour types observed
		foreach (var r in ai.player.units) {
			float tempArmourSlashing = r.unit.armourSlashing;
			float tempArmourPiercing = r.unit.armourPiercing;
			float tempArmourBludgeoning = r.unit.armourBludgeoning;
			float tempArmourRanged = r.unit.armourRanged;
			float tempArmourSiege = r.unit.armourSiege;
			float tempArmourMagic = r.unit.armourMagic;
			float tempHealth = r.unit.health;

			float baseDefenseEvaluation = (tempArmourSlashing * attackTypesObserved [0]) + (tempArmourPiercing * attackTypesObserved [1]) + (tempArmourBludgeoning * attackTypesObserved [2]) + 
				(tempArmourRanged * attackTypesObserved [3]) + (tempArmourSiege * attackTypesObserved [4]) + (tempArmourMagic * attackTypesObserved [5]);

			foreach (var e in research.effects) {
				if (e.targetObjectType == "Unit") {
					if (r.unit.GetType ().GetProperty (e.targetVariableIdentifier) != null) {
						if ((string)r.unit.GetType ().GetProperty (e.targetVariableIdentifier).GetValue (r.unit, null) == e.targetVariableValue) {
							if (e.effectVariableIdentifier == "health") {
								if (e.effectVariableModifier == "+") {
									tempHealth += e.effectVariableAmount;
								} else if (e.effectVariableModifier == "*") {
									tempHealth = tempHealth * e.effectVariableAmount;
								}
							} else if (e.effectVariableIdentifier == "armourSlashing") {
								if (e.effectVariableModifier == "+") {
									tempArmourSlashing += e.effectVariableAmount;
								} else if (e.effectVariableModifier == "*") {
									tempArmourSlashing = tempArmourSlashing * e.effectVariableAmount;
								}
							} else if (e.effectVariableIdentifier == "armourPiercing") {
								if (e.effectVariableModifier == "+") {
									tempArmourPiercing += e.effectVariableAmount;
								} else if (e.effectVariableModifier == "*") {
									tempArmourPiercing = tempArmourPiercing * e.effectVariableAmount;
								}
							} else if (e.effectVariableIdentifier == "armourBludgeoning") {
								if (e.effectVariableModifier == "+") {
									tempArmourBludgeoning += e.effectVariableAmount;
								} else if (e.effectVariableModifier == "*") {
									tempArmourBludgeoning = tempArmourBludgeoning * e.effectVariableAmount;
								}
							} else if (e.effectVariableIdentifier == "armourRanged") {
								if (e.effectVariableModifier == "+") {
									tempArmourRanged += e.effectVariableAmount;
								} else if (e.effectVariableModifier == "*") {
									tempArmourRanged = tempArmourRanged * e.effectVariableAmount;
								}
							} else if (e.effectVariableIdentifier == "armourSiege") {
								if (e.effectVariableModifier == "+") {
									tempArmourSiege += e.effectVariableAmount;
								} else if (e.effectVariableModifier == "*") {
									tempArmourSiege = tempArmourSiege * e.effectVariableAmount;
								}
							} else if (e.effectVariableIdentifier == "armourMagic") {
								if (e.effectVariableModifier == "+") {
									tempArmourMagic += e.effectVariableAmount;
								} else if (e.effectVariableModifier == "*") {
									tempArmourMagic = tempArmourMagic * e.effectVariableAmount;
								}
							}
						}
					}
				}
			}

			float newDefenseEvaluation = (tempArmourSlashing * attackTypesObserved [0]) + (tempArmourPiercing * attackTypesObserved [1]) + (tempArmourBludgeoning * attackTypesObserved [2]) + 
				(tempArmourRanged * attackTypesObserved [3]) + (tempArmourSiege * attackTypesObserved [4]) + (tempArmourMagic * attackTypesObserved [5]);

			baseTotalDefenseEvaluation += baseDefenseEvaluation;
			newTotalDefenseEvaluation += newDefenseEvaluation;
		}

		return newTotalDefenseEvaluation - baseTotalDefenseEvaluation;
	}

	public float[] attackTypesOnEnemyUnits () {
		float[] attackTypesObserved = new float [6];

		foreach (var r in ai.player.visibleObjects.rememberedEnemyUnits) {
			attackTypesObserved [(int)r.unit.attackType - 1] += r.unit.attack * r.unit.attackSpeed;
		}

		//float totalAttackObserved = attackTypesObserved [0] + attackTypesObserved [1] + attackTypesObserved [2] + attackTypesObserved [3] + attackTypesObserved [4] + attackTypesObserved [5];

		if (ai.player.visibleObjects.rememberedEnemyUnits.Count > 0) {
			for (int i = 0; i < 6; i++) {
				attackTypesObserved [i] = attackTypesObserved [i] / ai.player.visibleObjects.rememberedEnemyUnits.Count;
			}
		}

		return attackTypesObserved;
	}

	public float[] armourTypesOnEnemyUnits () {
		float[] armourTypesObserved = new float[6];

		foreach (var r in ai.player.visibleObjects.rememberedEnemyUnits) {
			armourTypesObserved [0] += r.unit.armourSlashing;
			armourTypesObserved [1] += r.unit.armourPiercing;
			armourTypesObserved [2] += r.unit.armourBludgeoning;
			armourTypesObserved [3] += r.unit.armourRanged;
			armourTypesObserved [4] += r.unit.armourSiege;
			armourTypesObserved [5] += r.unit.armourMagic;
		}

		if (ai.player.visibleObjects.rememberedEnemyUnits.Count > 0) {
			for (int i = 0; i < 6; i++) {
				armourTypesObserved [i] = armourTypesObserved [i] / ai.player.visibleObjects.rememberedEnemyUnits.Count;
			}
		}

		return armourTypesObserved;
	}
}

