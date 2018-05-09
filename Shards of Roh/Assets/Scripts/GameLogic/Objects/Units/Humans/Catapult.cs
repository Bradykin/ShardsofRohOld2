using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catapult : Unit {

	public Catapult (Player _owner) {
		unitSetup ();
		name = "Catapult";
		race = "Humans";
		owner = _owner;
		health = 100;
		attack = 10;
		attackSpeed = 0.6f;
		attackRange = 50;
		sightRadius = 40;
		cost = new Resource (0, 0, 50);
	}
}
