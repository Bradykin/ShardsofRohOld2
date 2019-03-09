using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class ResearchEffect {

	public string researchEffectName { get; protected set; }
	public string targetObjectType { get; protected set; }
	public ResearchPurpose researchEffectPurpose { get; protected set; } 

	//Identifies the variable in the target you are comparing to, and the value you want it to be, for it to be a viable candidate for this research
	public string targetVariableIdentifier { get; protected set; } 	//Example: "name".
	public string targetVariableValue { get; protected set; } 		//Example: "Worker".
	//public string targetVariableType { get; protected set; } 		//Example: "string". This might become unnecessary

	//Identifies the variable in the target you are modifying, and the modifier you wish to use and the amount you wish to modify it by
	public string effectVariableIdentifier { get; protected set; }	//Example: "attack"
	public string effectVariableModifier { get; protected set; }	//Example: "+" Currently Supported: "+", "*"
	public float effectVariableAmount { get; protected set; }		//Example: "1.5"
	//public string effectVariableType { get; protected set; }		//Example: "float". This might become unnecessary

	public ResearchEffect (string _researchEffectName, string _targetObjectType, ResearchPurpose _researchEffectPurpose, string _targetIdentifier, string _targetValue, string _effectIdentifier, string _effectModifier, float _effectAmount) {
		researchEffectName = _researchEffectName;
		targetObjectType = _targetObjectType;
		researchEffectPurpose = _researchEffectPurpose;

		targetVariableIdentifier = _targetIdentifier;
		targetVariableValue = _targetValue;

		effectVariableIdentifier = _effectIdentifier;
		effectVariableModifier = _effectModifier;
		effectVariableAmount = _effectAmount;
	}
}

