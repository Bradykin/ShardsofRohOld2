using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class ImprovedAxemen : Research {

	public ImprovedAxemen (Player _owner) {
		owner = _owner;
		name = "ImprovedAxemen";
		cost = new Resource (200, 200, 200, 100);
		queueTime = 5;
		neededResearch = new List<string> ();
		effects = new List<ResearchEffect> ();

		effects.Add (new ResearchEffect ("ImprovedAxemen01", "Unit", ResearchPurpose.Combat, "name", "Axeman", "attack", "+", 1.0f));
		effects.Add (new ResearchEffect ("ImprovedAxemen02", "Unit", ResearchPurpose.Combat, "name", "Axeman", "health", "+", 20.0f));
		effects.Add (new ResearchEffect ("ImprovedAxemen02", "Unit", ResearchPurpose.Combat, "name", "Axeman", "curHealth", "+", 20.0f));
	}
}
