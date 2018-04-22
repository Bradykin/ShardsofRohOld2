using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stables : Building {

	public Stables (Player _owner) {
		name = "Stables";
		race = "Humans";
		owner = _owner;
		health = 1000;
		cost = new Resource (0, 0, 50);

		abilities.Add (new QueueUnit ("LightCavalry", this));
		abilities.Add (new QueueUnit ("SpearCavalry", this));
		abilities.Add (new QueueUnit ("HeavyCavalry", this));
		abilities.Add (new QueueUnit ("BowCavalry", this));
	}
}
