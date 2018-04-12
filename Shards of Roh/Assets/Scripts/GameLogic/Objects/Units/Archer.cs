using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Unit {

	public Archer (Player _owner) {
		name = "Archer";
		race = "Humans";
		owner = _owner;
		health = 100;
		attack = 10;
		attackSpeed = 1;
		attackRange = 50;
	}
}
