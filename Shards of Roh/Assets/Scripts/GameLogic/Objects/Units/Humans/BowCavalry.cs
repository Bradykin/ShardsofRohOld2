using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowCavalry : Unit {

	public BowCavalry (Player _owner) {
		unitSetup ();
		name = "BowCavalry";
		race = "Humans";
		owner = _owner;
		health = 100;
		attack = 10;
		attackSpeed = 1;
		attackRange = 50;
		sightRadius = 40;
		cost = new Resource (0, 0, 50, 0);
	}
}
