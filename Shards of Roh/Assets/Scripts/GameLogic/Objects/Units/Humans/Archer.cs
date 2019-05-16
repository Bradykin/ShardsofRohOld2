using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class Archer : Unit {

	public Archer (Player _owner) {
		
		//Variables from inherited class
		name = "Archer";
		race = "Humans";
		owner = _owner;
		health = 75;
		cost = new Resource (40, 70, 0, 0);

		unitSetup ();

		//Variables from current class
		attack = 8;
		attackRange = 20.0f;
		attackSpeed = 0.75f;
		damageCheck = 0.5f;
		moveSpeed = 7;
		populationCost = 1;
		sightRadius = 30;
		unitType = UnitType.Infantry;
		attackType = AttackType.Ranged;
		queueTime = 5.0f;
		batchSize = 5;

		//Armour Types - value ranges from 0 to 100. 100 reduces all damage taken, 0 has no effect.
		armourSlashing = 10;
		armourPiercing = 15;
		armourBludgeoning = 5;
		armourRanged = 40;
		armourSiege = 30;
		armourMagic = 10;

		//Abilities

		//Required Research
		neededResearch.Add ("Age2");
	}
}
