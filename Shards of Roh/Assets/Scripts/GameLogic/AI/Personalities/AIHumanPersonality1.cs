using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class AIHumanPersonality1 : AIPersonality
{
	public AIHumanPersonality1 () {
		setup ();
		race = RaceType.Humans;

		personalityTraits.Add (new AITrait ("AnimalTracking", 0.0f));
		personalityTraits.Add (new AITrait ("Swordsman", 1.5f));
		personalityTraits.Add (new AITrait ("Spearman", 0.5f));
		personalityTraits.Add (new AITrait ("LightCavalry", 1.5f));
		personalityTraits.Add (new AITrait ("Age3", 0.25f));
	}
}


