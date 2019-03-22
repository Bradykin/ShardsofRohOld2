using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class AITrait
{
	public string target { get; private set; }
	public float modifier { get; private set; }

	public AITrait (string _target, float _modifier) {
		target = _target;
		modifier = _modifier;
	}
}


