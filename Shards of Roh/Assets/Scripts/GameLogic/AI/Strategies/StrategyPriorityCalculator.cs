using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class StrategyPriorityCalculator : Strategies {

	public float offenseConstructionPriority { get; private set; }
	public float defenseConstructionPriority { get; private set; }
	public float economicConstructionPriority { get; private set; }

	public float offenseCommandingPriority { get; private set; }
	public float defenseCommandingPriority { get; private set; }
	public float scoutingCommandingPriority { get; private set; }

	public StrategyPriorityCalculator (AIController _AI) {
		name = "StrategyPriorityCalculator";
		active = true;
		AI = _AI; 
	}

	public override void enact () {
		//Do some process to determine the amount that each of the strategies is prioritized

		//Set the values of the vector3's constructionPriorities and commandingPriorities in AIController equal to the values calculated here.
	}
}
