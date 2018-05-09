using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fort : Building {

	public Fort (Player _owner) {
		buildingSetup ();
		name = "Fort";
		race = "Humans";
		owner = _owner;
		health = 1000;
		cost = new Resource (0, 0, 50);
	}
}
