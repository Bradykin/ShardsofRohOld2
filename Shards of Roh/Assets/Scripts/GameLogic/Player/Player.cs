using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {

	private string name;
	private int population;
	private int maxPopulation;

	private List<UnitContainer> units = new List<UnitContainer> ();
	private List<BuildingContainer> buildings = new List<BuildingContainer> ();
	private List<UnitContainer> curUnitTarget = new List<UnitContainer> ();
	private List<BuildingContainer> curBuildingTarget = new List<BuildingContainer> ();
	private List<Research> researchList = new List<Research> ();
	private Resource resource = new Resource (0, 0, 0);

	public Player (string _name) {
		name = _name;
		updatePopulation ();
		updatemaxPopulation ();
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

	public void processFormationMovement (Vector3 targetLoc) {
		foreach (var r in GameManager.player.getPlayer ().getCurUnitTarget ()) {
			r.getUnit ().dropAttackTarget ();
		}

		//Calculate angle vectors for formations
		Vector3 unitVec = new Vector3 (0, 0, 0);
		foreach (var r in GameManager.player.getPlayer ().getCurUnitTarget ()) {
			//This works well, but I worry about performance issues. Think of a better way!
			UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath ();
			UnityEngine.AI.NavMesh.CalculatePath (r.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().transform.position, targetLoc, UnityEngine.AI.NavMesh.AllAreas, path);
			r.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().path = path;
			if (path.corners.Length >= 2) {
				unitVec = unitVec + path.corners [path.corners.Length - 2];
			} else {
				unitVec = unitVec + r.getUnit ().getCurLoc ();
			}
		}
		unitVec = unitVec / GameManager.player.getPlayer ().getCurUnitTarget ().Count;
		unitVec = (unitVec - targetLoc).normalized * 4;
		Vector3 perpVec = Vector3.Cross (unitVec, new Vector3 (0, 1, 0));

		//Calculate formation positions and match each unit to a formation spot.
		List <Formation> formationPositions = FormationController.findFormationPositions (false, GameManager.player.getPlayer ().getCurUnitTarget (), targetLoc, unitVec, perpVec);
		GameManager.player.getPlayer ().sortCurUnitTarget (targetLoc, formationPositions);

		//Assign each unit to move to it's formation location
		foreach (var r in GameManager.player.getPlayer ().getCurUnitTarget ()) {
			if (r.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> () != null) {
				if (formationPositions.Count > 0) {
					/*if (clicked.GetComponent<UnitContainer> () != null || clicked.GetComponent<BuildingContainer> () != null) {
						r.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().destination = formationPositions [0].getPosition ();
						UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath ();
						UnityEngine.AI.NavMesh.CalculatePath (r.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().transform.position, r.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().destination, UnityEngine.AI.NavMesh.AllAreas, path);
						r.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().path = path;
					} else {*/
					UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath ();
					UnityEngine.AI.NavMesh.CalculatePath (r.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().transform.position, formationPositions [0].getPosition (), UnityEngine.AI.NavMesh.AllAreas, path);
					r.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().path = path;
					//}
					formationPositions.RemoveAt (0);
				} else {
					GameManager.print ("Missing FormationPosition - MouseController");
				}
			} else {
				GameManager.print ("Missing NavMeshAgent - MouseController");
			}
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
						//UnityEngine.AI.NavMeshPath pathX = newList [x].GetComponent<UnityEngine.AI.NavMeshAgent> ().path;
						//UnityEngine.AI.NavMeshPath pathY = newList [y].GetComponent<UnityEngine.AI.NavMeshAgent> ().path;
						float curDistanceX = 0;
						float curDistanceY = 0;
						float newDistanceX = 0;
						float newDistanceY = 0;

						//Another idea on how to do this
						curDistanceX = getPathLength (newList [x], _formationPositions [x].getPosition ());
						curDistanceY = getPathLength (newList [y], _formationPositions [y].getPosition ());

						newDistanceX = getPathLength (newList [x], _formationPositions [y].getPosition ());
						newDistanceY = getPathLength (newList [y], _formationPositions [x].getPosition ());

						//Old approach
						/*if (pathX.corners.Length >= 2 && pathY.corners.Length >= 2) {
							curDistanceX = Vector3.Distance (pathX.corners [pathX.corners.Length - 2], _formationPositions [x].getPosition ());
							curDistanceY = Vector3.Distance (pathY.corners [pathY.corners.Length - 2], _formationPositions [y].getPosition ());

							newDistanceX = Vector3.Distance (pathX.corners [pathX.corners.Length - 2], _formationPositions [y].getPosition ());
							newDistanceY = Vector3.Distance (pathY.corners [pathY.corners.Length - 2], _formationPositions [x].getPosition ());
						} else {
							curDistanceX = Vector3.Distance (newList [x].getUnit ().getCurLoc (), _formationPositions [x].getPosition ());
							curDistanceY = Vector3.Distance (newList [y].getUnit ().getCurLoc (), _formationPositions [y].getPosition ());

							newDistanceX = Vector3.Distance (newList [x].getUnit ().getCurLoc (), _formationPositions [y].getPosition ());
							newDistanceY = Vector3.Distance (newList [y].getUnit ().getCurLoc (), _formationPositions [x].getPosition ());
						}*/

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

	public void addResearch (Research _research) {
		researchList.Add (_research);
	}

	public List<Research> getResearch () {
		return researchList;
	}

	public bool hasResearch (Research _research) {
		for (int i = 0; i < researchList.Count; i++) {
			if (researchList [i].getName () == _research.getName ()) {
				return true;
			}
		}
		return false;
	}

	public int getpopulation () {
		return population;
	}

	public int getmaxPopulation () {
		return maxPopulation;
	}

	public void updatePopulation () {
		population = 0;
		foreach (var r in getUnits ()) {
			population += r.getUnit ().getPopulationCost ();
		}

		foreach (var r in getBuildings ()) {
			foreach (var u in r.getBuilding ().getUnitQueue ()) {
				population += u.getSize ();
			}
		}
	}

	public void updatemaxPopulation () {
		maxPopulation = 0;
		foreach (var r in getBuildings ()) {
			if (r.getBuilding ().getIsBuilt ()) {
				maxPopulation += r.getBuilding ().getPopulationValue ();
			}
		}
	}

	public bool hasPopulationSpace (int _add) {
		if (population + _add <= maxPopulation) {
			return true;
		} else {
			return false;
		}
	}

	//Mid development funct ion
	public float getPathLength (UnitContainer _unit, Vector3 _newPathEnd) {
		UnityEngine.AI.NavMeshPath path = _unit.GetComponent<UnityEngine.AI.NavMeshAgent> ().path;
		path.corners [path.corners.Length - 1] = _newPathEnd;
		float newDist = 0;
		for (int i = 0; i < path.corners.Length - 1; i++) {
			newDist += Vector3.Distance (path.corners [i], path.corners [i + 1]);
		}

		return newDist;
	}
}