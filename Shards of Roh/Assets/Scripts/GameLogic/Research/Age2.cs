﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class Age2 : Research {

	public Age2 (Player _owner) {
		owner = _owner;
		name = "Age2";
		cost = new Resource (50, 50, 50, 50);
		queueTime = 5;
		neededResearch = new List<string> ();
		effects = new List<ResearchEffect> ();
	}
}
