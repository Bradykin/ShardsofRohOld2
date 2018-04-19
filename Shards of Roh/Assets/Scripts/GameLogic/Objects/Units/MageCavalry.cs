using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageCavalry : Unit {

	public MageCavalry (Player _owner) {
		name = "MageCavalry";
		race = "Humans";
		owner = _owner;
		health = 100;
		attack = 10;
		attackSpeed = 0.75f;
		attackRange = 2;
		cost = new Resource (0, 0, 50);
	}
}
