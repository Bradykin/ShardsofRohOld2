using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swordsman : Unit {

	public Swordsman (Player _owner) {
		name = "Swordsman";
		race = "Humans";
		owner = _owner;
		health = 100;
		attack = 10;
		attackSpeed = 1.333f;
		attackRange = 2;
		cost = new Resource (0, 0, 50);
	}
}
