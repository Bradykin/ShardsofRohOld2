using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : Unit {

	public Worker (Player _owner) {
		unitSetup ();

		//Variables from inherited class
		name = "Worker";
		race = "Humans";
		owner = _owner;
		health = 100;
		cost = new Resource (0, 0, 50);

		//Variables from current class
		attack = 1	;
		attackRange = 1.0f;
		attackSpeed = 1.2f;
		damageCheck = 0.5f;
		moveSpeed = 6;
		populationCost = 1;
		sightRadius = 15;
		isVillager = true;
		queueTime = 5.0f;
		batchSize = 1;

		//Unit Abilities
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

		//Unit Research Required
	}
}
