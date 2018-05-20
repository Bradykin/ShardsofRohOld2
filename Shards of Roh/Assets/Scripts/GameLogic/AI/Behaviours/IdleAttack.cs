using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAttack : Behaviours {

	float timer = 0.0f;

	public IdleAttack (UnitContainer _unitInfo) {
		name = "IdleAttack";
		active = true;
		unitInfo = _unitInfo;
	}

	public override void enact () {
		if (active == true) {
			if (unitInfo.unit.isMoving == false && unitInfo.unit.unitTarget == null && unitInfo.unit.buildingTarget == null) {
				timer += Time.deltaTime;

				UnitContainer target = null;

				if (unitInfo.unit.visibleObjects.visibleEnemyUnits.Count > 0) {
					target = unitInfo.unit.visibleObjects.closestEnemyUnit;
				}


				if (target != null) {
					timer = 0.0f;
					unitInfo.unit.setAttackTarget (target);
				}

				if (timer >= 5.0f) {
					active = false;
				}
			} else {
				timer = 0;
			}
		}
	}
}
