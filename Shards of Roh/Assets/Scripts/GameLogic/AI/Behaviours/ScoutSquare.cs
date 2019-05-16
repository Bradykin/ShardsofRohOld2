using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class ScoutSquare : Behaviours {

	public ScoutingGridSquare square;
	bool firstMove;
	public float timer = 0.0f;

	public ScoutSquare (UnitContainer _unitInfo) {
		name = "ScoutSquare";
		active = true;
		behaviourType = "Scout";
		unitInfo = _unitInfo;
		unitInfo.removeBehaviourByType (behaviourType, this);
	}

	public void addSquare (ScoutingGridSquare _square) {
		square = _square;
		firstMove = true;
		//GameManager.print ("Scout square: " + square.squareXLeft + " - " + square.squareXRight + ", " + square.squareZLeft + " - " + square.squareZRight);
	}

	public override void enact () {
		if (active == true) {
			timer += Time.deltaTime;

			if (unitInfo.unit.isMoving == false || firstMove == true) {
				firstMove = false;
				unitInfo.moveToLocation (false, new Vector3 (Random.Range (square.squareXLeft, square.squareXRight), 0, Random.Range (square.squareZLeft, square.squareZRight)));
			}
		}
	}
}
