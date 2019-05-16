using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tavern : Building {

	public Tavern (Player _owner) {
		buildingSetup ();
		name = "Tavern";
		race = "Humans";
		owner = _owner;
		health = 1000;
		cost = new Resource (0, 100, 100, 0);

		neededResearch.Add ("Age2");
	}
}
