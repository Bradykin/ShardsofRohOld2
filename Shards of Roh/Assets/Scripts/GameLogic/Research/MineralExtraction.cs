using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class MineralExtraction : Research {

	public MineralExtraction (Player _owner) {
		owner = _owner;
		name = "MineralExtraction";
		cost = new Resource (0, 50, 100, 0);
		queueTime = 5;
		neededResearch = new List<string> ();
		effects = new List<ResearchEffect> ();

		effects.Add (new ResearchEffect ("MineralExtraction01", "Unit", ResearchPurpose.Economic, "name", "Worker", "goldGatherRate", "+", 0.8f));
		effects.Add (new ResearchEffect ("MineralExtraction02", "Unit", ResearchPurpose.Economic, "name", "Worker", "metalGatherRate", "+", 0.8f));
	}
}
