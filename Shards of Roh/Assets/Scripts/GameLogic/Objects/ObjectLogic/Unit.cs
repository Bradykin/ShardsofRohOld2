using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : Object {

	//Variables that must be declared in subclass
	protected int attack;
	//Number of attacks per second
	protected float attackSpeed;

	//Variables that will default if not declared
	protected float attackRange = 2.0f;
	protected float damageCheck = 0.5f;
	protected bool isVillager = false;

	//Variables that adjust during gameplay
	protected UnitContainer unitTarget;
	protected BuildingContainer buildingTarget;
	protected float attackTimer;
	protected bool hasHit;

	public void initPostCreate () {
		curHealth = health;
		isDead = false;
	}

	public void update () {
		if (curHealth <= 0) {
			isDead = true;
		}
	}

	public void passiveAttackTimer () {
		if (hasHit == true) {
			attackTimer += Time.deltaTime;
		} else {
			attackTimer = 0.0f;
		}
		if (attackTimer > (1 / attackSpeed)) {
			attackTimer = 0.0f;
			hasHit = false;
		}
	}

	public void attackUnit () {
		attackTimer += Time.deltaTime;
		if ((attackTimer * (1.0f / damageCheck)) >= (1.0f / attackSpeed) && hasHit == false) {
			if (unitTarget != null) {
				unitTarget.getUnit ().getHit (this, attack);
			} else {
				GameManager.print ("Missing UnitTarget - Unit");
			}
			hasHit = true;
		}
		if (attackTimer >= (1 / attackSpeed)) {
			attackTimer = attackTimer - (1 / attackSpeed);
			hasHit = false;
		}
	}

	public void attackBuilding () {	
		attackTimer += Time.deltaTime;
		if ((attackTimer * (1.0f / damageCheck)) >= (1.0f / attackSpeed) && hasHit == false) {
			if (buildingTarget != null) {
				buildingTarget.getBuilding ().getHit (this, attack);
			} else {
				GameManager.print ("Missing BuildingTarget - Unit");
			}
			hasHit = true;
		}
		if (attackTimer >= (1 / attackSpeed)) {
			attackTimer = attackTimer - (1 / attackSpeed);
			hasHit = false;
		}
	}

	public void getHit (Unit _attacker, float _attack) {
		curHealth -= _attack;
		GameManager.print ("gotHit: " + curHealth + "/" + health);
	}

	public override string getPrefabPath () {
		return "Prefabs/" + race + "/Units/" + name;
	}

	public UnitContainer getUnitTarget () {
		return unitTarget;
	}

	public BuildingContainer getBuildingTarget () {
		return buildingTarget;
	}

	public float getAttackRange () {
		return attackRange;
	}

	public void setAttackTarget (UnitContainer _unitTarget) {
		unitTarget = _unitTarget;
		buildingTarget = null;
	}

	public void setAttackTarget (BuildingContainer _buildingTarget) {
		buildingTarget = _buildingTarget;
		unitTarget = null;
	}

	public void dropAttackTarget () {
		unitTarget = null;
		buildingTarget = null;
	}

	public bool getVillager () {
		return isVillager;
	}

	public bool getDead () {
		return isDead;
	}
}