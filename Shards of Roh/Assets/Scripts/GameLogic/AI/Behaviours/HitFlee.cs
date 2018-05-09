using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFlee : Behaviours {

	UnitContainer fleeing;

	public HitFlee () {
		name = "HitFlee";
		fleeing = null;
	}
	
	public override void enact (UnitContainer unitInfo) {
		if (unitInfo.unit.gotHit == true) {
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
