using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public abstract class Research {

	protected string name;
	protected Resource cost;
	public float queueTime { get; protected set; }
	protected List<Research> neededResearch = new List<Research> ();

	public string getName () {
		return name;
	}

	public Resource getCost () {
		return cost;
	}

	public List<Research> getNeededResearch () {
		return neededResearch;
	}
}
