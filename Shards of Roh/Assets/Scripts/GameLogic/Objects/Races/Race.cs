using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public abstract class Race {

	//Variables that must be declared in subclass

	//Variables that will default if not declared

	//Variables that adjust during gameplay

	//Lists populated by the subclass
	public List<Unit> unitTypes { get; protected set; }
	public List<Building> buildingTypes { get; protected set; }
	public List<Research> researchTypes { get; protected set; }

	//Lists of the objects suitable for various scenarios
	//public List<Purchaseable> 

	public void setup () {
		unitTypes = new List<Unit> ();
		buildingTypes = new List<Building> ();
		researchTypes = new List<Research> ();
	}
}
