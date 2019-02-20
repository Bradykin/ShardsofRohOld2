using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : Unit {

	public Mage (Player _owner) {
		unitSetup ();
		name = "Mage";
		race = "Humans";
		owner = _owner;
		health = 100;
		attack = 10;
		attackSpeed = 0.75f;
		attackRange = 20;
		sightRadius = 40;
		cost = new Resource (0, 0, 50, 0);
	}
}
