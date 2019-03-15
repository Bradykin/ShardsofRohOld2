using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public abstract class Purchaseable {

	//Variables that must be declared in subclass
	public string name { get; protected set; }
	public string race { get; protected set; }
	public Player owner { get; set; }
	public Resource cost { get; protected set; }
	public AIInterpret interpret { get; protected set; }
	public List<Research> neededResearch { get; protected set; }

	//Variables used by the AI decision maker
	public bool canCurrentlyBeProduced { get; set; }
	public float AIOffensiveScore { get; set; }
	public float AIDefensiveScore { get; set; }
	public float AIRelativeScore { get; set; }
	public float AIEconomicFoodScore { get; set; }
	public float AIEconomicWoodScore { get; set; }
	public float AIEconomicGoldScore { get; set; }
	public float AIEconomicMetalScore { get; set; }
	public float AIEconomicTotalScore { get; set; }
	public float AITotalScore { get; set; }
	public bool viableDraw { get; set; }


	protected void purchaseSetup () {
		interpret = new AIInterpret ();
	}
}

