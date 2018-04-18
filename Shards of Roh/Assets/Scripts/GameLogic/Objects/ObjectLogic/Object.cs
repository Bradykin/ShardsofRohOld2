using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Object {

	//Variables that must be declared in subclass
	protected string name;
	protected string race;
	protected Player owner;
	protected int health;
	protected float curHealth;
	protected List<Ability> abilities = new List<Ability> ();
	protected Resource cost;

	//Variables that will default if not declared

	//Variables that adjust during gameplay
	protected Vector3 curLoc;
	protected bool isDead;

	public Vector3 getCurLoc () {
		return curLoc;
	}

	public void setCurLoc (Vector3 _curLoc) {
		curLoc = _curLoc;
	}

	public Player getOwner () {
		return owner;
	}

	public void setOwner (Player _owner) {
		owner = _owner;
	}

	public string getName () {
		return name;
	}

	public Ability getAbility (int _index) {
		if (abilities.Count > _index) {
			return abilities [_index];
		} else {
			return null;
		}
	}

	public List<Ability> getAbilities () {
		return abilities;
	}

	public abstract string getPrefabPath ();

	public Resource getCost () {
		return cost;
	}
}
