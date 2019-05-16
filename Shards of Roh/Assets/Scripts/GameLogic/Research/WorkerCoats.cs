using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class WorkerCoats : Research {

	public WorkerCoats (Player _owner) {
		owner = _owner;
		name = "WorkerCoats";
		cost = new Resource (100, 50, 50, 0);
		queueTime = 5;
		neededResearch = new List<string> ();
		effects = new List<ResearchEffect> ();

		effects.Add (new ResearchEffect ("WorkerCoats01", "Unit", ResearchPurpose.Combat, "unitType", "Villager", "health", "+", 25.0f));
		effects.Add (new ResearchEffect ("WorkerCoats02", "Unit", ResearchPurpose.Combat, "unitType", "Villager", "curHealth", "+", 25.0f));
		effects.Add (new ResearchEffect ("WorkerCoats03", "Unit", ResearchPurpose.Combat, "unitType", "Villager", "moveSpeed", "+", 1.0f));
	}
}
