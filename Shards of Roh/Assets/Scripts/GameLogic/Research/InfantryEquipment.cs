using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class InfantryEquipment : Research {

	public InfantryEquipment (Player _owner) {
		owner = _owner;
		name = "InfantryEquipment";
		cost = new Resource (150, 100, 0, 50);
		queueTime = 5;
		neededResearch = new List<string> ();
		effects = new List<ResearchEffect> ();

		effects.Add (new ResearchEffect ("InfantryEquipment01", "Unit", ResearchPurpose.Economic, "unitType", "Infantry", "health", "+", 20.0f));
		effects.Add (new ResearchEffect ("InfantryEquipment02", "Unit", ResearchPurpose.Economic, "unitType", "Infantry", "curHealth", "+", 20.0f));
		effects.Add (new ResearchEffect ("InfantryEquipment03", "Unit", ResearchPurpose.Economic, "unitType", "Infantry", "moveSpeed", "+", 1.0f));
	}
}
