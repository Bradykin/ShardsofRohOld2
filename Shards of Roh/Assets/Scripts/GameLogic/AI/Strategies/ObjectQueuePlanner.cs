using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class ObjectQueuePlanner : Strategies {

	OffenseConstructionStrategizer ocs;
	DefenseConstructionStrategizer dcs;
	EconomicConstructionStrategizer ecs;

	public ObjectQueuePlanner (AIController _AI) {
		name = "ObjectQueuePlanner";
		active = true;
		AI = _AI; 
		ocs = new OffenseConstructionStrategizer ();
		dcs = new DefenseConstructionStrategizer ();
		ecs = new EconomicConstructionStrategizer ();
	}

	public override void enact () {
		//Check the values of the vector3 constructionPriorities, to determine how much resources should be offered to each of the miniStrategy functions

		//Consult each miniStrategy on the objects they would like to create

		//Combine these values into a new list, and update creationPriorities to reflect that list
	}
}
