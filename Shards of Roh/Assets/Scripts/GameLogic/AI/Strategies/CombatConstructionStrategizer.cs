using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

class CombatConstructionStrategizer {

	Player player;

	public CombatConstructionStrategizer (AIController _AI) {
		player = _AI.player;
	}

	public void unitCreationProposal () {
		//Step 1 - analyze the enemy military makeup based on VisibleObjectsToPlayer.VisibleEnemyUnits. Eventually this should be replaced with "RememberedEnemyUnits", that's for a future change.
		float[] attackTypesObserved = attackTypesOnEnemyUnits ();
		float[] armourTypesObserved = armourTypesOnEnemyUnits ();

		Unit optimalAttackUnit = player.playerRace.unitTypes [0];
		float bestOffenseEvaluation = 0;
		foreach (var r in player.playerRace.unitTypes) {
			float offenseEvaluation = (r.attack * r.attackSpeed) / armourTypesObserved [(int)r.attackType - 1]; // Higher score = better offensivre evaluation

			if (offenseEvaluation > bestOffenseEvaluation) {
				optimalAttackUnit = r;
				bestOffenseEvaluation = offenseEvaluation;
			}
		}
		GameManager.print ("BEST ATTACK UNIT: " + optimalAttackUnit.name);

		Unit optimalArmourUnit = player.playerRace.unitTypes [0];
		float bestDefenseEvaluation = 0;
		foreach (var r in player.playerRace.unitTypes) {
			float defenseEvaluation = (r.armourSlashing * attackTypesObserved [0]) + (r.armourPiercing * attackTypesObserved [1]) + (r.armourBludgeoning * attackTypesObserved [2]) + 
				(r.armourRanged * attackTypesObserved [3]) + (r.armourSiege * attackTypesObserved [4]) + (r.armourMagic * attackTypesObserved [5]);

			if (defenseEvaluation > bestDefenseEvaluation) {
				optimalArmourUnit = r;
				bestDefenseEvaluation = defenseEvaluation;
			}
		}
		GameManager.print ("BEST ARMOUR UNIT: " + optimalArmourUnit.name);

		//Step 2 - Using previous analysis, determine what the weakest armour types on the enemy team are, and the least represented attack types.

		//Step 3 - Search the player.race.unitTypes list to see what units best defeat this composition. Propose a a large portion of the army be represented by that, and then smaller portions for other units. 
		//Don't suggest units that the player's comp directly counters.
	}

	public float[] attackTypesOnEnemyUnits () {
		float[] attackTypesObserved = new float [6];

		foreach (var r in player.visibleObjects.visibleEnemyUnits) {
			attackTypesObserved [(int)r.unit.attackType - 1] += r.unit.attack * r.unit.attackSpeed;
		}

		float totalAttackObserved = attackTypesObserved [0] + attackTypesObserved [1] + attackTypesObserved [2] + attackTypesObserved [3] + attackTypesObserved [4] + attackTypesObserved [5];

		if (totalAttackObserved != 0) {
			for (int i = 0; i < 6; i++) {
				attackTypesObserved [i] = attackTypesObserved [i] / totalAttackObserved;
			}
		}

		return attackTypesObserved;
	}

	public float[] armourTypesOnEnemyUnits () {
		float[] armourTypesObserved = new float[6];

		foreach (var r in player.visibleObjects.visibleEnemyUnits) {
			armourTypesObserved [0] += r.unit.armourSlashing;
			armourTypesObserved [1] += r.unit.armourPiercing;
			armourTypesObserved [2] += r.unit.armourBludgeoning;
			armourTypesObserved [3] += r.unit.armourRanged;
			armourTypesObserved [4] += r.unit.armourSiege;
			armourTypesObserved [5] += r.unit.armourMagic;
		}

		float totalArmourObserved = armourTypesObserved [0] + armourTypesObserved [1] + armourTypesObserved [2] + armourTypesObserved [3] + armourTypesObserved [4] + armourTypesObserved [5];

		if (totalArmourObserved != 0) {
			for (int i = 0; i < 6; i++) {
				armourTypesObserved [i] = armourTypesObserved [i] / totalArmourObserved;
			}
		}

		return armourTypesObserved;
	}
}

