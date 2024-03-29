﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stables : Building {

	public Stables (Player _owner) {
		buildingSetup ();
		name = "Stables";
		race = "Humans";
		owner = _owner;
		health = 200;
		cost = new Resource (0, 200, 0, 0);

		abilities.Add (new AddToUnitQueue ("LightCavalry", this));
		abilities.Add (new AddToUnitQueue ("SpearCavalry", this));
		abilities.Add (new AddToUnitQueue ("HeavyCavalry", this));
		abilities.Add (new AddToUnitQueue ("BowCavalry", this));
		abilities.Add (new AddToResearchQueue ("Horseshoes", this));

		neededResearch.Add ("Age2");
	}
}
