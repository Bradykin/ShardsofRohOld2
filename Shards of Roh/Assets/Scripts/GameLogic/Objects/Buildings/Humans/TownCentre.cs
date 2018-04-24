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
		populationValue = 10;

		abilities.Add (new AddToUnitQueue ("Villager", this));
		abilities.Add (new AddToResearchQueue ("Age2", this));
	}
}
