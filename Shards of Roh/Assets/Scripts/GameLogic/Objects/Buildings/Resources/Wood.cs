using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class Wood : Building {

	public Wood (Player _owner) {
		buildingSetup ();
		name = "Wood";
		race = "Nature";
		owner = _owner;
		health = 1000;
		cost = new Resource (10000, 10000, 10000, 10000);

		isResource = true;
		resourceType = ResourceType.Wood;
	}
}
