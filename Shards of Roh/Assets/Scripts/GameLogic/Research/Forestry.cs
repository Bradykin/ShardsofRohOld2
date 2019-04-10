using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class Forestry : Research {

	public Forestry (Player _owner) {
		owner = _owner;
		name = "Forestry";
		cost = new Resource (0, 50, 0, 0);
		queueTime = 5;
		neededResearch = new List<string> ();
		effects = new List<ResearchEffect> ();

		effects.Add (new ResearchEffect ("Forestry01", "Unit", ResearchPurpose.Economic, "name", "Worker", "foodForageGatherRate", "+", 0.8f));
		effects.Add (new ResearchEffect ("Forestry02", "Unit", ResearchPurpose.Economic, "name", "Worker", "woodGatherRate", "+", 0.8f));
	}
}
