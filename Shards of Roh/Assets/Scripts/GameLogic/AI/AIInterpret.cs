using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class AIInterpret {

	//Floats valued between 0 and 100 that denote the intended purpose of the object

	//Values set manually

	//Values set automatically
	public float combatValue { get; protected set; } // If unit, determined based on a formula based on their combat statistics and reduced by being high resource cost. 0 if a building. If research, ??????
	public float economicValue { get; protected set; } // 0 if a building. If unit, determined based on a formula that sets the economicValue very high if they are a worker, and lowers it slightly if they have a high resource cost or have a high combat value. If a research, ??????
	public float scoutingValue { get; protected set; } // 0 if not a unit. If unit, determined based on a formula where the value is increased based on the units movement speed, being low resource cost, and being low combat value
}

