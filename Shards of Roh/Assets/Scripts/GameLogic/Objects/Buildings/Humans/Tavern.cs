using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tavern : Building {

	public Tavern (Player _owner) {
		name = "Tavern";
		race = "Humans";
		owner = _owner;
		health = 1000;
		cost = new Resource (0, 0, 50);
	}
}
