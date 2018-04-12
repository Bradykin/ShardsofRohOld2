using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catapult : Unit {

	public Catapult (Player _owner) {
		name = "Catapult";
		race = "Humans";
		owner = _owner;
		health = 100;
		attack = 10;
		attackSpeed = 1;
		attackRange = 50;
	}
}
