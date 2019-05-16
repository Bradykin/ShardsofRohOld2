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
		health = 2000;
		cost = new Resource (0, 2000, 0, 0);

		isResource = true;
		resourceType = ResourceType.Wood;
	}
}
