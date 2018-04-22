using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchFactory : MonoBehaviour {

	public static Age2 createAge2 (Player _owner) {
		Age2 research = new Age2 (_owner);
		return research;
	}

	public static Research createResearchByName (string _name, Player _owner) {
		if (_name == "Age2") {
			return createAge2 (_owner);
		}


		print ("Fail to find name - ResearchFactory");
		return createAge2 (_owner);
	}
}
