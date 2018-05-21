using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {

	public string name { get; set; }
	public int population { get; private set; }
	public int maxPopulation { get; private set; }

	public List<UnitContainer> units { get; private set; }
	public List<BuildingContainer> buildings { get; private set; }
	public List<UnitContainer> curUnitTarget { get; private set; }
	public List<BuildingContainer> curBuildingTarget { get; private set; }
	public List<Research> researchList { get; set; }
	public Resource resource { get; set; }

	public VisibleObjectsToPlayer visibleObjects { get; protected set; }
		
	public Player (string _name) {
		name = _name;
		units = new List<UnitContainer> ();
		buildings = new List<BuildingContainer> ();
		curUnitTarget = new List<UnitContainer> ();
		curBuildingTarget = new List<BuildingContainer> ();
		researchList = new List<Research> ();
		resource = new Resource (0, 0, 0);
		visibleObjects = new VisibleObjectsToPlayer (this);
		updatePopulation ();
		updateMaxPopulation ();
	}

	public void update () {
		visibleObjects.updateVisible ();
		updatePopulation ();
		updateMaxPopulation ();
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
		curUnitTarget.Clear ();
		curUnitTarget = _curUnitTarget;
		toggleSelectionCircles (true);
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

	public void processFormationMovement (Vector3 targetLoc) {
		foreach (var r in GameManager.playerContainer.player.curUnitTarget) {
			r.unit.dropAttackTarget ();
			r.removeBehaviourByType ("Idle");
		}

		//Calculate angle vectors for formations
		Vector3 unitVec = new Vector3 (0, 0, 0);
		int unitCount = 0;
		foreach (var r in GameManager.playerContainer.player.curUnitTarget) {
			//This works well, but I worry about performance issues. Think of a better way!
			if (r.agent.enabled == true) {
				unitCount++;
				UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath ();
				UnityEngine.AI.NavMesh.CalculatePath (r.agent.transform.position, targetLoc, UnityEngine.AI.NavMesh.AllAreas, path);
				r.agent.path = path;
				if (path.corners.Length >= 2) {
					unitVec = unitVec + path.corners [path.corners.Length - 2];
				} else {
					unitVec = unitVec + r.unit.curLoc;
				}
			}
		}
		unitVec = unitVec / unitCount;
		unitVec = (unitVec - targetLoc).normalized * 4;
		Vector3 perpVec = Vector3.Cross (unitVec, new Vector3 (0, 1, 0));

		//Calculate formation positions and match each unit to a formation spot.
		List <Formation> formationPositions = FormationController.findFormationPositions (false, GameManager.playerContainer.player.curUnitTarget, targetLoc, unitVec, perpVec);
		GameManager.playerContainer.player.sortCurUnitTarget (targetLoc, formationPositions);

		//Assign each unit to move to it's formation location
		foreach (var r in GameManager.playerContainer.player.curUnitTarget) {
			if (r.agent != null) {
				if (formationPositions.Count > 0) {
					r.navMeshToggle ("Agent");
					r.moveToLocation (formationPositions [0].getPosition ());
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
			UnitContainer lowest = curUnitTarget [0];

			foreach (var r in curUnitTarget) {
				if (_formationPositions.Count > 0) {
					float distanceToPosition = Vector3.Distance (r.unit.curLoc, _formationPositions [x].getPosition ());
					if (distance == -1 || distanceToPosition < distance) {
						if (r.unit.name == _formationPositions [x].getUnitType () || _formationPositions [x].getUnitType () == "") {
							lowest = r;
							distance = distanceToPosition;
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
						float curDistanceX = getPathLength (newList [x], _formationPositions [x].getPosition ());
						float curDistanceY = getPathLength (newList [y], _formationPositions [y].getPosition ());
						float newDistanceX = getPathLength (newList [x], _formationPositions [y].getPosition ());
						float newDistanceY = getPathLength (newList [y], _formationPositions [x].getPosition ());

						if ((Mathf.Max (curDistanceX, curDistanceY) - Mathf.Min (curDistanceX, curDistanceY)) > (Mathf.Max (newDistanceX, newDistanceY) - Mathf.Min (newDistanceX, newDistanceY))) {
							if ((curDistanceX + curDistanceY) > (newDistanceX + newDistanceY)) {
								findBetter = true;
								UnitContainer temp = newList [x];
								newList [x] = newList [y];
								newList [y] = temp;
							}
						}
					}
				}
			}
		}

		setCurUnitTarget (newList);
	}

	public void toggleSelectionCircle (bool _toggle, UnitContainer _unit) {
		foreach (Transform child in _unit.gameObject.transform) {
			if (child.name == "TargetRing") {
				child.gameObject.SetActive (_toggle);
			}
		}
	}

	public void toggleSelectionCircles (bool _toggle) {
		foreach (var r in curUnitTarget) {
			foreach (Transform child in r.gameObject.transform) {
				if (child.name == "TargetRing") {
					child.gameObject.SetActive (_toggle);
				}
			}
		}
	}

	public void toggleSelectionBox (bool _toggle, BuildingContainer _building) {
		foreach (Transform child in _building.gameObject.transform) {
			if (child.name == "TargetRing") {
				child.gameObject.SetActive (_toggle);
			}
		}
	}

	public void toggleSelectionBoxes (bool _toggle) {
		foreach (var r in curBuildingTarget) {
			foreach (Transform child in r.gameObject.transform) {
				if (child.name == "TargetRing") {
					child.gameObject.SetActive (_toggle);
				}
			}
		}
	}

	public bool hasResearch (Research _research) {
		foreach (var r in researchList) {
			if (r.getName () == _research.getName ()) {
				return true;
			}
		}

		return false;
	}

	public void updatePopulation () {
		population = 0;
		foreach (var r in units) {
			population += r.unit.populationCost;
		}

		foreach (var r in buildings) {
			foreach (var u in r.building.unitQueue) {
				population += u.size;
			}
		}
	}

	public void updateMaxPopulation () {
		maxPopulation = 0;
		foreach (var r in buildings) {
			if (r.building.isBuilt) {
				maxPopulation += r.building.populationValue;
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

	public float getPathLength (UnitContainer _unit, Vector3 _newPathEnd) {
		UnityEngine.AI.NavMeshPath path = _unit.agent.path;
		path.corners [path.corners.Length - 1] = _newPathEnd;
		float newDist = 0;
		//This is a potential inefficiency
		for (int i = 0; i < path.corners.Length - 1; i++) {
			newDist += Vector3.Distance (path.corners [i], path.corners [i + 1]);
		}

		return newDist;
	}

	public void useCurTargetAbility (int _index) {
		if (GameManager.playerContainer.player.curUnitTarget.Count > 0) {
			//This shouldn't check first in list, eventually should use some other logic system
			if (GameManager.playerContainer.player.curUnitTarget [0].unit.abilities [_index] != null) {
				GameManager.playerContainer.player.curUnitTarget [0].unit.abilities [_index].enact (GameManager.playerContainer.player);
			}
		} else if (GameManager.playerContainer.player.curBuildingTarget.Count > 0) {
			//This shouldn't check first in list, eventually should use some other logic system
			if (GameManager.playerContainer.player.curBuildingTarget [0].building.abilities [_index] != null) {
				GameManager.playerContainer.player.curBuildingTarget [0].building.abilities [_index].enact (GameManager.playerContainer.player);
			}
		}
	}

	public void createBuildingFoundation (string _buildingName, Vector3 _location) {
		Building newBuilding = ObjectFactory.createBuildingByName (_buildingName, this, false);

		if (resource.hasEnough (newBuilding.cost)) {
			resource.spend (newBuilding.cost);
			GameObject instance = GameManager.Instantiate (Resources.Load (newBuilding.prefabPath, typeof(GameObject)) as GameObject);
			instance.GetComponent<BuildingContainer> ().building = newBuilding;
			if (instance.GetComponent<UnityEngine.AI.NavMeshObstacle> () != null) {
				instance.GetComponent<UnityEngine.AI.NavMeshObstacle> ().transform.position = (new Vector3 (_location.x, Terrain.activeTerrain.SampleHeight (_location), _location.z));
			}

			if (instance.transform.GetChild (0).gameObject.name == "Model" && instance.transform.GetChild (1).gameObject.name == "Foundation") {
				instance.transform.GetChild (0).gameObject.SetActive (false);
				instance.transform.GetChild (1).gameObject.SetActive (true);
				instance.GetComponent<BoxCollider> ().center = instance.transform.GetChild (1).GetComponent <BoxCollider> ().center;
				instance.GetComponent<BoxCollider> ().size = instance.transform.GetChild (1).GetComponent <BoxCollider> ().size;
				instance.GetComponent<UnityEngine.AI.NavMeshObstacle> ().center = instance.transform.GetChild (1).GetComponent<UnityEngine.AI.NavMeshObstacle> ().center;
				instance.GetComponent<UnityEngine.AI.NavMeshObstacle> ().size = instance.transform.GetChild (1).GetComponent<UnityEngine.AI.NavMeshObstacle> ().size;
			} else {
				GameManager.print ("Model Child problem - MouseController");
			}

			GameManager.addPlayerToGame (name).buildings.Add (instance.GetComponent<BuildingContainer> ());
		}


	}
}