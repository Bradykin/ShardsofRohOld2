using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class SpendingPrioritizer
{
	AIController ai;

	public float combatPriority { get; private set; }
	public float economicPriority { get; private set; }

	public SpendingPrioritizer (AIController _AI) {
		ai = _AI;
		combatPriority = 0.5f;
		economicPriority = 0.5f;
	}
}

