using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : Object {

	//Variables that must be declared in subclass
	protected int attack;
	//Number of attacks per second
	protected float attackSpeed;

	//Variables that will default if not declared
	protected int populationCost = 1;
	protected float attackRange = 2.0f;
	protected float damageCheck = 0.5f;
	protected bool isVillager = false;

	//Variables that adjust during gameplay
	protected UnitContainer unitTarget;
	protected BuildingContainer buildingTarget;
	protected float attackTimer;
	protected bool hasHit;
	protected float isCombatTimer = 0;

	public void initPostCreate () {
		curHealth = health;
		isDead = false;
	}

	public void update () {
		if (isCombatTimer > 0) {
			isCombatTimer -= Time.deltaTime;
		}

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

	public bool attackUnit () {
		bool shouldHit = false;
		isCombatTimer = 5.0f;
		attackTimer += Time.deltaTime;
		if ((attackTimer * (1.0f / damageCheck)) >= (1.0f / attackSpeed) && hasHit == false) {
			if (unitTarget != null) {
				shouldHit = true;
			} else {
				GameManager.print ("Missing UnitTarget - Unit");
			}
			hasHit = true;
		}
		if (attackTimer >= (1 / attackSpeed)) {
			attackTimer = attackTimer - (1 / attackSpeed);
			hasHit = false;
		}

		return shouldHit;
	}

	public bool attackBuilding () {	
		bool shouldHit = false;
		isCombatTimer = 5.0f;
		attackTimer += Time.deltaTime;
		if ((attackTimer * (1.0f / damageCheck)) >= (1.0f / attackSpeed) && hasHit == false) {
			if (buildingTarget != null) {
				shouldHit = true;
			} else {
				GameManager.print ("Missing BuildingTarget - Unit");
			}
			hasHit = true;
		}
		if (attackTimer >= (1 / attackSpeed)) {
			attackTimer = attackTimer - (1 / attackSpeed);
			hasHit = false;
		}

		return shouldHit;
	}

	public void getHit (UnitContainer _attacker, float _attack) {
		/*if (unitTarget == null && buildingTarget == null && isMoving == false) {
			setAttackTarget (_attacker);
		}*/
		isCombatTimer = 5.0f;
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

	public int getAttack () {
		return attack;
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

	public int getPopulationCost () {
		return populationCost;
	}

	public float getIsCombatTimer () {
		return isCombatTimer;
	}
}