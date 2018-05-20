using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class VisibleObjectsToObject {

	public List<UnitContainer> visiblePlayerUnits { get; protected set; }
	public List<BuildingContainer> visiblePlayerBuildings { get; protected set; }
	public List<UnitContainer> visibleResourceUnits { get; protected set; }
	public List<BuildingContainer> visibleResourceBuildings { get; protected set; }
	public List<UnitContainer> visibleEnemyUnits { get; protected set; }
	public List<BuildingContainer> visibleEnemyBuildings { get; protected set; }

	public List<BuildingContainer> visiblePlayerUnbuilt { get; protected set; }

	public List<BuildingContainer> visibleResourceFood { get; protected set; }
	public List<BuildingContainer> visibleResourceWood { get; protected set; }
	public List<BuildingContainer> visibleResourceGold { get; protected set; }

	public UnitContainer closestPlayerUnit { get; protected set; }
	public BuildingContainer closestPlayerBuilding { get; protected set; }
	public UnitContainer closestResourceUnit { get; protected set; }
	public BuildingContainer closestResourceBuilding { get; protected set; }
	public UnitContainer closestEnemyUnit { get; protected set; }
	public BuildingContainer closestEnemyBuilding { get; protected set; }

	public BuildingContainer closestPlayerUnbuilt { get; protected set; }

	public BuildingContainer closestResourceFood { get; protected set; }
	public BuildingContainer closestResourceWood { get; protected set; }
	public BuildingContainer closestResourceGold { get; protected set; }

	public float distanceToClosestPlayerUnitSqr { get; protected set; }
	public float distanceToClosestPlayerBuildingSqr { get; protected set; }
	public float distanceToClosestResourceUnitSqr { get; protected set; }
	public float distanceToClosestResourceBuildingSqr { get; protected set; }
	public float distanceToClosestEnemyUnitSqr { get; protected set; }
	public float distanceToClosestEnemyBuildingSqr { get; protected set; }

	public float distanceToClosestPlayerUnbuilt { get; protected set; }

	public float distanceToClosestResourceFoodSqr { get; protected set; }
	public float distanceToClosestResourceWoodSqr { get; protected set; }
	public float distanceToClosestResourceGoldSqr { get; protected set; }


	public VisibleObjectsToObject () {
		visiblePlayerUnits = new List<UnitContainer> ();
		visiblePlayerBuildings = new List<BuildingContainer> ();
		visibleResourceUnits = new List<UnitContainer> ();
		visibleResourceBuildings = new List<BuildingContainer> ();
		visibleEnemyUnits = new List<UnitContainer> ();
		visibleEnemyBuildings = new List<BuildingContainer> ();

		visiblePlayerUnbuilt = new List<BuildingContainer> ();

		visibleResourceFood = new List<BuildingContainer> ();
		visibleResourceWood = new List<BuildingContainer> ();
		visibleResourceGold = new List<BuildingContainer> ();

		resetVisibleObjects ();
	}

	public void resetVisibleObjects () {
		visiblePlayerUnits.Clear ();
		visiblePlayerBuildings.Clear ();
		visibleResourceUnits.Clear ();
		visibleResourceBuildings.Clear ();
		visibleEnemyUnits.Clear ();
		visibleEnemyBuildings.Clear ();

		closestPlayerUnit = null;
		closestPlayerBuilding = null;
		closestResourceUnit = null;
		closestResourceBuilding = null;
		closestEnemyUnit = null;
		closestEnemyBuilding = null;

		closestPlayerUnbuilt = null;

		closestResourceFood = null;
		closestResourceWood = null;
		closestResourceGold = null;

		distanceToClosestPlayerUnitSqr = 1000000;
		distanceToClosestPlayerBuildingSqr = 1000000;
		distanceToClosestResourceUnitSqr = 1000000;
		distanceToClosestResourceBuildingSqr = 1000000;
		distanceToClosestEnemyUnitSqr = 1000000;
		distanceToClosestEnemyBuildingSqr = 1000000;

		distanceToClosestPlayerUnbuilt = 1000000;

		distanceToClosestResourceFoodSqr = 1000000;
		distanceToClosestResourceWoodSqr = 1000000;
		distanceToClosestResourceGoldSqr = 1000000;
	}

	public void doCalculations () {
		
	}

	public void addPlayerUnit (UnitContainer _playerUnit, float _sqrDistance, bool _inSight) {
		if (_inSight) {
			visiblePlayerUnits.Add (_playerUnit);
		}

		if (_sqrDistance <= distanceToClosestPlayerUnitSqr) {
			distanceToClosestPlayerUnitSqr = _sqrDistance;
			closestPlayerUnit = _playerUnit;
		}
	}

	public void addPlayerBuilding (BuildingContainer _playerBuilding, float _sqrDistance, bool _inSight) {
		if (_inSight) {
			visiblePlayerBuildings.Add (_playerBuilding);
		}

		if (_sqrDistance <= distanceToClosestPlayerBuildingSqr) {
			distanceToClosestPlayerBuildingSqr = _sqrDistance;
			closestPlayerBuilding = _playerBuilding;
		}

		if (_playerBuilding.building.isBuilt == false) {
			visiblePlayerUnbuilt.Add (_playerBuilding);
			if (_sqrDistance <= distanceToClosestPlayerUnbuilt) {
				distanceToClosestPlayerUnbuilt = _sqrDistance;
				closestPlayerUnbuilt = _playerBuilding;
			}
		}
	}

	public void addResourceUnit (UnitContainer _resourceUnit, float _sqrDistance, bool _inSight) {
		if (_inSight) {
			visibleResourceUnits.Add (_resourceUnit);
		}

		if (_sqrDistance <= distanceToClosestResourceUnitSqr) {
			distanceToClosestResourceUnitSqr = _sqrDistance;
			closestResourceUnit = _resourceUnit;
		}
	}

	public void addResourceBuilding (BuildingContainer _resourceBuilding, float _sqrDistance, bool _inSight) {
		if (_inSight) {
			visibleResourceBuildings.Add (_resourceBuilding);
		}

		if (_sqrDistance <= distanceToClosestResourceBuildingSqr) {
			distanceToClosestResourceBuildingSqr = _sqrDistance;
			closestResourceBuilding = _resourceBuilding;
		}

		if (_resourceBuilding.building.resourceType == ResourceType.Food) {
			visibleResourceFood.Add (_resourceBuilding);
			if (_sqrDistance <= distanceToClosestResourceFoodSqr) {
				distanceToClosestResourceFoodSqr = _sqrDistance;
				closestResourceFood = _resourceBuilding;
			}
		}

		if (_resourceBuilding.building.resourceType == ResourceType.Wood) {
			visibleResourceWood.Add (_resourceBuilding);
			if (_sqrDistance <= distanceToClosestResourceWoodSqr) {
				distanceToClosestResourceWoodSqr = _sqrDistance;
				closestResourceWood = _resourceBuilding;
			}
		}

		if (_resourceBuilding.building.resourceType == ResourceType.Gold) {
			visibleResourceGold.Add (_resourceBuilding);
			if (_sqrDistance <= distanceToClosestResourceGoldSqr) {
				distanceToClosestResourceGoldSqr = _sqrDistance;
				closestResourceGold = _resourceBuilding;
			}
		}
	}

	public void addEnemyUnit (UnitContainer _enemyUnit, float _sqrDistance, bool _inSight) {
		if (_inSight) {
			visibleEnemyUnits.Add (_enemyUnit);
		}

		if (_sqrDistance <= distanceToClosestEnemyUnitSqr) {
			distanceToClosestEnemyUnitSqr = _sqrDistance;
			closestEnemyUnit = _enemyUnit;
		}
	}

	public void addEnemyBuilding (BuildingContainer _enemyBuilding, float _sqrDistance, bool _inSight) {
		if (_inSight) {
			visibleEnemyBuildings.Add (_enemyBuilding);
		}

		if (_sqrDistance <= distanceToClosestEnemyBuildingSqr) {
			distanceToClosestEnemyBuildingSqr = _sqrDistance;
			closestEnemyBuilding = _enemyBuilding;
		}
	}
}
