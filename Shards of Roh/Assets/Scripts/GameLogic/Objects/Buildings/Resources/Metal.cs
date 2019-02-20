using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class Metal : Building {

	public Metal (Player _owner) {
		buildingSetup ();
		name = "Metal";
		race = "Nature";
		owner = _owner;
		health = 1000;
		cost = new Resource (10000, 10000, 10000, 10000);

		isResource = true;
		resourceType = ResourceType.Metal;
	}
}
