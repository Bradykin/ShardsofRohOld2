using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBuild : Behaviours {

	float timer = 0.0f;

	public IdleBuild (UnitContainer _unitInfo) {
		name = "IdleBuild";
		active = true;
		unitInfo = _unitInfo;
	}

	public override void enact () {
		if (active == true) {
			if (unitInfo.unit.isMoving == false && unitInfo.unit.unitTarget == null && unitInfo.unit.buildingTarget == null && unitInfo.unit.isCombatTimer <= 0) {
				timer += Time.deltaTime;

				BuildingContainer target = null;

				if (unitInfo.unit.visibleObjects.closestPlayerUnbuilt != null) {
					if (unitInfo.unit.visibleObjects.distanceToClosestPlayerUnbuilt <= (unitInfo.unit.sightRadius * unitInfo.unit.sightRadius)) {
						target = unitInfo.unit.visibleObjects.closestPlayerUnbuilt;
					}
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