using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formation {

	private Vector3 position;
	private string unitType;

	public Formation (Vector3 _position, string _unitType) {
		position = _position;
		unitType = _unitType;
	}

	public Vector3 getPosition () {
		return position;
	}

	public string getUnitType () {
		return unitType;
	}
}
