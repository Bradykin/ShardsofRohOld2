using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyInfantry : Unit {

	public HeavyInfantry (Player _owner) {
		unitSetup ();
		name = "HeavyInfantry";
		race = "Humans";
		owner = _owner;
		health = 100;
		attack = 10;
		attackSpeed = 1;
		attackRange = 2;
		sightRadius = 40;
		cost = new Resource (0, 0, 50);
	}
}
