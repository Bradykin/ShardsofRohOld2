using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class ImprovedArchers : Research {

	public ImprovedArchers (Player _owner) {
		owner = _owner;
		name = "ImprovedArchers";
		cost = new Resource (200, 200, 200, 100);
		queueTime = 5;
		neededResearch = new List<string> ();
		effects = new List<ResearchEffect> ();

		effects.Add (new ResearchEffect ("ImprovedArchers01", "Unit", ResearchPurpose.Combat, "name", "Archer", "attack", "+", 1.0f));
		effects.Add (new ResearchEffect ("ImprovedArchers02", "Unit", ResearchPurpose.Combat, "name", "Archer", "health", "+", 20.0f));
		effects.Add (new ResearchEffect ("ImprovedArchers02", "Unit", ResearchPurpose.Combat, "name", "Archer", "curHealth", "+", 20.0f));
	}
}
