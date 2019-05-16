using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class ImprovedSwordsmen : Research {

	public ImprovedSwordsmen (Player _owner) {
		owner = _owner;
		name = "ImprovedSwordsmen";
		cost = new Resource (200, 200, 200, 100);
		queueTime = 5;
		neededResearch = new List<string> ();
		effects = new List<ResearchEffect> ();

		effects.Add (new ResearchEffect ("ImprovedSwordsmen01", "Unit", ResearchPurpose.Combat, "name", "Swordsman", "attack", "+", 1.0f));
		effects.Add (new ResearchEffect ("ImprovedSwordsmen02", "Unit", ResearchPurpose.Combat, "name", "Swordsman", "health", "+", 20.0f));
		effects.Add (new ResearchEffect ("ImprovedSwordsmen02", "Unit", ResearchPurpose.Combat, "name", "Swordsman", "curHealth", "+", 20.0f));
	}
}
