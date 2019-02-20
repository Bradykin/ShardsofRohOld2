using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blacksmith : Building {

	public Blacksmith (Player _owner) {
		buildingSetup ();
		name = "Blacksmith";
		race = "Humans";
		owner = _owner;
		health = 1000;
		cost = new Resource (0, 0, 50, 0);

		abilities.Add (new AddToUnitQueue ("Catapult", this));

		neededResearch.Add (ResearchFactory.createResearchByName ("Age2", _owner));
	}
}
