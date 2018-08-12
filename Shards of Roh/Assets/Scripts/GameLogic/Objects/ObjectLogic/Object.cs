	using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Object {

	//Variables that must be declared in subclass
	public string name { get; protected set; }
	public string race { get; protected set; }
	public Player owner { get; set; }
	public float health { get; protected set; }
	public Resource cost { get; protected set; }

	//Variables that will default if not declared
	public string prefabPath { get; protected set; }
	public List<Research> neededResearch { get; protected set; }
	public List<Ability> abilities { get; protected set; }
	public Vector4 healthbarDimensions { get; protected set; }

	//Variables that adjust during gameplay
	public float curHealth { get; protected set; }
	public Vector3 curLoc { get; set; }
	public bool isDead { get; protected set; }
	public VisibleObjectsToObject visibleObjects { get; protected set; }

	protected void setup () {
		neededResearch = new List<Research> ();
		abilities = new List<Ability> ();
		healthbarDimensions = new Vector4 (1.0f, 1.0f, 2.0f, 0.2f);

		isDead = false;
		visibleObjects = new VisibleObjectsToObject ();
	}
}
