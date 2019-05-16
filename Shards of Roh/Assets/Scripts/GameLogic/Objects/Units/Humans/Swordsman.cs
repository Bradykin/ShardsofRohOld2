using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class Swordsman : Unit {

	public Swordsman (Player _owner) {
		
		//Variables from inherited class
		name = "Swordsman";
		race = "Humans";
		owner = _owner;
		health = 100;
		cost = new Resource (40, 0, 60, 0);

		unitSetup ();

		//Variables from current class
		attack = 9;
		attackRange = 1.0f;
		attackSpeed = 1.333f;
		damageCheck = 0.5f;
		moveSpeed = 7;
		populationCost = 1;
		sightRadius = 50;
		unitType = UnitType.Infantry;
		attackType = AttackType.Slashing;
		queueTime = 5.0f;
		batchSize = 5;

		//Armour Types - value ranges from 0 to 100. 100 reduces all damage taken, 0 has no effect.
		armourSlashing = 30;
		armourPiercing = 20;
		armourBludgeoning = 10;
		armourRanged = 35;
		armourSiege = 25;
		armourMagic = 0;

		//Abilities

		//Required Research
		neededResearch.Add ("Age2");
	}
}
