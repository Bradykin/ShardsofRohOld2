using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIQueue {

	public string type { get; protected set; }
	//public Purchaseable purchase { get; protected set; }
	public ObjectBase objectValue { get; protected set; }
	public Research research { get; protected set; }

	public AIQueue (string _type, ObjectBase _objectBase = null, Research _research = null) {
		type = _type;

		if (type == "Unit" || type == "Building") {
			objectValue = _objectBase;
		} else if (type == "Research") {
			research = _research;
		}
	}

	public Resource getCost () {
		if (type == "Unit" || type == "Building") {
			return objectValue.cost;
		} else if (type == "Research") {
			return research.cost;
		}

		return null;
	}
}