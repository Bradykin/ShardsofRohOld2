﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class AddToUnitQueue : Ability {

	private string unitName;
	private Building source;

	public AddToUnitQueue (string _unitName, Building _source) {
		unitName = _unitName;
		source = _source;
		name = "Spawn " + _unitName;
		targetType = TargetType.None;
	}

	//If you have the required resources and required prerequisites for this unit, add it to the queue of the source building
	public override void enact (Player owner, Unit targetUnit = null, Building targetBuilding = null, Vector3 targetPos = new Vector3 ()) {
		Unit newUnit = ObjectFactory.createUnitByName (unitName, GameManager.addPlayerToGame (owner.name));
		if (source.isBuilt) {
			if (owner.hasPopulationSpace (newUnit.populationCost)) {
				if (owner.resource.hasEnough (newUnit.cost)) {
					bool hasResearch = true;
					foreach (var r in newUnit.neededResearch) {
						if (owner.hasResearch (r) == false) {
							hasResearch = false;
						}
					}
					if (hasResearch == true) {
						owner.resource.spend (newUnit.cost);
						source.addToUnitQueue (newUnit);
					}
				}
			}
		}
	}
}