using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {

	private string name;

	private List<UnitContainer> units = new List<UnitContainer> ();
	private List<BuildingContainer> buildings = new List<BuildingContainer> ();
	private List<UnitContainer> curUnitTarget = new List<UnitContainer> ();
	private List<BuildingContainer> curBuildingTarget = new List<BuildingContainer> ();
	private Resource resource = new Resource(0,0,0);

	public Player (string _name) {
		name = _name;
	}

	public void addCurUnitTarget (UnitContainer _curUnitTarget) {
		if (!curUnitTarget.Contains (_curUnitTarget)) {
			curUnitTarget.Add (_curUnitTarget);
			toggleSelectionCircle (true, _curUnitTarget);
		}
	}

	public void remCurUnitTarget (UnitContainer _curUnitTarget) {
		if (curUnitTarget.Contains (_curUnitTarget)) {
			curUnitTarget.Remove (_curUnitTarget);
			toggleSelectionCircle (false, _curUnitTarget);
		}
	}

	public void setCurUnitTarget (UnitContainer _curUnitTarget) {
		toggleSelectionCircles (false);
		curUnitTarget.Clear ();
		curUnitTarget.Add (_curUnitTarget);
		toggleSelectionCircle (true, _curUnitTarget);
	}

	public void setCurUnitTarget (List<UnitContainer> _curUnitTarget) {
		toggleSelectionCircles (false);
		curUnitTarget = _curUnitTarget;
		toggleSelectionCircles (true);
	}

	public List<UnitContainer> getCurUnitTarget () {
		return curUnitTarget;
	}

	public UnitContainer getCurUnitTarget (int _index) {
		if (curUnitTarget.Count > _index) {
			return curUnitTarget [_index];
		} else {
			return null;
		}
	}

	public void addCurBuildingTarget (BuildingContainer _curBuildingTarget) {
		if (!curBuildingTarget.Contains (_curBuildingTarget)) {
			curBuildingTarget.Add (_curBuildingTarget);
			toggleSelectionBox (true, _curBuildingTarget);
		}
	}

	public void remCurBuildingTarget (BuildingContainer _curBuildingTarget) {
		if (curBuildingTarget.Contains (_curBuildingTarget)) {
			curBuildingTarget.Remove (_curBuildingTarget);
			toggleSelectionBox (false, _curBuildingTarget);
		}
	}

	public void setCurBuildingTarget (BuildingContainer _curBuildingTarget) {
		toggleSelectionBoxes (false);
		curBuildingTarget.Clear ();
		curBuildingTarget.Add (_curBuildingTarget);
		toggleSelectionBox (true, _curBuildingTarget);
	}

	public void setCurBuildingTarget (List<BuildingContainer> _curBuildingTarget) {
		toggleSelectionBoxes (false);
		curBuildingTarget = _curBuildingTarget;
		toggleSelectionBoxes (true);
	}

	public List<BuildingContainer> getCurBuildingTarget () {
		return curBuildingTarget;
	}

	public BuildingContainer getCurBuildingTarget (int _index) {
		if (curBuildingTarget.Count > _index) {
			return curBuildingTarget [_index];
		} else {
			return null;
		}
	}

	public void sortCurUnitTarget (Vector3 _destination, List <Formation> _formationPositions) {
		List <UnitContainer> newList = new List <UnitContainer> ();

		for (int x = 0; x < _formationPositions.Count; x++) {
			float distance = -1;
			UnitContainer lowest = getCurUnitTarget (0);

			foreach (var r in curUnitTarget) {
				if (_formationPositions.Count > 0) {
					if (distance == -1 || Vector3.Distance (r.getUnit ().getCurLoc (), _formationPositions [x].getPosition ()) < distance) {
						if (r.getUnit ().getName () == _formationPositions [x].getUnitType () || _formationPositions [x].getUnitType () == "") {
							lowest = r;
							distance = Vector3.Distance (r.getUnit ().getCurLoc (), _formationPositions [x].getPosition ());
						} else {
							GameManager.print ("Wrong unit");
						}
					}
				} else {
					GameManager.print ("Empty FormationPosition - Player");
				}
			}
			newList.Add (lowest);
			curUnitTarget.Remove (lowest);
		}

		bool findBetter = true;
		while (findBetter == true) {
			findBetter = false;
			for (int x = 0; x < _formationPositions.Count; x++) {
				for (int y = 0; y < _formationPositions.Count; y++) {
					if (x != y && _formationPositions [x].getUnitType () == _formationPositions [y].getUnitType ()) {
						UnityEngine.AI.NavMeshPath pathX = newList [x].GetComponent<UnityEngine.AI.NavMeshAgent> ().path;
						UnityEngine.AI.NavMeshPath pathY = newList [y].GetComponent<UnityEngine.AI.NavMeshAgent> ().path;
						float curDistanceX = 0;
						float curDistanceY = 0;
						float newDistanceX = 0;
						float newDistanceY = 0;
						if (pathX.corners.Length >= 2 && pathY.corners.Length >= 2) {
							curDistanceX = Vector3.Distance (pathX.corners [pathX.corners.Length - 2], _formationPositions [x].getPosition ());
							curDistanceY = Vector3.Distance (pathY.corners [pathY.corners.Length - 2], _formationPositions [y].getPosition ());

							newDistanceX = Vector3.Distance (pathX.corners [pathX.corners.Length - 2], _formationPositions [y].getPosition ());
							newDistanceY = Vector3.Distance (pathY.corners [pathY.corners.Length - 2], _formationPositions [x].getPosition ());
						} else {
							curDistanceX = Vector3.Distance (newList [x].getUnit ().getCurLoc (), _formationPositions [x].getPosition ());
							curDistanceY = Vector3.Distance (newList [y].getUnit ().getCurLoc (), _formationPositions [y].getPosition ());

							newDistanceX = Vector3.Distance (newList [x].getUnit ().getCurLoc (), _formationPositions [y].getPosition ());
							newDistanceY = Vector3.Distance (newList [y].getUnit ().getCurLoc (), _formationPositions [x].getPosition ());
						}
						if ((Mathf.Max (curDistanceX, curDistanceY) - Mathf.Min (curDistanceX, curDistanceY)) > (Mathf.Max (newDistanceX, newDistanceY) - Mathf.Min (newDistanceX, newDistanceY))) {
							if ((curDistanceX + curDistanceY) > (newDistanceX + newDistanceY)) {
								findBetter = true;
								//GameManager.print ((Mathf.Max (curDistanceX, curDistanceY) - Mathf.Min (curDistanceX, curDistanceY)) + " / " + (Mathf.Max (newDistanceX, newDistanceY) - Mathf.Min (newDistanceX, newDistanceY)));
								UnitContainer temp = newList [x];
								newList [x] = newList [y];
								newList [y] = temp;
							} else {
								//GameManager.print ("Miss2: " + (curDistanceX + curDistanceY) + " / " + (newDistanceX + newDistanceY));
							}
						} else {
							//GameManager.print ("Miss: " + (Mathf.Max (curDistanceX, curDistanceY) - Mathf.Min (curDistanceX, curDistanceY)) + " / " + (Mathf.Max (newDistanceX, newDistanceY) - Mathf.Min (newDistanceX, newDistanceY)));
						}
					}
				}
			}
		}

		setCurUnitTarget (newList);

		//I don't know if this is actually doing anything. Theoretically it should be helping units find attack angles
		/*for (int i = 0; i < curUnitTarget.Count; i++) {
			curUnitTarget [i].gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().avoidancePriority = 50 - curUnitTarget.Count + i;
		}*/
	}

	public void toggleSelectionCircle (bool _toggle, UnitContainer _unit) {
		if (_unit.gameObject.transform.GetChild (0).name == "TargetRing") {
			_unit.gameObject.transform.GetChild (0).gameObject.SetActive (_toggle);
		}
	}

	public void toggleSelectionCircles (bool _toggle) {
		foreach (var r in curUnitTarget) {
			if (r.gameObject.transform.GetChild (0).name == "TargetRing") {
				r.gameObject.transform.GetChild (0).gameObject.SetActive (_toggle);
			}
		}
	}

	public void toggleSelectionBox (bool _toggle, BuildingContainer _Building) {
		if (_Building.gameObject.transform.GetChild (0).name == "TargetRing") {
			_Building.gameObject.transform.GetChild (0).gameObject.SetActive (_toggle);
		}
	}

	public void toggleSelectionBoxes (bool _toggle) {
		foreach (var r in curBuildingTarget) {
			if (r.gameObject.transform.GetChild (0).name == "TargetRing") {
				r.gameObject.transform.GetChild (0).gameObject.SetActive (_toggle);
			}
		}
	}

	public void addUnitToPlayer (UnitContainer _unitContainer) {
		units.Add (_unitContainer);
	}

	public void addBuildingToPlayer (BuildingContainer _buildingContainer) {
		buildings.Add (_buildingContainer);
	}

	public List<UnitContainer> getUnits () {
		return units;
	}

	public List<BuildingContainer> getBuildings () {
		return buildings;
	}

	public void setName (string _name) {
		name = _name;
	}

	public string getName () {
		return name;
	}

	public Resource getResource () {
		return resource;
	}
}