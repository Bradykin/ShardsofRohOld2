using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class AnimalTracking : Research {

	public AnimalTracking (Player _owner) {
		owner = _owner;
		name = "AnimalTracking";
		cost = new Resource (100, 0, 0, 0);
		queueTime = 5;
		neededResearch = new List<Research> ();
		effects = new List<ResearchEffect> ();

		effects.Add (new ResearchEffect ("AnimalTracking01", "Unit", ResearchPurpose.Combat, "name", "Worker", "attack", "+", 20.0f));
	}
}
