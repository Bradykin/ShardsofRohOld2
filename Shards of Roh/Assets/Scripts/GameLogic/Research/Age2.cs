using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class Age2 : Research {

	public Age2 (Player _owner) {
		owner = _owner;
		name = "Age2";
		cost = new Resource (50, 50, 50, 50);
		queueTime = 5;
		neededResearch = new List<string> ();
		effects = new List<ResearchEffect> ();

		neededResearch.Add ("AnimalTracking");

		effects.Add (new ResearchEffect ("Age201", "Unit", ResearchPurpose.Economic, "name", "Worker", "foodForageGatherRate", "+", 0.2f));
		effects.Add (new ResearchEffect ("Age202", "Unit", ResearchPurpose.Economic, "name", "Worker", "woodGatherRate", "+", 0.2f));
	}
}
