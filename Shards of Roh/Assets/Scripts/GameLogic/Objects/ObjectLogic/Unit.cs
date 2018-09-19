using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : ObjectBase {

	//Variables that must be declared in subclass
	public int attack { get; protected set; }
	public float attackRange { get; protected set; }
	protected float attackSpeed { get; set; } 			//Number of attacks per second
	protected float damageCheck { get; set; }			//Percentage of the way through attack animation that the damage is dealt
	public int moveSpeed { get; protected set; }
	public int populationCost { get; protected set; }
	public int sightRadius { get; protected set; }
	public bool isVillager { get; protected set; }
	public float queueTime { get; protected set; }
	public int batchSize { get; protected set; }
	public int avoidanceValue { get; set; }

	//Variables that adjust during gameplay
	public UnitContainer unitTarget { get; protected set; }
	public BuildingContainer buildingTarget { get; protected set; }
	protected float attackTimer { get; set; }
	public bool hasHit { get; protected set; }
	public bool isCombat { get; private set; }
	public float isCombatTimer { get; private set; }
	public bool isMoving { get; set; }
	public bool isAttacking { get; set; }
	public bool gotHit { get; set; }
	public UnitContainer gotHitBy  { get; set; }
	public List<Vector3> moveDestinations { get; private set; }
	public Vector3 flagPosition { get; set; }

	public void unitSetup () {
		setup ();

		//Variables from inherited class
		prefabPath = "Prefabs/" + race + "/Units/" + name;

		//Variables from current class
		moveDestinations = new List<Vector3> ();
	}

	public void initPostCreate () {
		//Variables from current class
		curHealth = health;
		isCombatTimer = 0;
		isMoving = false;
		isAttacking = false;
		gotHit = false;
		gotHitBy = null;
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
		if (unitTarget.unit.owner.name != "Nature") {
			isCombatTimer = 5.0f;
		}
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
		if (buildingTarget.building.owner.name != "Nature" || buildingTarget.building.owner.name == owner.name) {
			isCombatTimer = 5.0f;
		}
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
		gotHit = true;
		gotHitBy = _attacker;
		isCombatTimer = 5.0f;
		curHealth -= _attack;
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
}