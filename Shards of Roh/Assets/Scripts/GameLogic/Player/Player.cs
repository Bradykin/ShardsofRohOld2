using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class Player {

	public string name { get; private set; }
	public Race playerRace { get; private set; }
	public int population { get; private set; }
	public int maxPopulation { get; private set; }
	public int curUnitFocusIndex { get; private set; }
	public int curBuildingFocusIndex { get; private set; }

	public List<UnitContainer> units { get; private set; }
	public List<BuildingContainer> buildings { get; private set; }
	public List<UnitContainer> curUnitTarget { get; private set; }
	public List<BuildingContainer> curBuildingTarget { get; private set; }
	public List<Research> researchList { get; private set; }
	public Resource resource { get; private set; }
	public VisibleObjectsToPlayer visibleObjects { get; private set; }

	//Establish all player variables
	public Player (string _name, string _race) {
		name = _name;
		playerRace = new Humans (this); //This should eventually be based on passing in the _race string
		units = new List<UnitContainer> ();
		buildings = new List<BuildingContainer> ();
		curUnitTarget = new List<UnitContainer> ();
		curBuildingTarget = new List<BuildingContainer> ();
		curUnitFocusIndex = -1;
		curBuildingFocusIndex = -1;
		researchList = new List<Research> ();
		resource = new Resource (0, 0, 0, 0);
		visibleObjects = new VisibleObjectsToPlayer (this);
		updatePopulation ();
		updateMaxPopulation ();
		updateCurFocusIndex ();
	}

	//Call various update functions
	public void update () {
		visibleObjects.updateVisible ();
		updatePopulation ();
		updateMaxPopulation ();
	}

	//The add, rem, and set functions for unit and building are called to modify the curUnitTarget and curBuildingTarget lists by the user input classes
	public void addCurUnitTarget (UnitContainer _curUnitTarget) {
		if (!curUnitTarget.Contains (_curUnitTarget)) {
			curUnitTarget.Add (_curUnitTarget);
			toggleSelectionCircle (true, _curUnitTarget);
			updateCurFocusIndex ();
		}
	}

	//The add, rem, and set functions for unit and building are called to modify the curUnitTarget and curBuildingTarget lists by the user input classes
	public void remCurUnitTarget (UnitContainer _curUnitTarget) {
		if (curUnitTarget.Contains (_curUnitTarget)) {
			curUnitTarget.Remove (_curUnitTarget);
			toggleSelectionCircle (false, _curUnitTarget);
			updateCurFocusIndex ();
		}
	}

	//The add, rem, and set functions for unit and building are called to modify the curUnitTarget and curBuildingTarget lists by the user input classes
	public void setCurUnitTarget (UnitContainer _curUnitTarget) {
		toggleSelectionCircles (false);
		curUnitTarget.Clear ();
		curUnitTarget.Add (_curUnitTarget);
		toggleSelectionCircle (true, _curUnitTarget);
		updateCurFocusIndex ();
	}

	//The add, rem, and set functions for unit and building are called to modify the curUnitTarget and curBuildingTarget lists by the user input classes
	public void setCurUnitTarget (List<UnitContainer> _curUnitTarget) {
		toggleSelectionCircles (false);
		curUnitTarget.Clear ();
		curUnitTarget = _curUnitTarget;
		toggleSelectionCircles (true);
		updateCurFocusIndex ();
	}

	//The add, rem, and set functions for unit and building are called to modify the curUnitTarget and curBuildingTarget lists by the user input classes
	public void addCurBuildingTarget (BuildingContainer _curBuildingTarget) {
		if (!curBuildingTarget.Contains (_curBuildingTarget)) {
			curBuildingTarget.Add (_curBuildingTarget);
			toggleSelectionBox (true, _curBuildingTarget);
			updateCurFocusIndex ();
		}
	}

	//The add, rem, and set functions for unit and building are called to modify the curUnitTarget and curBuildingTarget lists by the user input classes
	public void remCurBuildingTarget (BuildingContainer _curBuildingTarget) {
		if (curBuildingTarget.Contains (_curBuildingTarget)) {
			curBuildingTarget.Remove (_curBuildingTarget);
			toggleSelectionBox (false, _curBuildingTarget);
			updateCurFocusIndex ();
		}
	}

	//The add, rem, and set functions for unit and building are called to modify the curUnitTarget and curBuildingTarget lists by the user input classes
	public void setCurBuildingTarget (BuildingContainer _curBuildingTarget) {
		toggleSelectionBoxes (false);
		curBuildingTarget.Clear ();
		curBuildingTarget.Add (_curBuildingTarget);
		toggleSelectionBox (true, _curBuildingTarget);
		updateCurFocusIndex ();
	}

	//The add, rem, and set functions for unit and building are called to modify the curUnitTarget and curBuildingTarget lists by the user input classes
	public void setCurBuildingTarget (List<BuildingContainer> _curBuildingTarget) {
		toggleSelectionBoxes (false);
		curBuildingTarget = _curBuildingTarget;
		toggleSelectionBoxes (true);
		updateCurFocusIndex ();
	}

	//The toggleSelection circle and box functions are called to enable and disable the TargetRings and TargetBoxes on units and buildings to denote them being selected by this class
	public void toggleSelectionCircle (bool _toggle, UnitContainer _unit) {

		foreach (Transform child in _unit.gameObject.transform) {
			if (child.name == "TargetRing" || child.name == "Waypoint") {
				child.gameObject.SetActive (_toggle);
			}
		}
	}

	//The toggleSelection circle and box functions are called to enable and disable the TargetRings and TargetBoxes on units and buildings to denote them being selected by this class
	public void toggleSelectionCircles (bool _toggle) {
		foreach (var r in curUnitTarget) {

			foreach (Transform child in r.gameObject.transform) {
				if (child.name == "TargetRing" || child.name == "Waypoint") {
					child.gameObject.SetActive (_toggle);
				}
			}
		}
	}

	//The toggleSelection circle and box functions are called to enable and disable the TargetRings and TargetBoxes on units and buildings to denote them being selected by this class
	public void toggleSelectionBox (bool _toggle, BuildingContainer _building) {
		foreach (Transform child in _building.gameObject.transform) {
			if (child.name == "TargetRing" || child.name == "Waypoint") {
				child.gameObject.SetActive (_toggle);
			}
		}
	}

	//The toggleSelection circle and box functions are called to enable and disable the TargetRings and TargetBoxes on units and buildings to denote them being selected by this class
	public void toggleSelectionBoxes (bool _toggle) {
		foreach (var r in curBuildingTarget) {
			foreach (Transform child in r.gameObject.transform) {
				if (child.name == "TargetRing" || child.name == "Waypoint") {
					child.gameObject.SetActive (_toggle);
				}
			}
		}
	}

	//This function, currently placeholder, sets the values of curUnitFocusIndex and curBuildingFocusIndex to determine which of the selected objects is primary focus.
	//This is used to display UI abilities and data, by the AbilityPanelDisplay and to determine which abilities are used from input in HotKeysController
	public void updateCurFocusIndex () {
		//This is likely to get additional logic in the future
		if (curUnitTarget.Count == 0) {
			curUnitFocusIndex = -1;
		} else {
			curUnitFocusIndex = 0;
		}

		if (curBuildingTarget.Count == 0) {
			curBuildingFocusIndex = -1;
		} else {
			curBuildingFocusIndex = 0;
		}
	}

	//This function processes a movement command from PlayerContainer to determine the destinations for ont or more units moved in formation.
	public void processFormationMovement (Vector3 targetLoc, bool _isWayPointing = false) {
		//1: Clear unit attack targets and idle behaviours
		foreach (var r in GameManager.playerContainer.player.curUnitTarget) {
			r.unit.dropAttackTarget ();
			if (r.unit.unitType == UnitType.Villager) {
				r.removeBehaviourByType ("Idle");
			}
		}

		//2: Calculate angle vectors from each unit to destination to determine formation's angle of approach
		Vector3 unitVec = new Vector3 (0, 0, 0);
		int unitCount = 0;
		foreach (var r in GameManager.playerContainer.player.curUnitTarget) {
			//This works well, but I worry about performance issues. Update: No performance issues noticed thus far but leaving this in anyways.
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

		//3: Calculate formation positions and match each unit to a formation spot, using the findFormationPositions and sortCurUnitTarget functions
		List <Formation> formationPositions = FormationController.findFormationPositions (false, GameManager.playerContainer.player.curUnitTarget, targetLoc, unitVec, perpVec);
		GameManager.playerContainer.player.sortCurUnitTarget (targetLoc, formationPositions);

		//4: Assign each unit to move to it's formation location
		foreach (var r in GameManager.playerContainer.player.curUnitTarget) {
			if (r.agent != null) {
				if (formationPositions.Count > 0) {
					r.navMeshToggle ("Agent");
					r.moveToLocation (true, formationPositions [0].getPosition (), _isWayPointing);
					formationPositions.RemoveAt (0);
				} else {
					GameManager.print ("Missing FormationPosition - MouseController");
				}
			} else {
				GameManager.print ("Missing NavMeshAgent - MouseController");
			}
		}
	}

	//This function determines which unit is matched to which position in a calculated formation when called by processFormationMovement.
	private void sortCurUnitTarget (Vector3 _destination, List <Formation> _formationPositions) {
		List <UnitContainer> newList = new List <UnitContainer> ();

		//1: Create a rough draft assignment based on assigning each unit in order to the closest un-assigned matching formation spot
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

		//2: This section compares every pair of units in the formation together, to assess the benefits of switching their formation destinations. 
		//It switches them if the total travel time would be reduced, AND the difference in the travel time between the two units would be reduced
		//Repeats looping through every possible pairing until it finds no beneficial swaps
		//Should formation calculations start lagging, this is almost certainly why. HIGHLY SUSPECT FOR LAG CODE BLOCK
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

		//3: Set the waypoint flag of each unit to their respective destination
		foreach (var r in newList) {
			r.unit.flagPosition = _formationPositions [0].getPosition ();
		}

		setCurUnitTarget (newList);
	}

	//Check if the player has a specific resource. Called by a variety of sources.
	public bool hasResearch (Research _research) {
		foreach (var r in researchList) {
			if (r.name == _research.name) {
				return true;
			}
		}

		return false;
	}

	//Update the player's current populaation. Called by Player update.
	private void updatePopulation () {
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

	//Update the player's max populaation. Called by Player update.
	private void updateMaxPopulation () {
		maxPopulation = 0;
		foreach (var r in buildings) {
			if (r.building.isBuilt) {
				maxPopulation += r.building.populationValue;
			}
		}
	}

	//Check if the player has the population space amount passed in. Called by a variety of sources.
	public bool hasPopulationSpace (int _add) {
		if (population + _add <= maxPopulation) {
			return true;
		} else {
			return false;
		}
	}

	//Helper function called by sortCurUnitTarget to determine the length of a path between a unit and a destination.
	private float getPathLength (UnitContainer _unit, Vector3 _newPathEnd) {
		UnityEngine.AI.NavMeshPath path = _unit.agent.path;
		path.corners [path.corners.Length - 1] = _newPathEnd;
		float newDist = 0;
		//This is a potential inefficiency
		for (int i = 0; i < path.corners.Length - 1; i++) {
			newDist += Vector3.Distance (path.corners [i], path.corners [i + 1]);
		}

		return newDist;
	}

	//This function called by user Input functions, calls the ability of the currently focused target at the specified index
	public void useCurTargetAbility (int _index) {
		if (GameManager.playerContainer.player.curUnitFocusIndex != -1) {
			//This shouldn't check first in list, eventually should use some other logic system
			if (GameManager.playerContainer.player.curUnitTarget [curUnitFocusIndex].unit.abilities.Count > _index && GameManager.playerContainer.player.curUnitTarget [curUnitFocusIndex].unit.abilities [_index] != null) {
				GameManager.playerContainer.player.curUnitTarget [curUnitFocusIndex].unit.abilities [_index].enact (GameManager.playerContainer.player);
			}
		} else if (GameManager.playerContainer.player.curBuildingFocusIndex != -1) {
			//This shouldn't check first in list, eventually should use some other logic system
			if (GameManager.playerContainer.player.curBuildingTarget [curBuildingFocusIndex].building.abilities.Count > _index && GameManager.playerContainer.player.curBuildingTarget [curBuildingFocusIndex].building.abilities [_index] != null) {
				GameManager.playerContainer.player.curBuildingTarget [curBuildingFocusIndex].building.abilities [_index].enact (GameManager.playerContainer.player);
			}
		}
	}

	//This function is used to process a command to place down a building foundation. It checks if one can be placed, and if it can, creates it and spends the required resources
	public bool createBuildingFoundation (string _buildingName, Vector3 _location) {
		Building newBuilding = ObjectFactory.createBuildingByName (_buildingName, this, false);

		//1: If the player has enough resources, create an instance of the building and relocate it to the intended position
		if (resource.hasEnough (newBuilding.cost)) {
			GameObject instance = GameManager.Instantiate (Resources.Load (newBuilding.prefabPath, typeof(GameObject)) as GameObject);

			instance.GetComponent<BuildingContainer> ().building = newBuilding;
			if (instance.GetComponent<UnityEngine.AI.NavMeshObstacle> () != null) {
				instance.GetComponent<UnityEngine.AI.NavMeshObstacle> ().transform.position = (new Vector3 (_location.x, Terrain.activeTerrain.SampleHeight (_location), _location.z));
			}

			if (instance.transform.GetChild (0).gameObject.name == "Model" && instance.transform.GetChild (1).gameObject.name == "Foundation") {
				instance.transform.GetChild (0).gameObject.SetActive (false);
				instance.transform.GetChild (1).gameObject.SetActive (true);
			} else {
				GameManager.print ("Model Child problem - MouseController");
			}

			instance.GetComponent<BuildingContainer> ().setWaypointFlagActive (false);

			//2: Check if the instance of the building overlaps with any current unit or building. If it does, destroy it.
			Collider[] hitColliders = Physics.OverlapBox (instance.transform.position, instance.GetComponent<BoxCollider> ().size / 2, new Quaternion (0.9240f, 0.0f, 0.383f, 0.0f));
			foreach (var r in hitColliders) {
				if (r.gameObject != instance) {
					if (r.gameObject.GetComponent<UnitContainer> () != null) {
						GameManager.Destroy (instance);
						return false;
					}

					if (r.gameObject.GetComponent<BuildingContainer> () != null) {
						GameManager.Destroy (instance);
						return false;
					}
				}
			}

			//3: If reached this part of the function, spend the resources to create the building, and add the building to the correct player
			resource.spend (newBuilding.cost);
			GameManager.addPlayerToGame (name).buildings.Add (instance.GetComponent<BuildingContainer> ());

			return true;
		}

		return false;
	}
}