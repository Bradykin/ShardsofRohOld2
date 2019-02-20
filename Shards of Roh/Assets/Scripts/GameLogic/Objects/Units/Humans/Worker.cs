﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class Worker : Unit {

	public Worker (Player _owner) {

		//Variables from inherited class
		name = "Worker";
		race = "Humans";
		owner = _owner;
		health = 60;
		cost = new Resource (50, 0, 0, 0);

		unitSetup ();	

		//Variables from current class
		attack = 4;
		attackRange = 1.0f;
		attackSpeed = 1.2f;
		damageCheck = 0.5f;
		moveSpeed = 6;
		populationCost = 1;
		sightRadius = 50;
		unitType = UnitType.Villager;
		attackType = AttackType.Slashing;
		queueTime = 5.0f;
		batchSize = 1;

		//Armour Types - value ranges from 0 to 100. 100 reduces all damage taken, 0 has no effect.
		armourSlashing = 0;
		armourPiercing = 0;
		armourBludgeoning = 0;
		armourRanged = 0;
		armourSiege = 0;
		armourMagic = 0;

		//Villager stats
		/*foodAnimalGatherRate = 1.4f;
		foodBerryGatherRate = 1.2f;
		foodFarmGatherRate = 1.1f;
		woodGatherRate = 1.0f;
		goldGatherRate = 1.2f;
		metalGatherRate = 1.0f;
		buildRate = 10.0f;*/

		foodAnimalGatherRate = 3.0f;
		foodBerryGatherRate = 3.0f;
		foodFarmGatherRate = 3.0f;
		woodGatherRate = 3.0f;
		goldGatherRate = 3.0f;
		metalGatherRate = 3.0f;
		buildRate = 20.0f;

		//Abilities
		abilities.Add (new ToggleBuild ("TownCentre"));
		//abilities.Add (new ToggleBuild ("House"));
		//abilities.Add (new ToggleBuild ("Wall"));
		abilities.Add (new ToggleBuild ("Windmill"));

		abilities.Add (new ToggleBuild ("Barracks"));
		abilities.Add (new ToggleBuild ("Stables"));
		abilities.Add (new ToggleBuild ("WatchTower"));
		abilities.Add (new ToggleBuild ("Blacksmith"));
		abilities.Add (new ToggleBuild ("Tavern"));

		abilities.Add (new ToggleBuild ("Fort"));
		abilities.Add (new ToggleBuild ("Cathedral"));
		abilities.Add (new ToggleBuild ("CityHall"));

		//Required Research
	}
}
