using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : Object {

	//Variables that must be declared in subclass

	//Variables that will default if not declared
	protected bool isResource = false;
	protected bool isBuilt = true;
	protected bool toBuild = false;
	protected Vector3 navColliderSize = new Vector3 ();

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

	public void getHit (Unit _attacker, int _attack) {
		if (_attacker.getVillager () == true) {
			if (getIsResource () == true) {
				if (getName () == "Food") {
					_attacker.getOwner ().getResource ().add (new Resource (_attack, 0, 0));
				} else if (getName () == "Wood") {
					_attacker.getOwner ().getResource ().add (new Resource (0, _attack, 0));
				} else if (getName () == "Gold") {
					_attacker.getOwner ().getResource ().add (new Resource (0, 0, _attack));
				} else {
					GameManager.print ("Unidentified resource - Building");
				}
				curHealth -= _attack;
			} else if (getOwner ().getName () == _attacker.getOwner ().getName ()) {
				curHealth += _attack;
			} else {
				curHealth -= _attack;
			}
		} else {
			curHealth -= _attack;
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
}
