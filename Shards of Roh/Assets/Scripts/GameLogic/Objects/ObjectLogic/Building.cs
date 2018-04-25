using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public abstract class Building : Object {

	//Variables that must be declared in subclass

	//Variables that will default if not declared
	protected int populationValue = 0;
	protected bool isResource = false;
	protected ResourceType resourceType = ResourceType.None;
	protected bool isBuilt = true;
	protected bool toBuild = false;
	protected Vector3 navColliderSize = new Vector3 ();
	protected List<UnitQueue> unitQueue = new List<UnitQueue> ();
	protected List<ResearchQueue> researchQueue = new List<ResearchQueue> ();

	//Variables that adjust during gameplay

	public void initPostCreate (bool _isBuilt = true) {
		isBuilt = _isBuilt;
		if (isBuilt == true) {
			curHealth = health;
		} else {
			curHealth = 1;
		}
		isDead = false;
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
		if (_attacker.getUnit ().getVillager () == true) {
			if (getIsResource () == true) {
				if (getName () == "Food") {
					_attacker.getUnit ().getOwner ().getResource ().add (new Resource (_attack, 0, 0));
				} else if (getName () == "Wood") {
					_attacker.getUnit ().getOwner ().getResource ().add (new Resource (0, _attack, 0));
				} else if (getName () == "Gold") {
					_attacker.getUnit ().getOwner ().getResource ().add (new Resource (0, 0, _attack));
				} else {
					GameManager.print ("Unidentified resource - Building");
				}
				curHealth -= _attack;
			} else if (getOwner ().getName () == _attacker.getUnit ().getOwner ().getName ()) {
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
		targetLoc.x += getColliderSize ().x / 2;
		targetLoc.z -= getColliderSize ().z / 2;
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Camera.main.WorldToScreenPoint (targetLoc));
		if (Physics.Raycast (ray, out hit, 1000)) {
			if (unitQueue.Count > 0) {
				//Spawn units according to the size of the next unitQueue
				for (int i = 0; i < unitQueue [0].getSize (); i++) {
					Unit newUnit = ObjectFactory.createUnitByName (unitQueue [0].getUnit ().getName (), getOwner ());
					GameObject instance = GameManager.Instantiate (Resources.Load (newUnit.getPrefabPath (), typeof(GameObject)) as GameObject);
					instance.GetComponent<UnitContainer> ().setUnit (newUnit);
					if (instance.GetComponent<UnityEngine.AI.NavMeshAgent> () != null) {
						instance.GetComponent<UnityEngine.AI.NavMeshAgent> ().Warp (hit.point);
					}

					GameManager.addPlayerToGame (getOwner ().getName ()).addUnitToPlayer (instance.GetComponent<UnitContainer> ());
				}
				getUnitQueue ().RemoveAt (0);
			}
		}
	}

	public void createResearch () {
		//Gain the next research
		if (researchQueue.Count > 0) {
			GameManager.print ("AddResearch: " + researchQueue [0].getResearch ().getName ());
			GameManager.addPlayerToGame (getOwner ().getName ()).addResearch (researchQueue [0].getResearch ());
			getResearchQueue ().RemoveAt (0);
		}
	}

	public override string getPrefabPath () {
		return "Prefabs/" + race + "/Buildings/" + name;
	}

	public bool getDead () {
		return isDead;
	}

	public bool getIsResource () {
		return isResource;
	}

	public bool getIsBuilt () {
		return isBuilt;
	}

	public void setIsBuilt (bool _isBuilt) {
		isBuilt = _isBuilt;
	}

	public bool getToBuild () {
		return toBuild;
	}

	public void setToBuild (bool _toBuild) {
		toBuild = _toBuild;
	}

	public Vector3 getColliderSize () {
		return navColliderSize;
	}

	public void setColliderSize (Vector3 _size) {
		navColliderSize = _size;
	}

	public void addToUnitQueue (Unit _newUnit) {
		bool insert = false;
		for (int i = 0; i < unitQueue.Count; i++) {
			if (unitQueue [i].getUnit ().getName () == _newUnit.getName () && unitQueue [i].getFull () == false && insert == false) {
				unitQueue [i].addSize (1);
				insert = true;
			}
		}

		if (insert == false) {
			unitQueue.Add (new UnitQueue (_newUnit, 1));
		}
	}

	public List<UnitQueue> getUnitQueue () {
		return unitQueue;
	}

	public void addToResearchQueue (Research _newResearch) {
		researchQueue.Add (new ResearchQueue (_newResearch));
	}

	public List<ResearchQueue> getResearchQueue () {
		return researchQueue;
	}

	public int getPopulationValue () {
		return populationValue;
	}
}
