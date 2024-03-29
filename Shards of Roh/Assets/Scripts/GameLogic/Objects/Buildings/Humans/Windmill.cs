﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windmill : Building {

	public Windmill (Player _owner) {
		buildingSetup ();
		name = "Windmill";
		race = "Humans";
		owner = _owner;
		health = 1000;
		cost = new Resource (50, 100, 0, 0);

		abilities.Add (new AddToResearchQueue ("Forestry", this));
		abilities.Add (new AddToResearchQueue ("AnimalTracking", this));
		abilities.Add (new AddToResearchQueue ("MineralExtraction", this));
	}
}
