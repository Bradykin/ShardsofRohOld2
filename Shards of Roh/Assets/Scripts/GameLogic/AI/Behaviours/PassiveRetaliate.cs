using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PassiveAttack Behaviour gives a unit the behaviour of "If not attacking or moving, and there is an enemy unit nearby, set them as attack target
//Similar to IdleAttack except doesn't disable itself idling
//Unlike Idle behaviours, Passive behaviours have multiple various criteria for triggering, based on what the unit is currently doing, and are permanent
public class PassiveRetaliate : Behaviours {

	public PassiveRetaliate (UnitContainer _unitInfo) {
		name = "PassiveRetaliatee";
		active = true;
		behaviourType = "Passive";
		unitInfo = _unitInfo;
	}

	public override void enact () {
		if (unitInfo.unit.gotHit == true) {
			if (unitInfo.unit.unitTarget == null && unitInfo.unit.buildingTarget == null && unitInfo.unit.isMoving == false && unitInfo.unit.isAttacking == false) {
				if (unitInfo.unit.gotHitBy != null) {
					unitInfo.unit.setAttackTarget (unitInfo.unit.gotHitBy);
				} else {
					GameManager.print ("Can't find got hit by - PassiveRetaliate");
				}
			}
		}
	}
}