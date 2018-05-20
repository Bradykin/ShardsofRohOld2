using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class IdleGather : Behaviours {

	float timer = 0.0f;
	ResourceType resourceType;

	public IdleGather (UnitContainer _unitInfo, ResourceType _resourceType = ResourceType.None) {
		name = "IdleGather";
		active = true;
		unitInfo = _unitInfo;
		resourceType = _resourceType;
	}

	public override void enact () {
		if (active == true) {
			if (unitInfo.unit.isMoving == false && unitInfo.unit.unitTarget == null && unitInfo.unit.buildingTarget == null && unitInfo.unit.isCombatTimer <= 0) {
				timer += Time.deltaTime;

				BuildingContainer target = null;

				if (resourceType == ResourceType.None) {
					if (unitInfo.unit.visibleObjects.visibleResourceBuildings.Count > 0) {
						target = unitInfo.unit.visibleObjects.closestResourceBuilding;
					}
				} else if (resourceType == ResourceType.Food) {
					if (unitInfo.unit.visibleObjects.visibleResourceFood.Count > 0) {
						target = unitInfo.unit.visibleObjects.closestResourceFood;
					}
				} else if (resourceType == ResourceType.Wood) {
					if (unitInfo.unit.visibleObjects.visibleResourceWood.Count > 0) {
						target = unitInfo.unit.visibleObjects.closestResourceWood;
					}
				} else if (resourceType == ResourceType.Gold) {
					if (unitInfo.unit.visibleObjects.visibleResourceGold.Count > 0) {
						target = unitInfo.unit.visibleObjects.closestResourceGold;
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
				timer = 0.0f;
			}
		}
	}
}
