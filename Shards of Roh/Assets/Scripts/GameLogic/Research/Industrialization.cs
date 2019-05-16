using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class Industrialization : Research {

	public Industrialization (Player _owner) {
		owner = _owner;
		name = "Industrialization";
		cost = new Resource (1000, 1000, 1000, 1000);
		queueTime = 5;
		neededResearch = new List<string> ();
		effects = new List<ResearchEffect> ();

		effects.Add (new ResearchEffect ("Industrialization01", "Unit", ResearchPurpose.Economic, "name", "Worker", "foodForageGatherRate", "+", 0.5f));
		effects.Add (new ResearchEffect ("Industrialization02", "Unit", ResearchPurpose.Economic, "name", "Worker", "foodAnimalGatherRate", "+", 0.5f));
		effects.Add (new ResearchEffect ("Industrialization03", "Unit", ResearchPurpose.Economic, "name", "Worker", "woodGatherRate", "+", 0.5f));
		effects.Add (new ResearchEffect ("Industrialization04", "Unit", ResearchPurpose.Economic, "name", "Worker", "goldGatherRate", "+", 0.5f));
		effects.Add (new ResearchEffect ("Industrialization05", "Unit", ResearchPurpose.Economic, "name", "Worker", "metalGatherRate", "+", 0.5f));

		neededResearch.Add ("Age3");
	}
}
