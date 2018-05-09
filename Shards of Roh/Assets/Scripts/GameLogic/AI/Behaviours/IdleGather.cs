using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleGather : Behaviours {

	float timer = 1.0f;
	float checkAt = 1.0f;

	public IdleGather () {
		name = "IdleGather";
		active = true;
	}

	public override void enact (UnitContainer unitInfo) {
		if (active == true) {
			timer += Time.deltaTime;

			if (timer >= checkAt) {
				if (unitInfo.unit.isMoving == false && unitInfo.unit.unitTarget == null && unitInfo.unit.buildingTarget == null) {

					float distanceToCollider = unitInfo.unit.sightRadius * unitInfo.unit.sightRadius;
					List<BuildingContainer> resources = GameManager.addPlayerToGame ("Nature").buildings;
					BuildingContainer target = null;

					for (int i = 0; i < resources.Count; i++) {
						if (resources [i].building.isResource == true) {
							float distance = Vector3.SqrMagnitude (unitInfo.unit.curLoc - resources [i].building.curLoc);
							if (distance <= distanceToCollider) {
								distanceToCollider = distance;
								target = resources [i];
							}
						}
					}

					if (target != null) {
						timer = 0.0f;
						checkAt = 0.0f;
						unitInfo.unit.setAttackTarget (target);
					} else {
						checkAt += 1.0f;
					}

					if (checkAt >= 5.0f) {
						active = false;
					}
				}
			}
		}
	}
}
