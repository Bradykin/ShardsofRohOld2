using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formation {

	private Vector3 position;
	private string unitType;

	//Set value of position/unitType
	public Formation (Vector3 _position, string _unitType) {
		position = _position;
		unitType = _unitType;
	}

	//Get position value
	public Vector3 getPosition () {
		return position;
	}

	//Get unitType value
	public string getUnitType () {
		return unitType;
	}
}
