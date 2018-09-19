using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//IdleAttack Behaviour gives a unit the behaviour of "If not attacking or moving, and there is an enemy unit nearby, set them as attack target
//Might extend this to include idle attacking buildings, haven't decided.
//Disables after idling for 5 seconds
public class IdleAttack : Behaviours {

	float timer = 0.0f;

	public IdleAttack (UnitContainer _unitInfo) {
		name = "IdleAttack";
		active = true;
		behaviourType = "Idle";
		unitInfo = _unitInfo;
		unitInfo.removeBehaviourByType (behaviourType, this);
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
