using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public abstract class Research : Purchaseable {


	public List<ResearchEffect> effects { get; protected set; }
	public float queueTime { get; protected set; }

	public void applyOnFinish () {
		foreach (var r in owner.units) {
			applyToUnit (r.unit);
		}

		foreach (var r in owner.playerRace.unitTypes) {
			applyToUnit (r);
		}
	}

	//Below two methods might become obsolete
	//When this is complete it should probably be part of the base research
	public void applyToUnit (Unit _unit) {
		bool relevantToUnit = false;
		if (_unit.hasResearchApplied (name) == false) {
			foreach (var r in effects) {
				if (r.targetObjectType == "Unit") {
					if (_unit.GetType ().GetProperty (r.targetVariableIdentifier) != null) {
						if ((string)_unit.GetType ().GetProperty (r.targetVariableIdentifier).GetValue (_unit, null) == r.targetVariableValue) {
							relevantToUnit = true;
							_unit.activateResearch (r);
						}
					} else {
						GameManager.print ("r.targetVariableIdentifier == null, something broke");
					}
				}
			}
		}

		if (relevantToUnit == true) {
			_unit.researchApplied.Add (this);
		}
	}

	public void applyToBuilding (Building _building) {

	}
}
