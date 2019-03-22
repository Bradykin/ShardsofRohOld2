using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class Mage : Unit {

	public Mage (Player _owner) {

		//Variables from inherited class
		name = "Mage";
		race = "Humans";
		owner = _owner;
		health = 95;
		cost = new Resource (60, 30, 20, 0);

		unitSetup ();

		//Variables from current class
		attack = 10;
		attackRange = 15.0f;
		attackSpeed = 0.75f;
		damageCheck = 0.5f;
		moveSpeed = 7;
		populationCost = 1;
		sightRadius = 30;
		unitType = UnitType.Infantry;
		attackType = AttackType.Magic;
		queueTime = 5.0f;
		batchSize = 5;

		//Armour Types - value ranges from 0 to 100. 100 reduces all damage taken, 0 has no effect.
		armourSlashing = 0;
		armourPiercing = 0;
		armourBludgeoning = 0;
		armourRanged = 0;
		armourSiege = 0;
		armourMagic = 0;

		//Abilities

		//Required Research
		neededResearch.Add ("Age2");
	}
}
