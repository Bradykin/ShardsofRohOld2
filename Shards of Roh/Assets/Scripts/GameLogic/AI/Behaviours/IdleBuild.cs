using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBuild : Behaviours {

	float timer = 1.0f;
	float checkAt = 1.0f;

	public IdleBuild () {
		name = "IdleBuild";
		active = true;
	}

	public override void enact (UnitContainer unitInfo) {
		if (active == true) {
			timer += Time.deltaTime;

			if (timer >= checkAt) {
				if (unitInfo.unit.isMoving == false && unitInfo.unit.unitTarget == null && unitInfo.unit.buildingTarget == null) {
					/*Collider[] hitColliders = Physics.OverlapSphere (unitInfo.unit.curLoc, unitInfo.unit.sightRadius);

				float distanceToCollider = unitInfo.unit.sightRadius;

				for (int i = 0; i < hitColliders.Length; i++) {
					BuildingContainer check = hitColliders [i].gameObject.GetComponent<BuildingContainer> ();
					if (check != null) {
						if (check.building.isResource == true) {
							if (Vector3.Distance (unitInfo.unit.curLoc, check.building.curLoc) <= distanceToCollider) {
								distanceToCollider = Vector3.Distance (unitInfo.unit.curLoc, check.building.curLoc);
								unitInfo.unit.setAttackTarget (check);
								timer = 0;
							}
						}
					}
				}*/

					float distanceToCollider = unitInfo.unit.sightRadius * unitInfo.unit.sightRadius;
					List<BuildingContainer> buildings = GameManager.addPlayerToGame (unitInfo.unit.owner.name).buildings;
					BuildingContainer target = null;

					for (int i = 0; i < buildings.Count; i++) {
						if (buildings [i].building.isBuilt == false) {
							float distance = Vector3.SqrMagnitude (unitInfo.unit.curLoc - buildings [i].building.curLoc);
							if (distance <= distanceToCollider) {
								distanceToCollider = distance;
								target = buildings [i];
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
