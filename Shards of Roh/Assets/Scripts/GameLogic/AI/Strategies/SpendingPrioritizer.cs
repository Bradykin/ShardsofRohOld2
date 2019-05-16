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
		combatPriority = 0.1f;
		economicPriority = 0.9f;
	}

	public void updatePriorities () {
		float timeModification = GameManager.gameClock / 300;
		combatPriority = Mathf.Min (0.75f, 0.1f + timeModification);
		economicPriority = Mathf.Max (0.25f, 0.9f - timeModification);
		//GameManager.print (combatPriority + " - " + economicPriority);
	}
}

