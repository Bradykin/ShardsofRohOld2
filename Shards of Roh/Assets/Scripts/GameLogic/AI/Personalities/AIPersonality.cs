using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class AIPersonality {
	public RaceType race { get; protected set; }
	public List<AITrait> personalityTraits { get; protected set; }

	public void setup () {
		race = RaceType.None;	
		personalityTraits = new List<AITrait> ();
	}
}

