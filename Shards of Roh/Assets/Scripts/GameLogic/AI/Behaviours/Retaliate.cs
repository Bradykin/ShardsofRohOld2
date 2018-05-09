using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Retaliate : Behaviours {

	public Retaliate () {
		name = "Retaliate";
		active = true;
	}

	public override void enact (UnitContainer unitInfo) {
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
