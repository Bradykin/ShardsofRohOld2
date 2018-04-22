﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitContainer : ObjectContainer {

	private Unit unit;

	void Start () {
		//Set values for preset units
		setup ();
		//Rest of function
		if (presetOwnerName != "") {
			Player newPlayer = GameManager.addPlayerToGame (presetOwnerName);
			setUnit (ObjectFactory.createUnitByName (gameObject.name, newPlayer));
			if (newPlayer.getUnits ().Contains (this) == false) {
				newPlayer.addUnitToPlayer (this);
			}
			if (gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> () != null) {
				gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().Warp (gameObject.transform.position);
			}
		}

		foreach (var r in gameObject.GetComponentsInChildren<MeshRenderer> ()) {
			if (r.material.name.Contains("WK_Standard")) {
				r.material = Resources.Load (PlayerMaterial.getMaterial (unit.getOwner ().getName ()), typeof (Material)) as Material;
			}
		}
		foreach (var r in gameObject.GetComponentsInChildren<SkinnedMeshRenderer> ()) {
			if (r.material.name.Contains("WK_Standard")) {
				r.material = Resources.Load (PlayerMaterial.getMaterial (unit.getOwner ().getName ()), typeof (Material)) as Material;
			}
		}
	}

	void Update () {
		unit.update ();

		//Update curLoc and isMoving
		if (gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> () != null) {
			unit.setCurLoc (gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().transform.position);
			if (gameObject.GetComponent<Animator> () != null) {
				if (Vector3.Distance (gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().transform.position, gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().destination) <= 0.1) {
					gameObject.GetComponent<Animator> ().SetBool ("isMoving", false);
				} else {
					gameObject.GetComponent<Animator> ().SetBool ("isMoving", true);
				}
			} else {
				print ("Missing Animator - UnitContainer");
			}
		} else {
			print ("Missing NavMeshAgent - UnitContainer");
		}

		//Check for distance to targets, update isAttacking
		if (unit.getUnitTarget () != null && unit.getUnitTarget ().getUnit ().getDead () == false) {
			Vector3 point1 = gameObject.GetComponent<CapsuleCollider> ().bounds.center;
			Vector3 point2 = unit.getUnitTarget ().GetComponent<CapsuleCollider> ().bounds.center;
			point1.y = 0;
			point2.y = 0;
			if (Vector3.Distance (point1, point2) - gameObject.GetComponent<CapsuleCollider> ().radius - unit.getUnitTarget ().GetComponent<CapsuleCollider> ().radius <= unit.getAttackRange ()) {
				gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().destination = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().transform.position;
				lookDirection (GetComponent<CapsuleCollider> ().bounds.center, unit.getUnitTarget ().GetComponent<CapsuleCollider> ().bounds.center);
				gameObject.GetComponent<Animator> ().SetBool ("isAttacking", true);
				unit.attackUnit ();
			}
		} else if (unit.getBuildingTarget () != null && unit.getBuildingTarget ().getBuilding ().getDead () == false) {
			Vector3 point1 = GetComponent<CapsuleCollider> ().bounds.center;
			Vector3 point2 = unit.getBuildingTarget ().GetComponent<BoxCollider> ().ClosestPoint (GetComponent<CapsuleCollider> ().bounds.center);
			point1.y = 0;
			point2.y = 0;
			if (Vector3.Distance (point1, point2) - GetComponent<CapsuleCollider> ().radius <= unit.getAttackRange ()) {
				gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().ResetPath ();
				lookDirection (GetComponent<CapsuleCollider> ().bounds.center, unit.getBuildingTarget ().GetComponent<BoxCollider> ().bounds.center);
				gameObject.GetComponent<Animator> ().SetBool ("isAttacking", true);
				unit.attackBuilding ();
			}
		} else {
			gameObject.GetComponent<Animator> ().SetBool ("isAttacking", false);
			unit.dropAttackTarget ();
			unit.passiveAttackTimer ();
		}

		if (unit.getDead () == true) {
			gameObject.GetComponent<CapsuleCollider> ().enabled = false;
			gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().enabled = false;
			gameObject.GetComponent<Animator> ().SetBool ("isDead", true);
			gameObject.GetComponent<Animator> ().SetInteger ("deathAnimation", Random.Range (0, 2));
			if (gameObject.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("PostDeath")) {
				GameManager.destroyUnit (this, unit.getOwner ());
			}
		}
	}

	private void lookDirection (Vector3 _point1, Vector3 _point2) {
		Vector3 direction = (_point2 - _point1).normalized;
		direction.y = 0;
		Quaternion lookRotation = Quaternion.LookRotation (direction);
		gameObject.transform.rotation = Quaternion.Slerp (gameObject.transform.rotation, lookRotation, Time.deltaTime * 10);
	}

	public void setUnit (Unit _unit) {
		unit = _unit;
	}

	public Unit getUnit () {
		return unit;
	}
}
