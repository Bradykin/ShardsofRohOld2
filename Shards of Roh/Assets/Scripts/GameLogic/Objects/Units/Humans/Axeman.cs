﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axeman : Unit {

	public Axeman (Player _owner) {
		unitSetup ();
		name = "Axeman";
		race = "Humans";
		owner = _owner;
		health = 100;
		attack = 10;
		attackSpeed = 1.333f;
		attackRange = 2;
		sightRadius = 40;
		cost = new Resource (0, 0, 50);

		neededResearch.Add (ResearchFactory.createResearchByName ("Age2", _owner));
	}
}
