using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public abstract class Building : ObjectBase {

	//Variables that must be declared in subclass

	//Variables that will default if not declared
	public int populationValue { get; protected set; }
	public bool isResource { get; protected set; }
	public ResourceType resourceType { get; protected set; }
	public FoodType foodType { get; protected set; }
	public bool isBuilt { get; set; }
	public bool toBuild { get; set; }
	public Vector3 navColliderSize { get; set; }
	public List<UnitQueue> unitQueue { get; protected set; }
	public List<ResearchQueue> researchQueue { get; protected set; }
	public Vector3 wayPoint { get; set; }
	public UnitContainer unitWayPointTarget { get; set; }
	public BuildingContainer buildingWayPointTarget { get; set; }

	//Variables that adjust during gameplay

	public void buildingSetup () {
		setup ();
		populationValue = 0;
		isResource = false;
		resourceType = ResourceType.None;
		foodType = FoodType.None;
		isBuilt = true;
		toBuild = false;
		navColliderSize = new Vector3 ();
		unitQueue = new List<UnitQueue> ();
		researchQueue = new List<ResearchQueue> ();

		//Temporary
		Vector3 targetLoc = curLoc;
		targetLoc.x += navColliderSize.x;
		targetLoc.z -= navColliderSize.z;
		wayPoint = targetLoc;
	}

	public void initPostCreate (bool _isBuilt = true) {
		isBuilt = _isBuilt;
		if (isBuilt == true) {
			curHealth = health;
		} else {
			curHealth = 1;
		}
		prefabPath = "Prefabs/" + race + "/Buildings/" + name;
	}

	public void update () {
		if (isBuilt == true) {
			if (curHealth <= 0) {
				isDead = true;
			}
		} else {
			if (curHealth >= health) {
				toBuild = true;
			}
		}

		updateToolTip ();
	}

	public void getHit (UnitContainer _attacker, float _attack) {
		if (_attacker.unit.unitType == UnitType.Villager) {
			if (isResource == true) {
				if (resourceType == ResourceType.Food) {
					if (foodType == FoodType.Animal) {
						_attacker.unit.owner.addResources (new Resource (_attacker.unit.foodAnimalGatherRate, 0, 0, 0));
						curHealth -= _attacker.unit.foodAnimalGatherRate;
					} else if (foodType == FoodType.Forage) {
						_attacker.unit.owner.addResources (new Resource (_attacker.unit.foodForageGatherRate, 0, 0, 0));
						curHealth -= _attacker.unit.foodForageGatherRate;
					} else if (foodType == FoodType.Farm) {
						_attacker.unit.owner.addResources (new Resource (_attacker.unit.foodFarmGatherRate, 0, 0, 0));
					}
				} else if (resourceType == ResourceType.Wood) {
					_attacker.unit.owner.addResources (new Resource (0, _attacker.unit.woodGatherRate, 0, 0));
					curHealth -= _attacker.unit.woodGatherRate;
				} else if (resourceType == ResourceType.Gold) {
					_attacker.unit.owner.addResources (new Resource (0, 0, _attacker.unit.goldGatherRate, 0));
					curHealth -= _attacker.unit.goldGatherRate;
				} else if (resourceType == ResourceType.Metal) {
					_attacker.unit.owner.addResources (new Resource (0, 0, 0, _attacker.unit.metalGatherRate));
					curHealth -= _attacker.unit.metalGatherRate;
				} else {
					GameManager.print ("Unidentified resource - Building");
				}
			} else if (owner.name == _attacker.unit.owner.name) {
				curHealth += _attacker.unit.buildRate;
				if (curHealth >= health) {
					curHealth = health;
				}
			} else {
				curHealth -= _attack;
			}
		} else {
			curHealth -= _attack;
		}
	}

	public void createResearch () {
		//Gain the next research
		if (researchQueue.Count > 0) {
			GameManager.addPlayerToGame (owner.name).researchList.Add (researchQueue [0].research);
			researchQueue [0].research.applyOnFinish ();
			researchQueue.RemoveAt (0);
		}
	}

	public void addToUnitQueue (Unit _newUnit) {
		bool insert = false;
		for (int i = 0; i < unitQueue.Count; i++) {
			if (unitQueue [i].unit.name == _newUnit.name && unitQueue [i].getFull () == false && insert == false) {
				unitQueue [i].size += 1;
				insert = true;
			}
		}

		if (insert == false) {
			unitQueue.Add (new UnitQueue (_newUnit, 1));
		}
	}

	public void updateToolTip () {
		tooltipString = name;
	}
}
