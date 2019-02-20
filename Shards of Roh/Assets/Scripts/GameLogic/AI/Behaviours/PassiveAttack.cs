using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PassiveAttack Behaviour gives a unit the behaviour of "If not attacking or moving, and there is an enemy unit nearby, set them as attack target
//Similar to IdleAttack except doesn't disable itself idling
//Unlike Idle behaviours, Passive behaviours have multiple various criteria for triggering, based on what the unit is currently doing, and are permanent
public class PassiveAttack : Behaviours {

	float timer = 1.0f;
	float checkAt = 1.0f;

	public PassiveAttack (UnitContainer _unitInfo) {
		name = "PassiveAttack";
		active = true;
		behaviourType = "Passive"; 
		unitInfo = _unitInfo;
	}

	//INCOMPLETE PLACEHOLDER FUNCTION
	public override void enact () {
		if (active == true) {
			timer += Time.deltaTime;

			if (timer >= checkAt) {
				if (unitInfo.unit.isMoving == false && unitInfo.unit.unitTarget == null && unitInfo.unit.buildingTarget == null) {

					UnitContainer target = null;

					if (unitInfo.unit.visibleObjects.visibleEnemyUnits.Count > 0) {
						target = unitInfo.unit.visibleObjects.closestEnemyUnit;
					}

					if (target != null) {
						timer = 0.0f;
						checkAt = 0.0f;
						unitInfo.unit.setAttackTarget (target);
					} else {
						checkAt += 1.0f;
					}
				}
			}
		}
	}
}