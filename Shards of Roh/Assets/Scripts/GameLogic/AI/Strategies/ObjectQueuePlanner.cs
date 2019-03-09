using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class ObjectQueuePlanner : Strategies {

	CombatConstructionStrategizer ccs;
	EconomicConstructionStrategizer ecs;

	public ObjectQueuePlanner (AIController _AI) {
		name = "ObjectQueuePlanner";
		active = true;
		AI = _AI; 
		ccs = new CombatConstructionStrategizer (_AI);
		ecs = new EconomicConstructionStrategizer (_AI);
	}

	public override void enact () {
		Vector2 priorityWeighting = new Vector2 (0.5f, 0.5f);

		if (GameManager.gameClock >= 0.5) {
			ccs.creationProposal (priorityWeighting.x);
			ecs.creationProposal (priorityWeighting.y);
		}

		//Consult each miniStrategy on the objects they would like to create

		//Combine these values into a new list, and update creationPriorities to reflect that list
	}
}
