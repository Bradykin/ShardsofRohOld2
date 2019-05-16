using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : Building {

	public House (Player _owner) {
		buildingSetup ();
		name = "House";
		race = "Humans";
		owner = _owner;
		health = 200;
		cost = new Resource (0, 50, 0, 0);
		populationValue = 10;
	}
}

