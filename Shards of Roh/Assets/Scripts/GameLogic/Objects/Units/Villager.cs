using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villager : Unit {

	public Villager (Player _owner) {
		name = "Villager";
		race = "Humans";
		owner = _owner;
		health = 100;
		attack = 10;
		attackSpeed = 1;
		attackRange = 2;

		abilities.Add (new ToggleBuild ("WatchTower"));
		abilities.Add (new ToggleBuild ("Stables"));
		isVillager = true;
	}
}
