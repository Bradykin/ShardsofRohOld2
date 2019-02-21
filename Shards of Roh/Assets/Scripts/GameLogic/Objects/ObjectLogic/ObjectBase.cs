using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectBase : Purchaseable {

	//Variables that must be declared in subclass
	public float health { get; protected set; }

	//Variables that will default if not declared
	public string prefabPath { get; protected set; }
	public List<Research> neededResearch { get; protected set; }
	public List<Research> researchApplied { get; protected set; }
	public List<Ability> abilities { get; protected set; }
	public Vector4 healthbarDimensions { get; protected set; }

	//Variables that adjust during gameplay
	public float curHealth { get; protected set; }
	public Vector3 curLoc { get; set; }
	public bool isDead { get; protected set; }
	public VisibleObjectsToObject visibleObjects { get; protected set; }
	public string tooltipString { get; protected set; }

	protected void setup () {
		neededResearch = new List<Research> ();
		researchApplied = new List<Research> ();
		abilities = new List<Ability> ();
		healthbarDimensions = new Vector4 (1.0f, 1.0f, 2.0f, 0.2f);

		isDead = false;
		visibleObjects = new VisibleObjectsToObject ();
	}

	public bool useAbility (string _abilityName) {
		foreach (var r in abilities) {
			if (r.getName () == _abilityName) {
				//GameManager.print ("MakeUnitEnact: " + _abilityName);
				r.enact (owner);
				return true;
			}
		}

		return false;
	}

	public bool hasAbility (string _abilityName) {
		foreach (var r in abilities) {
			if (r.getName () == _abilityName) {
				return true;
			}
		}

		return false;
	}

	public bool hasResearch () {
		foreach (var r in neededResearch) {
			if (owner.hasResearch (r) == false) {
				return false;
			}
		}

		return true;
	}

	public bool hasResearchApplied (string _name) {
		foreach (var r in researchApplied) {
			if (r.name == _name) {
				return true;
			}
		}

		return false;
	}
}
