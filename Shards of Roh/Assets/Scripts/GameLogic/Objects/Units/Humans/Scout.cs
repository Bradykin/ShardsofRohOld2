using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class Scout : Unit {

	public Scout (Player _owner) {
		
		//Variables from inherited class
		name = "Scout";
		race = "Humans";
		owner = _owner;
		health = 75;
		cost = new Resource (0, 50, 0, 0);

		unitSetup ();

		//Variables from current class
		attack = 3;
		attackRange = 1.0f;
		attackSpeed = 1.0f;
		damageCheck = 0.5f;
		moveSpeed = 11;
		populationCost = 1;
		sightRadius = 30;
		unitType = UnitType.Cavalry;
		attackType = AttackType.Slashing;
		queueTime = 5.0f;
		batchSize = 5;

		//Armour Types - value ranges from 0 to 100. 100 reduces all damage taken, 0 has no effect.
		armourSlashing = 20;
		armourPiercing = 5;
		armourBludgeoning = 5;
		armourRanged = 60;
		armourSiege = 30;
		armourMagic = 60;

		//Abilities

		//Required Research
	}
}
