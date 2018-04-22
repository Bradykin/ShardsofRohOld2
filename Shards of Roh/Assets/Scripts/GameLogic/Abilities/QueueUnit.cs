using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class QueueUnit : Ability {

	private string unitName;
	private Building source;

	public QueueUnit (string _unitName, Building _source) {
		unitName = _unitName;
		source = _source;
		name = "Spawn " + _unitName;
		targetType = TargetType.None;
	}

	public override void enact (Player owner, Unit targetUnit = null, Building targetBuilding = null, Vector3 targetPos = new Vector3 ()) {
		Unit newUnit = ObjectFactory.createUnitByName (unitName, GameManager.addPlayerToGame (owner.getName ()));
		if (owner.getResource ().hasEnough (newUnit.getCost ())) {
			bool hasResearch = true;
			foreach (var r in newUnit.getNeededResearch ()) {
				if (owner.hasResearch (r) == false) {
					hasResearch = false;
				}
			}
			if (hasResearch == true) {
				owner.getResource ().spend (newUnit.getCost ());
				source.addToUnitQueue (newUnit);
			}
		}
	}
}