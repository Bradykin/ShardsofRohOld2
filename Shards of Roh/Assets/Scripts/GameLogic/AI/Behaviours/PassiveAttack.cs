using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAttack : Behaviours {

	float timer = 1.0f;
	float checkAt = 1.0f;

	public PassiveAttack (UnitContainer _unitInfo) {
		name = "PassiveAttack";
		active = true;
		unitInfo = _unitInfo;
	}

	//INCOMPLETE PLACEHOLDER FUNCTION
	public override void enact () {
		if (active == true) {
			timer += Time.deltaTime;

			if (timer >= checkAt) {
				if (unitInfo.unit.isMoving == false && unitInfo.unit.unitTarget == null && unitInfo.unit.buildingTarget == null) {

					float distanceToCollider = unitInfo.unit.sightRadius * unitInfo.unit.sightRadius;
					List<UnitContainer> units = unitInfo.unit.visibleObjects.visibleEnemyUnits;
					UnitContainer target = null;

					for (int i = 0; i < units.Count; i++) {
						float xPos = Mathf.Abs (unitInfo.unit.curLoc.x - units [i].unit.curLoc.x);
						float zPos = Mathf.Abs (unitInfo.unit.curLoc.z - units [i].unit.curLoc.z);

						if (xPos < unitInfo.unit.sightRadius && zPos < unitInfo.unit.sightRadius) {
							float distance = Vector3.SqrMagnitude (unitInfo.unit.curLoc - units [i].unit.curLoc);
							if (distance <= distanceToCollider) {
								distanceToCollider = distance;
								target = units [i];
							}
						}
					}


					if (target != null) {
						timer = 0.0f;
						checkAt = 0.0f;
						GameManager.print (target.unit.owner.name);
						unitInfo.unit.setAttackTarget (target);
					} else {
						checkAt += 1.0f;
					}
				}
			}
		}
	}
}
