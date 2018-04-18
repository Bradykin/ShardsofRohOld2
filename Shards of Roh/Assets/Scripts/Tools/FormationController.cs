using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationController : MonoBehaviour {

	//Variable that denotes which formation to use
	public static int formationMode = 0;

	//Choose a formation and return it
	public static List<Formation> findFormationPositions (bool _attacking, List <UnitContainer> _units, Vector3 _destination, Vector3 _unitBackVec, Vector3 _unitSideVec) {

		if (_attacking == true) {
			return attackFormation (_units, _destination, _unitBackVec, _unitSideVec);
		} else {
			if (formationMode == 0) {
				return basicFormation (_units, _destination, _unitBackVec, _unitSideVec);
			} else if (formationMode == 1) {
				return arrowheadFormation (_units, _destination, _unitBackVec, _unitSideVec);
			} else if (formationMode == 2) {
				return doubleBoxFormation (_units, _destination, _unitBackVec, _unitSideVec);
			}
		}

		print ("FormationMode outside of range");
		return basicFormation (_units, _destination, _unitBackVec, _unitSideVec);
	}

	//Return a rectangle formation, rows of 5
	public static List<Formation> basicFormation (List <UnitContainer> _units, Vector3 _destination, Vector3 _unitBackVec, Vector3 _unitSideVec) {
		List <Formation> positions = new List <Formation> ();
		Vector3 curPoint = _destination;
		List <string> unitTypes = new List <string> ();

		for (int i = 0; i < _units.Count; i++) {
			unitTypes.Add (_units [i].getUnit ().getName ());
		}
		unitTypes.Sort ();

		for (int i = 0; i < _units.Count; i++) {
			positions.Add (new Formation (curPoint, unitTypes [i]));
			if (i % 5 == 0) {
				curPoint = curPoint + _unitSideVec;
			} else if (i % 5 == 1) {
				curPoint = curPoint - (_unitSideVec * 2);
			} else if (i % 5 == 2) {
				curPoint = curPoint + (_unitSideVec * 3);
			} else if (i % 5 == 3) {
				curPoint = curPoint - (_unitSideVec * 4);
			} else if (i % 5 == 4) {
				curPoint = curPoint + (_unitSideVec * 2) + _unitBackVec;
			} 
		}

		return positions;
	}

	//Return an arrowhead formation, grows by 2 on each row
	public static List<Formation> arrowheadFormation (List <UnitContainer> _units, Vector3 _destination, Vector3 _unitBackVec, Vector3 _unitSideVec) {
		List <Formation> positions = new List <Formation> ();
		Vector3 curPoint = _destination;
		List <string> unitTypes = new List <string> ();

		for (int i = 0; i < _units.Count; i++) {
			unitTypes.Add (_units [i].getUnit ().getName ());
		}
		unitTypes.Sort ();

		int q = 0;
		int r = 0;
		int l = 1;
		while (q < _units.Count) {
			Vector3 tempPoint = curPoint - (r * _unitSideVec);
			for (int i = 0; i < l; i++) {
				if (q < _units.Count) {
					positions.Add (new Formation (tempPoint, unitTypes [i]));
					tempPoint = tempPoint + _unitSideVec;
					q++;
				}
			}
			r++;
			l = l + 2;
			curPoint = curPoint + _unitBackVec;
		}

		return positions;
	}

	//Return a doubleBox formation, two groups of rows of 3
	public static List<Formation> doubleBoxFormation (List <UnitContainer> _units, Vector3 _destination, Vector3 _unitBackVec, Vector3 _unitSideVec) {
		List <Formation> positions = new List <Formation> ();
		Vector3 curPoint = _destination;
		List <string> unitTypes = new List <string> ();

		for (int i = 0; i < _units.Count; i++) {
			unitTypes.Add (_units [i].getUnit ().getName ());
		}
		unitTypes.Sort ();
		Vector3 tempPoint1 = curPoint - (_unitSideVec * 3);
		Vector3 tempPoint2 = curPoint + (_unitSideVec * 3);
	

		for (int i = 0; i < _units.Count; i++) {
			if (i % 6 == 0) {
				positions.Add (new Formation (tempPoint1, unitTypes [i]));
			} else if (i % 6 == 1) {
				positions.Add (new Formation (tempPoint1 + _unitSideVec, unitTypes [i]));
			} else if (i % 6 == 2) {
				positions.Add (new Formation (tempPoint1 - _unitSideVec, unitTypes [i]));
			} else if (i % 6 == 3) {
				positions.Add (new Formation (tempPoint2, unitTypes [i]));
			} else if (i % 6 == 4) {
				positions.Add (new Formation (tempPoint2 - _unitSideVec, unitTypes [i]));
			} else if (i % 6 == 5) {
				positions.Add (new Formation (tempPoint2 + _unitSideVec, unitTypes [i]));
				tempPoint1 = tempPoint1 + _unitBackVec;
				tempPoint2 = tempPoint2 + _unitBackVec;
			} 
		}

		return positions;
	}

	//Return an attack formation, TBD
	public static List<Formation> attackFormation (List <UnitContainer> _units, Vector3 _destination, Vector3 _unitBackVec, Vector3 _unitSideVec) {
		List <Formation> positions = new List <Formation> ();
		Vector3 curPoint = _destination;
		List <string> unitTypes = new List <string> ();

		for (int i = 0; i < _units.Count; i++) {
			unitTypes.Add (_units [i].getUnit ().getName ());
		}
		unitTypes.Sort ();

		for (int i = 0; i < _units.Count; i++) {
			positions.Add (new Formation (curPoint, unitTypes [i]));
		}

		return positions;
	}
}
