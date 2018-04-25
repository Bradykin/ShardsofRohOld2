using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Retaliate : Behaviours {

	public Retaliate () {
		name = "Retaliate";
	}

	public override void enact (UnitContainer unitInfo) {
		if (unitInfo.getUnit ().gotHit == true) {
			if (unitInfo.getUnit ().getUnitTarget () == null && unitInfo.getUnit ().getBuildingTarget () == null && unitInfo.getUnit ().isMoving == false && unitInfo.getUnit ().isAttacking == false) {
				if (unitInfo.getUnit ().gotHitBy != null) {
					unitInfo.getUnit ().setAttackTarget (unitInfo.getUnit ().gotHitBy);
				} else {
					GameManager.print ("Can't find got hit by - Retaliate");
				}
			}
		}
	}
}
