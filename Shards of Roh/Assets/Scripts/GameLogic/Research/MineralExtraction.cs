using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class MineralExtraction : Research {

	public MineralExtraction (Player _owner) {
		owner = _owner;
		name = "MineralExtraction";
		cost = new Resource (0, 50, 100, 0);
		queueTime = 5;
		neededResearch = new List<string> ();
	}
}
