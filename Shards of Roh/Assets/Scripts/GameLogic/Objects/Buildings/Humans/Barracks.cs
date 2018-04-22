using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barracks : Building {

	public Barracks (Player _owner) {
		name = "Barracks";
		race = "Humans";
		owner = _owner;
		health = 1000;
		cost = new Resource (0, 0, 50);

		abilities.Add (new QueueUnit ("Axeman", this));
		abilities.Add (new QueueUnit ("Spearman", this));
		abilities.Add (new QueueUnit ("Swordsman", this));
		abilities.Add (new QueueUnit ("HeavyInfantry", this));
		abilities.Add (new QueueUnit ("Archer", this));
	}
}
