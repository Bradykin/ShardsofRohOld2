using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class ImprovedShields : Research {

	public ImprovedShields (Player _owner) {
		owner = _owner;
		name = "ImprovedShields";
		cost = new Resource (0, 300, 100, 50);
		queueTime = 5;
		neededResearch = new List<string> ();
		effects = new List<ResearchEffect> ();

		effects.Add (new ResearchEffect ("ImprovedShields01", "Unit", ResearchPurpose.Combat, "unitType", "Infantry", "armourSlashing", "+", 10.0f));
		effects.Add (new ResearchEffect ("ImprovedShields02", "Unit", ResearchPurpose.Combat, "unitType", "Infantry", "armourPiercing", "+", 10.0f));
		effects.Add (new ResearchEffect ("ImprovedShields02", "Unit", ResearchPurpose.Combat, "unitType", "Infantry", "armourRanged", "+", 10.0f));
	}
}
