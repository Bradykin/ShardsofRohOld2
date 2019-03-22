using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public abstract class Ability {

	public string name { get; protected set; }
	protected TargetType targetType = TargetType.None;

	public abstract void enact (Player owner, Unit targetUnit = null, Building targetBuilding = null, Vector3 targetPos = new Vector3 ());

	public string getName () {
		return name;
	}

	public TargetType getTargetType () {
		return targetType;
	}
}
