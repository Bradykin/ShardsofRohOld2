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

		abilities.Add (new AddToUnitQueue ("Axeman", this));
		abilities.Add (new AddToUnitQueue ("Spearman", this));
		abilities.Add (new AddToUnitQueue ("Swordsman", this));
		abilities.Add (new AddToUnitQueue ("HeavyInfantry", this));
		abilities.Add (new AddToUnitQueue ("Archer", this));
	}
}
