using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windmill : Building {

	public Windmill (Player _owner) {
		buildingSetup ();
		name = "Windmill";
		race = "Humans";
		owner = _owner;
		health = 1000;
		cost = new Resource (0, 0, 50);
	}
}
