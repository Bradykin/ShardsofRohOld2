using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PassiveRetaliate Behaviour gives a unit the behaviour of "If attacked, and not attacking or moving, set the attacker as attack target
//Might make this trigger whilst attacking/moving, or make a second version that does that. Undecided.
//Unlike Idle behaviours, Passive behaviours have multiple various criteria for triggering, based on what the unit is currently doing, and are permanent
public class PassiveRetaliate : Behaviours {

	public PassiveRetaliate (UnitContainer _unitInfo) {
		name = "PassiveRetaliate";
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