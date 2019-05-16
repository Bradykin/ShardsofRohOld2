using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public abstract class Unit : ObjectBase {

	//Variables that must be declared in subclass
	public float attack { get; protected set; }
	public float attackRange { get; protected set; }
	public float attackSpeed { get; protected set; } 			//Number of attacks per second
	protected float damageCheck { get; set; }					//Percentage of the way through attack animation that the damage is dealt
	public float moveSpeed { get; protected set; }
	public int populationCost { get; protected set; }
	public int sightRadius { get; protected set; }
	public UnitType unitType { get; protected set; }
	public AttackType attackType { get; protected set; }
	public float queueTime { get; protected set; }
	public int batchSize { get; protected set; }
	public int avoidanceValue { get; set; }

	//Armour Types
	public float armourSlashing { get; protected set; }
	public float armourPiercing { get; protected set; }
	public float armourBludgeoning { get; protected set; }
	public float armourRanged { get; protected set; }
	public float armourSiege { get; protected set; }
	public float armourMagic { get; protected set; }

	//Only relevant if villager
	public float foodAnimalGatherRate { get; protected set; }
	public float foodForageGatherRate { get; protected set; }
	public float foodFarmGatherRate { get; protected set; }
	public float woodGatherRate { get; protected set; }
	public float goldGatherRate { get; protected set; }
	public float metalGatherRate { get; protected set; }
	public float buildRate { get; protected set; }

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
	public float isCommandedRecently { get; set; }
	public int scoutingValue { get; protected set; }

	public void unitSetup () {
		//Temporary
		scoutingValue = 0;

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
		isCommandedRecently = 0;

		foreach (var r in owner.researchList) {
			r.applyToUnit (this);
		}
	}

	public void update () {
		if (isCombatTimer > 0) {
			isCombatTimer -= Time.deltaTime;
		}

		if (isCommandedRecently > 0) {
			isCommandedRecently -= Time.deltaTime;
		}

		if (isCommandedRecently < 0) {
			isCommandedRecently = 0;
		}

		if (curHealth <= 0) {
			isDead = true;
		}

		updateToolTip ();
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
		isAttacking = false;
	}

	public void activateResearch (ResearchEffect _research) {
		if (this.GetType ().GetProperty (_research.effectVariableIdentifier) != null) {
			if (_research.effectVariableModifier == "+") {
				this.GetType ().GetProperty (_research.effectVariableIdentifier).SetValue (this, (float) this.GetType ().GetProperty (_research.effectVariableIdentifier).GetValue (this, null) + _research.effectVariableAmount, null);
			} else if (_research.effectVariableModifier == "*") {
				//This is definitely, 100% flawed for multiplicative multipliers, and will need to be redone. This will net different results based on order of operations
				this.GetType ().GetProperty (_research.effectVariableIdentifier).SetValue (this, (float) this.GetType ().GetProperty (_research.effectVariableIdentifier).GetValue (this, null) * _research.effectVariableAmount, null);
			}
		} else {
			GameManager.print ("r.effectVariableIdentifier == null, something broke - error finding " + _research.effectVariableIdentifier);
		}

		/*if (hasResearchApplied (_research.name) == false) {
			researchApplied.Add (_research);
			if (_research.name == "AnimalTracking") {
				health += 10.0f;
				attack += 1.0f;
				foodAnimalGatherRate += 0.1f;
			} else if (_research.name == "Forestry") {
				foodBerryGatherRate += 0.1f;
				woodGatherRate += 0.1f;
			} else if (_research.name == "MineralExtraction") {
				goldGatherRate += 0.1f;
				metalGatherRate += 0.1f;
			}
		}*/
	}

	public void updateToolTip () {
		tooltipString = name;
	}
}