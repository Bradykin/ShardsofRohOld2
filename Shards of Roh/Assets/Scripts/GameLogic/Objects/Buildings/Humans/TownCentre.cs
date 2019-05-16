using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownCentre : Building {

	public TownCentre (Player _owner) {
		buildingSetup ();
		name = "TownCentre";
		race = "Humans";
		owner = _owner;
		health = 1000;
		cost = new Resource (0, 250, 100, 0);
		populationValue = 10;

		abilities.Add (new AddToUnitQueue ("Worker", this));
		abilities.Add (new AddToUnitQueue ("Scout", this));
		abilities.Add (new AddToResearchQueue ("WorkerCoats", this));
		abilities.Add (new AddToResearchQueue ("Age2", this));
		abilities.Add (new AddToResearchQueue ("Age3", this));
		abilities.Add (new AddToResearchQueue ("Age4", this));
	}
}
