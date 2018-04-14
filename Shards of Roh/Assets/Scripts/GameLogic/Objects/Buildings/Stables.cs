using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stables : Building {

	public Stables (Player _owner) {
		name = "Stables";
		race = "Humans";
		owner = _owner;
		health = 1000;

		abilities.Add (new CreateUnit ("LightCavalry", this));
		abilities.Add (new CreateUnit ("SpearCavalry", this));
	}
}
