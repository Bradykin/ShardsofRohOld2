using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class ScoutHarry : Behaviours {

	public List<UnitContainer> targets;
	public List<UnitContainer> threats;
	public float timer = 0.0f;

	public ScoutHarry (UnitContainer _unitInfo) {
		name = "ScoutHarry";
		active = true;
		behaviourType = "Scout";
		unitInfo = _unitInfo;
		unitInfo.removeBehaviourByType (behaviourType, this);
		targets = new List<UnitContainer> ();
		threats = new List<UnitContainer> ();
	}

	public void updateTracking () {
		targets.Clear ();
		threats.Clear ();

		foreach (var r in unitInfo.unit.visibleObjects.visibleEnemyUnits) {
			if (r.unit.unitType == UnitType.Villager) {
				targets.Add (r);
			} else {
				threats.Add (r);
			}
		}

		foreach (var r in targets) {
			foreach (var u in r.unit.visibleObjects.visiblePlayerUnits) {
				if (threats.Contains (u) == false) {
					if (u.unit.unitType != UnitType.Villager) {
						threats.Add (u);
					}
				}
			}
		}
	}

	public override void enact () {
		if (active == true) {
			timer += Time.deltaTime;
			updateTracking ();


			//GameManager.print (threats.Count + " --- " + targets.Count);
			if (targets.Count > 0 && threats.Count == 0) {
				float shortestDistanceSqr = 10000000;
				UnitContainer closestTarget = targets [0];
				foreach (var r in targets) {
					float distanceSqr = Vector3.SqrMagnitude (unitInfo.unit.curLoc - r.unit.curLoc);
					if (distanceSqr < shortestDistanceSqr) {
						shortestDistanceSqr = distanceSqr;
						closestTarget = r;
					}
				}

				unitInfo.unit.setAttackTarget (closestTarget);
			} else if (targets.Count > 0 && threats.Count > 0) {
				unitInfo.unit.dropAttackTarget ();

				if (unitInfo.unit.isMoving == false) {
					bool goodSpot = false;
					Vector3 newMoveSpot = new Vector3 (-100, 0, -100);
					int x = 0;
					while (goodSpot == false || x < 25) {
						//newMoveSpot = new Vector3 (Random.Range (unitInfo.unit.curLoc.x - 30, unitInfo.unit.curLoc.x + 30), 0, Random.Range (unitInfo.unit.curLoc.z - 30, unitInfo.unit.curLoc.z + 30));
						Vector2 randomNew = Random.insideUnitCircle * unitInfo.unit.sightRadius;
						newMoveSpot = new Vector3 (unitInfo.unit.curLoc.x + randomNew.x, 0, unitInfo.unit.curLoc.z + randomNew.y);

						bool closeToTarget = false;
						foreach (var r in targets) {
							float distanceSqr = Vector3.SqrMagnitude (newMoveSpot - r.unit.curLoc);
							if (distanceSqr <= unitInfo.unit.sightRadius * unitInfo.unit.sightRadius / 2) {
								closeToTarget = true;
								break;
							}
						}

						bool farFromThreat = true;
						foreach (var r in threats) {
							float distanceSqr = Vector3.SqrMagnitude (newMoveSpot - r.unit.curLoc);
							if (distanceSqr < 25) {
								farFromThreat = false;
								break;
							}
						}

						if (closeToTarget == true && farFromThreat == true) {
							goodSpot = true;
							break;
						}
						x++;
					}

					if (goodSpot == true) {
						unitInfo.moveToLocation (false, newMoveSpot);
					}
				}
			} else if (targets.Count == 00 && threats.Count > 0) {

			} else {

			}
		}
	}
}
