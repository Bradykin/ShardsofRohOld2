using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spearman : Unit {

	public Spearman (Player _owner) {
		name = "Spearman";
		race = "Humans";
		owner = _owner;
		health = 100;
		attack = 10;
		attackSpeed = 1;
		attackRange = 2;
		cost = new Resource (0, 0, 50);
	}
}
