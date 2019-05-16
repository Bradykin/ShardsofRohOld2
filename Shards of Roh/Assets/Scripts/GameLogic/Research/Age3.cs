using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class Age3 : Research {

	public Age3 (Player _owner) {
		owner = _owner;
		name = "Age3";
		cost = new Resource (1000, 1000, 1000, 1000);
		queueTime = 5;
		neededResearch = new List<string> ();
		effects = new List<ResearchEffect> ();

		neededResearch.Add ("Age2");
	}
}
