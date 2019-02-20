using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cathedral : Building {

	public Cathedral (Player _owner) {
		buildingSetup ();
		name = "Cathedral";
		race = "Humans";
		owner = _owner;
		health = 1000;
		cost = new Resource (0, 0, 50, 0);

		abilities.Add (new AddToUnitQueue ("Mage", this));
		abilities.Add (new AddToUnitQueue ("MageCavalry", this));
		abilities.Add (new AddToUnitQueue ("Priest", this));
		abilities.Add (new AddToUnitQueue ("PriestCavalry", this));
	}
}
