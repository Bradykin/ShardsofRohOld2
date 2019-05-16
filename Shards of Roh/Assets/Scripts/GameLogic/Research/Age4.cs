using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class Age4 : Research {

	public Age4 (Player _owner) {
		owner = _owner;
		name = "Age4";
		cost = new Resource (1000, 1000, 1000, 1000);
		queueTime = 5;
		neededResearch = new List<string> ();
		effects = new List<ResearchEffect> ();

		neededResearch.Add ("Age3");
	}
}
