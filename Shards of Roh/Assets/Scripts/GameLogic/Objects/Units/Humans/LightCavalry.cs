using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCavalry : Unit {

	public LightCavalry (Player _owner) {
		unitSetup ();
		name = "LightCavalry";
		race = "Humans";
		owner = _owner;
		health = 100;
		attack = 10;
		attackSpeed = 1.0f;
		attackRange = 2.0f;
		sightRadius = 40;
		cost = new Resource (0, 0, 50);
	}
}
