using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class Spearman : Unit {

	public Spearman (Player _owner) {

		//Variables from inherited class
		name = "Spearman";
		race = "Humans";
		owner = _owner;
		health = 90;
		cost = new Resource (50, 50, 0, 0);

		unitSetup ();

		//Variables from current class
		attack = 9;
		attackRange = 1.0f;
		attackSpeed = 1.0f;
		damageCheck = 0.5f;
		moveSpeed = 7;
		populationCost = 1;
		sightRadius = 50;
		unitType = UnitType.Infantry;
		attackType = AttackType.Slashing;
		queueTime = 5.0f;
		batchSize = 5;

		//Armour Types - value ranges from 0 to 100. 100 reduces all damage taken, 0 has no effect.
		armourSlashing = 20;
		armourPiercing = 50;
		armourBludgeoning = 15;
		armourRanged = 25;
		armourSiege = 20;
		armourMagic = 10;

		//Abilities

		//Required Research
		neededResearch.Add ("Age2");
	}
}
