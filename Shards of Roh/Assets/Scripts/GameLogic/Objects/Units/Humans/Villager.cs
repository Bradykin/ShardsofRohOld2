using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villager : Unit {

	public Villager (Player _owner) {
		setup ();
		name = "Villager";
		race = "Humans";
		owner = _owner;
		health = 100;
		attack = 10;
		attackSpeed = 1.2f;
		attackRange = 1.0f;
		cost = new Resource (0, 0, 50);

		isVillager = true;

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
	}
}
