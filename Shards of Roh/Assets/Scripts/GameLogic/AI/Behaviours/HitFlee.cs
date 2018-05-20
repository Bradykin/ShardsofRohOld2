using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFlee : Behaviours {

	UnitContainer fleeing;

	public HitFlee (UnitContainer _unitInfo) {
		name = "HitFlee";
		fleeing = null;
		active = true;
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
