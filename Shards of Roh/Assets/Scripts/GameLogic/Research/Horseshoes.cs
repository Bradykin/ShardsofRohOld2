using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class Horseshoes : Research {

	public Horseshoes (Player _owner) {
		owner = _owner;
		name = "Horrseshoes";
		cost = new Resource (100, 50, 50, 0);
		queueTime = 5;
		neededResearch = new List<string> ();
		effects = new List<ResearchEffect> ();

		effects.Add (new ResearchEffect ("WorkerCoats01", "Unit", ResearchPurpose.Combat, "unitType", "Cavalry", "health", "+", 20.0f));
		effects.Add (new ResearchEffect ("WorkerCoats02", "Unit", ResearchPurpose.Combat, "unitType", "Cavalry", "curHealth", "+", 20.0f));
		effects.Add (new ResearchEffect ("WorkerCoats03", "Unit", ResearchPurpose.Combat, "unitType", "Cavalry", "moveSpeed", "+", 1.0f));

		//neededResearch.Add ("Age2");
	}
}
