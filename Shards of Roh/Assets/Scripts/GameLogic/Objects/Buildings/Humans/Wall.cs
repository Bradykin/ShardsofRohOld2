using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Building {

	public Wall  (Player _owner) {
		buildingSetup ();
		name = "Wall";
		race = "Humans";
		owner = _owner;
		health = 1000;
		cost = new Resource (0, 5, 0, 0);

		neededResearch.Add ("Age2");
	}
}

