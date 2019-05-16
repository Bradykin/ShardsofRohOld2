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
		scoutingValue = 0;

		//Armour Types - value ranges from 0 to 100. 100 reduces all damage taken, 0 has no effect.
		armourSlashing = 30;
		armourPiercing = 5;
		armourBludgeoning = 5;
		armourRanged = 20;
		armourSiege = 15;
		armourMagic = 20;

		//Abilities

		//Required Research

		/*
		//Variables from inherited class
		name = "Scout";
		race = "Humans";
		owner = _owner;
		health = 60;
		cost = new Resource (0, 70, 0, 0);

		unitSetup ();	

		//Variables from current class
		attack = 2.0f;
		attackRange = 1.0f;
		attackSpeed = 1.2f;
		damageCheck = 0.5f;
		moveSpeed = 11;
		populationCost = 1;
		sightRadius = 50;
		unitType = UnitType.Villager;
		attackType = AttackType.Slashing;
		queueTime = 5.0f;
		batchSize = 1;

		//Armour Types - value ranges from 0 to 100. 100 reduces all damage taken, 0 has no effect.
		armourSlashing = 5;
		armourPiercing = 15;
		armourBludgeoning = 20;
		armourRanged = 80;
		armourSiege = 10;
		armourMagic = 50;

		//Villager stats
		foodAnimalGatherRate = 3.0f;
		foodForageGatherRate = 3.0f;
		foodFarmGatherRate = 3.0f;
		woodGatherRate = 3.0f;
		goldGatherRate = 3.0f;
		metalGatherRate = 3.0f;
		buildRate = 20.0f;

		neededResearch.Add ("Age2");
		*/
	}
}
