using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public abstract class Research {

	public Player owner { get; protected set; }
	public string name { get; protected set; }
	public Resource cost { get; protected set; }
	public float queueTime { get; protected set; }
	public List<Research> neededResearch { get; protected set; }

	public abstract void applyOnFinish ();

	public abstract void applyToUnit (Unit _unit);

	public abstract void applyToBuilding (Building _building);
}
