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
		health = 2000;
		cost = new Resource (0, 0, 0, 2000);

		isResource = true;
		resourceType = ResourceType.Metal;
	}
}
