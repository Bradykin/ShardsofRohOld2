using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Age3 : Research {

	public Age3 (Player _owner) {
		owner = _owner;
		name = "Age3";
		cost = new Resource (50, 50, 50, 50);
		queueTime = 5;
		neededResearch = new List<Research> ();
	}

	public override void applyOnFinish () {

	}

	public override void applyToUnit (Unit _unit) {

	}

	public override void applyToBuilding (Building _building) {

	}
}
