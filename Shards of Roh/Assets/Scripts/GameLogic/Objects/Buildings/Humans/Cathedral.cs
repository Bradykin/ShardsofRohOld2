using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cathedral : Building {

	public Cathedral (Player _owner) {
		name = "Cathedral";
		race = "Humans";
		owner = _owner;
		health = 1000;
		cost = new Resource (0, 0, 50);

		abilities.Add (new QueueUnit ("Mage", this));
		abilities.Add (new QueueUnit ("MageCavalry", this));
		abilities.Add (new QueueUnit ("Priest", this));
		abilities.Add (new QueueUnit ("PriestCavalry", this));
	}
}
