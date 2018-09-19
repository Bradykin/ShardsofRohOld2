using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//IdleBuild Behaviour gives a unit the behaviour of "If not attacking or moving, and there is an unbuilt friendly building nearby, set it as attack target.
//Disables after idling for 5 seconds
public class IdleBuild : Behaviours {

	float timer = 0.0f;

	public IdleBuild (UnitContainer _unitInfo) {
		name = "IdleBuild";
		active = true;
		behaviourType = "Idle";
		unitInfo = _unitInfo;
		unitInfo.removeBehaviourByType (behaviourType, this);
	}

	public override void enact () {
		if (active == true) {
			if (unitInfo.unit.isMoving == false && unitInfo.unit.unitTarget == null && unitInfo.unit.buildingTarget == null && unitInfo.unit.isCombatTimer <= 0) {
				timer += Time.deltaTime;

				BuildingContainer target = null;

				if (unitInfo.unit.visibleObjects.closestPlayerUnbuilt != null) {
					if (unitInfo.unit.visibleObjects.distanceToClosestPlayerUnbuiltSqr <= (unitInfo.unit.sightRadius * unitInfo.unit.sightRadius)) {
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