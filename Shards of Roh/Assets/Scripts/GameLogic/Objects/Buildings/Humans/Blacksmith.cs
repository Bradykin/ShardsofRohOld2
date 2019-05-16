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
		cost = new Resource (0, 150, 0, 50);

		abilities.Add (new AddToUnitQueue ("Catapult", this));
		abilities.Add (new AddToResearchQueue ("Industrialization", this));
		abilities.Add (new AddToResearchQueue ("InfantryEquipment", this));
		abilities.Add (new AddToResearchQueue ("ImprovedShields", this));

		neededResearch.Add ("Age2");
	}
}
