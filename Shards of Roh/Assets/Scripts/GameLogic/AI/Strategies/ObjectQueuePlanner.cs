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
		//float[] listTest = ccs.attackTypesOnEnemyUnits ();
		//float[] listTest = ccs.armourTypesOnEnemyUnits ();


		ccs.unitCreationProposal ();
		//GameManager.print ("ATTACKTYPESOBSERVED: " + listTest[0] + ", " + listTest[1] + ", " + listTest[2] + ", " + listTest[3] + ", " + listTest[4] + ", " + listTest[5]);
		//Check the values of the vector3 constructionPriorities, to determine how much resources should be offered to each of the miniStrategy functions

		//Consult each miniStrategy on the objects they would like to create

		//Combine these values into a new list, and update creationPriorities to reflect that list
	}
}
