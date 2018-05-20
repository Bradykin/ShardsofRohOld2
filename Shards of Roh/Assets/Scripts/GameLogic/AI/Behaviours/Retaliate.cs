using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Retaliate : Behaviours {

	public Retaliate (UnitContainer _unitInfo) {
		name = "Retaliate";
		active = true;
		unitInfo = _unitInfo;
	}

	public override void enact () {
		if (unitInfo.unit.gotHit == true) {
			if (unitInfo.unit.unitTarget == null && unitInfo.unit.buildingTarget == null && unitInfo.unit.isMoving == false && unitInfo.unit.isAttacking == false) {
				if (unitInfo.unit.gotHitBy != null) {
					unitInfo.unit.setAttackTarget (unitInfo.unit.gotHitBy);
				} else {
					GameManager.print ("Can't find got hit by - Retaliate");
				}
			}
		}
	} 
}
