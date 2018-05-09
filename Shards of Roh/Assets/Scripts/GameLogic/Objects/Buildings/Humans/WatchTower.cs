using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchTower : Building {

	public WatchTower (Player _owner) {
		buildingSetup ();
		name = "WatchTower";
		race = "Humans";
		owner = _owner;
		health = 1000;
		cost = new Resource (0, 0, 50);
	}
}
