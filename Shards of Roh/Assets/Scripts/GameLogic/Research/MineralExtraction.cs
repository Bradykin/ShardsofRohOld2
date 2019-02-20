using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class MineralExtraction : Research {

	public MineralExtraction (Player _owner) {
		owner = _owner;
		name = "MineralExtraction";
		cost = new Resource (0, 50, 100, 0);
		queueTime = 5;
		neededResearch = new List<Research> ();
	}

	public override void applyOnFinish () {
		foreach (var r in owner.units) {
			applyToUnit (r.unit);
		}
	}

	//Hate this system, but don't have a better one at the moment
	public override void applyToUnit (Unit _unit) {
		if (_unit.unitType == UnitType.Villager) {
			if (_unit.hasResearchApplied (name) == false) {
				if (_unit.name == "Worker") {
					//_unit.foodBerryGatherRate += 1;
					//_unit.woodGatherRate += 1;
					//_unit.researchApplied.Add (this);
					_unit.activateResearch (this);
				}
			}
		}
	}

	public override void applyToBuilding (Building _building) {

	}
}
