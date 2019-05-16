using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barracks : Building {

	public Barracks (Player _owner) {
		buildingSetup ();
		name = "Barracks";
		race = "Humans";
		owner = _owner;
		health = 200;
		cost = new Resource (0, 200, 0, 0);

		abilities.Add (new AddToUnitQueue ("Swordsman", this));
		abilities.Add (new AddToUnitQueue ("Spearman", this));
		abilities.Add (new AddToUnitQueue ("Axeman", this));
		abilities.Add (new AddToUnitQueue ("HeavyInfantry", this));
		abilities.Add (new AddToUnitQueue ("Archer", this));
		abilities.Add (new AddToResearchQueue ("ImprovedSwordsmen", this));
		abilities.Add (new AddToResearchQueue ("ImprovedSpearmen", this));
		abilities.Add (new AddToResearchQueue ("ImprovedAxemen", this));
		abilities.Add (new AddToResearchQueue ("ImprovedArchers", this));

		neededResearch.Add ("Age2");
	}
}
