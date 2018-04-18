using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : Building {

	public Gold (Player _owner) {
		name = "Gold";
		race = "Nature";
		owner = _owner;
		health = 1000;
		cost = new Resource (10000, 10000, 10000);

		isResource = true;
	}
}
