using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class Food : Building {

	public Food (Player _owner) {
		buildingSetup ();
		name = "Food";
		race = "Nature";
		owner = _owner;
		health = 2000;
		cost = new Resource (2000, 0, 0, 0);

		isResource = true;
		resourceType = ResourceType.Food;
		foodType = FoodType.Forage;
	}
}
