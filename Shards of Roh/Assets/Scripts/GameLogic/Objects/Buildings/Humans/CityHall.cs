using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityHall : Building {

	public CityHall (Player _owner) {
		buildingSetup ();
		name = "CityHall";
		race = "Humans";
		owner = _owner;
		health = 1000;
		cost = new Resource (0, 0, 50);
	}
}
