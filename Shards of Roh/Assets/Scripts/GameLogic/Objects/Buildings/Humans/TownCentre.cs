using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownCentre : Building {

	public TownCentre (Player _owner) {
		name = "TownCentre";
		race = "Humans";
		owner = _owner;
		health = 1000;
		cost = new Resource (0, 0, 50);

		abilities.Add (new QueueUnit ("Villager", this));
		abilities.Add (new QueueResearch ("Age2", this));
	}
}
