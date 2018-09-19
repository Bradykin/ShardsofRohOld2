using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PassiveAttack Behaviour gives a unit the behaviour of "If not attacking or moving, and there is an enemy unit nearby, set them as attack target
//Similar to IdleAttack except doesn't disable itself idling
//Unlike Idle behaviours, Passive behaviours have multiple various criteria for triggering, based on what the unit is currently doing, and are permanent
public class PassiveFlee : Behaviours {

	UnitContainer fleeing;

	public PassiveFlee (UnitContainer _unitInfo) {
		name = "PassiveFlee";
		fleeing = null;
		active = true;
		behaviourType = "Passive";
		unitInfo = _unitInfo;
	}

	public override void enact () {
		if (unitInfo.unit.gotHit == true && unitInfo.unit.isMoving == false) {
			fleeing = unitInfo.unit.gotHitBy;
			Vector3 fleeTo = unitInfo.unit.curLoc + ((unitInfo.unit.curLoc - fleeing.unit.curLoc).normalized * 8);
			unitInfo.moveToLocation (fleeTo);
		}

		if (fleeing != null && unitInfo.unit.isMoving == false) {
			if (Vector3.SqrMagnitude (unitInfo.unit.curLoc - fleeing.unit.curLoc) > 50) {
				fleeing = null;
			} else {
				Vector3 fleeTo = unitInfo.unit.curLoc + ((unitInfo.unit.curLoc - fleeing.unit.curLoc).normalized * 8);
				unitInfo.moveToLocation (fleeTo);
			}
		}
	}
}