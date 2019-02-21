using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public abstract class Research : Purchaseable {


	public float queueTime { get; protected set; }
	public List<Research> neededResearch { get; protected set; }

	public abstract void applyOnFinish ();

	public abstract void applyToUnit (Unit _unit);

	public abstract void applyToBuilding (Building _building);
}
