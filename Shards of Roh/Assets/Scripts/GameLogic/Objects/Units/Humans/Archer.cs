using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Unit {

	public Archer (Player _owner) {
		unitSetup ();
		name = "Archer";
		race = "Humans";
		owner = _owner;
		health = 100;
		attack = 10;
		attackSpeed = 0.75f;
		attackRange = 50;
		sightRadius = 40;
		cost = new Resource (0, 0, 50);
	}
}
