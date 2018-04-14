﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swordsman : Unit {

	public Swordsman (Player _owner) {
		name = "Swordsman";
		race = "Humans";
		owner = _owner;
		health = 100;
		attack = 10;
		attackSpeed = 1;
		attackRange = 2;
	}
}