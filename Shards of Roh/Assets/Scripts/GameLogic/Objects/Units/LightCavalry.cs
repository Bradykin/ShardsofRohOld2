using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCavalry : Unit {

	public LightCavalry (Player _owner) {
		name = "LightCavalry";
		race = "Humans";
		owner = _owner;
		health = 100;
		attack = 10;
		attackSpeed = 1;
		attackRange = 2;
	}
}
