using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class AIEvaluator {

	public bool canCurrentlyBeProduced { get; set; } // Not sure yet what this is
	public float AIOffensiveScore { get; set; }
	public float AIDefensiveScore { get; set; }
	public float AICombatScore { get; set; }
	public float AIEconomicFoodScore { get; set; }
	public float AIEconomicWoodScore { get; set; }
	public float AIEconomicGoldScore { get; set; }
	public float AIEconomicMetalScore { get; set; }
	public float AIEconomicTotalScore { get; set; }
	public float AITotalScore { get; set; }
	public bool viableDraw { get; set; }

	public AIEvaluator (Purchaseable _purchase) {
		//purchase = _purchase;
	}
}

