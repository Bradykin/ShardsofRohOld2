using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResearchFactory {

	public static Age2 createAge2 (Player _owner) {
		Age2 research = new Age2 (_owner);
		return research;
	}

	public static AnimalTracking createAnimalTracking (Player _owner) {
		AnimalTracking research = new AnimalTracking (_owner);
		return research;
	}

	public static Forestry createForestry (Player _owner) {
		Forestry research = new Forestry (_owner);
		return research;
	}

	public static Research createResearchByName (string _name, Player _owner) {
		if (_name == "Age2") {
			return createAge2 (_owner);
		} else if (_name == "AnimalTracking") {
			return createAnimalTracking (_owner);
		} else if (_name == "Forestry") {
			return createForestry (_owner);
		}


		GameManager.print ("Fail to find name - ResearchFactory");
		return createAge2 (_owner);
	}
}
