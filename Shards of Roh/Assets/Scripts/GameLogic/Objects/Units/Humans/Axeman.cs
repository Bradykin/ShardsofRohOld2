using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class Axeman : Unit {

	public Axeman (Player _owner) {

		//Variables from inherited class
		name = "Axeman";
		race = "Humans";
		owner = _owner;
		health = 95;
		cost = new Resource (60, 30, 20, 0);

		unitSetup ();

		//Variables from current class
		attack = 10;
		attackRange = 1.0f;
		attackSpeed = 1.33f;
		damageCheck = 0.5f;
		moveSpeed = 7;
		populationCost = 1;
		sightRadius = 30;
		unitType = UnitType.Infantry;
		attackType = AttackType.Bludgeoning;
		queueTime = 5.0f;
		batchSize = 5;

		//Armour Types - value ranges from 0 to 100. 100 reduces all damage taken, 0 has no effect.
		armourSlashing = 5;
		armourPiercing = 10;
		armourBludgeoning = 20;
		armourRanged = 15;
		armourSiege = 20;
		armourMagic = 15;

		//Abilities

		//Required Research
		neededResearch.Add ("Age2");
	}
}
