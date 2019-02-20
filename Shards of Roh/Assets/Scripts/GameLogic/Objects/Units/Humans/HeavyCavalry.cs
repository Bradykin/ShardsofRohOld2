using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyCavalry : Unit {

	public HeavyCavalry (Player _owner) {
		unitSetup ();
		name = "HeavyCavalry";
		race = "Humans";
		owner = _owner;
		health = 100;
		attack = 10;
		attackSpeed = 0.75f;
		attackRange = 2;
		sightRadius = 40;
		cost = new Resource (0, 0, 50, 0);
	}
}
