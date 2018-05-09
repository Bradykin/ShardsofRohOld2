using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Building {

	public Food (Player _owner) {
		buildingSetup ();
		name = "Food";
		race = "Nature";
		owner = _owner;
		health = 1000;
		cost = new Resource (10000, 10000, 10000);

		isResource = true;
		resourceType = Enum.ResourceType.Food;
	}
}
