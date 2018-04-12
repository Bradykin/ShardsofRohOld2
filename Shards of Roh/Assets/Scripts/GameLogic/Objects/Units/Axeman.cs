using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axeman : Unit {

	public Axeman (Player _owner) {
		name = "Axeman";
		race = "Humans";
		owner = _owner;
		health = 100;
		attack = 10;
		attackSpeed = 1;
		attackRange = 2;
	}
}
