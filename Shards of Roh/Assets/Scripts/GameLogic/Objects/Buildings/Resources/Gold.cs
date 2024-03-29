﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class Gold : Building {

	public Gold (Player _owner) {
		buildingSetup ();
		name = "Gold";
		race = "Nature";
		owner = _owner;
		health = 1000;
		cost = new Resource (0, 0, 1000, 0);

		isResource = true;
		resourceType = ResourceType.Gold;
	}
}
