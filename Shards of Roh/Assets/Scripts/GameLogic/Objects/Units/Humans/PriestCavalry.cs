﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestCavalry : Unit {

	public PriestCavalry (Player _owner) {
		name = "PriestCavalry";
		race = "Humans";
		owner = _owner;
		health = 100;
		attack = 10;
		attackSpeed = 0.75f;
		attackRange = 2;
		cost = new Resource (0, 0, 50);
	}
}