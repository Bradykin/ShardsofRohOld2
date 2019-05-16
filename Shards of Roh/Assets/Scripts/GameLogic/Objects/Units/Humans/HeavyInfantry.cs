using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class HeavyInfantry : Unit {

	public HeavyInfantry (Player _owner) {
		
		//Variables from inherited class
		name = "HeavyInfantry";
		race = "Humans";
		owner = _owner;
		health = 105;
		cost = new Resource (60, 30, 40, 20);

		unitSetup ();

		//Variables from current class
		attack = 10;
		attackRange = 1.0f;
		attackSpeed = 1.33f;
		damageCheck = 0.5f;
		moveSpeed = 7;
		populationCost = 1;
		sightRadius = 50;
		unitType = UnitType.Infantry;
		attackType = AttackType.Bludgeoning;
		queueTime = 5.0f;
		batchSize = 5;

		//Armour Types - value ranges from 0 to 100. 100 reduces all damage taken, 0 has no effect.
		armourSlashing = 40;
		armourPiercing = 35;
		armourBludgeoning = 20;
		armourRanged = 50;
		armourSiege = 15;
		armourMagic = 10;

		//Abilities

		//Required Research
		neededResearch.Add ("Age3");
	}
}
