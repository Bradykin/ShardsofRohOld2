using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public abstract class Building : Object {

	//Variables that must be declared in subclass

	//Variables that will default if not declared
	public int populationValue { get; protected set; }
	public bool isResource { get; protected set; }
	public ResourceType resourceType { get; protected set; }
	public bool isBuilt { get; set; }
	public bool toBuild { get; set; }
	public Vector3 navColliderSize { get; set; }
	public List<UnitQueue> unitQueue { get; protected set; }
	public List<ResearchQueue> researchQueue { get; protected set; }

	//Variables that adjust during gameplay

	public void buildingSetup () {
		setup ();
		populationValue = 0;
		isResource = false;
		resourceType = ResourceType.None;
		isBuilt = true;
		toBuild = false;
		navColliderSize = new Vector3 ();
		unitQueue = new List<UnitQueue> ();
		researchQueue = new List<ResearchQueue> ();
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
	}

	public void getHit (UnitContainer _attacker, int _attack) {
		if (_attacker.unit.isVillager == true) {
			if (isResource == true) {
				if (resourceType == ResourceType.Food) {
					_attacker.unit.owner.resource.add (new Resource (_attack, 0, 0));
				} else if (resourceType == ResourceType.Wood) {
					_attacker.unit.owner.resource.add (new Resource (0, _attack, 0));
				} else if (resourceType == ResourceType.Gold) {
					_attacker.unit.owner.resource.add (new Resource (0, 0, _attack));
				} else {
					GameManager.print ("Unidentified resource - Building");
				}
				curHealth -= _attack;
			} else if (owner.name == _attacker.unit.owner.name) {
				curHealth += _attack;
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

	public void createUnit () {
		//Set unit spawn location
		Vector3 targetLoc = curLoc;
		targetLoc.x += navColliderSize.x / 2;
		targetLoc.z -= navColliderSize.z / 2;
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Camera.main.WorldToScreenPoint (targetLoc));
		if (Physics.Raycast (ray, out hit, 1000, GlobalVariables.defaultMask)) {
			if (unitQueue.Count > 0) {
				//Spawn units according to the size of the next unitQueue
				for (int i = 0; i < unitQueue [0].size; i++) {
					Unit newUnit = ObjectFactory.createUnitByName (unitQueue [0].unit.name, owner);
					GameObject instance = GameManager.Instantiate (Resources.Load (newUnit.prefabPath, typeof(GameObject)) as GameObject);
					instance.GetComponent<UnitContainer> ().unit = newUnit;
					if (instance.GetComponent<UnityEngine.AI.NavMeshAgent> () != null) {
						instance.GetComponent<UnityEngine.AI.NavMeshAgent> ().Warp (hit.point);
					}

					GameManager.addPlayerToGame (owner.name).units.Add (instance.GetComponent<UnitContainer> ());
				}
				unitQueue.RemoveAt (0);
			}
		}
	}

	public void createResearch () {
		//Gain the next research
		if (researchQueue.Count > 0) {
			GameManager.addPlayerToGame (owner.name).researchList.Add (researchQueue [0].research);
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
}
