using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class ImprovedSpearmen : Research {

	public ImprovedSpearmen (Player _owner) {
		owner = _owner;
		name = "ImprovedSpearmen";
		cost = new Resource (200, 200, 200, 100);
		queueTime = 5;
		neededResearch = new List<string> ();
		effects = new List<ResearchEffect> ();

		effects.Add (new ResearchEffect ("ImprovedSpearmen01", "Unit", ResearchPurpose.Combat, "name", "Spearman", "attack", "+", 1.0f));
		effects.Add (new ResearchEffect ("ImprovedSpearmen02", "Unit", ResearchPurpose.Combat, "name", "Spearman", "health", "+", 20.0f));
		effects.Add (new ResearchEffect ("ImprovedSpearmen02", "Unit", ResearchPurpose.Combat, "name", "Spearman", "curHealth", "+", 20.0f));
	}
}
