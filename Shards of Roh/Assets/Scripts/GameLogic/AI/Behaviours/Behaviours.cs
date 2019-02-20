using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Behaviours class is a template for creating micro-AI behaviour for units
//Behaviours are identified by name, and can be toggled active or inactive. Inactive ones are frequently removed from the behaviour list.
//Behaviours are broken down into types based on their behaviourType - some types of behaviours cannot coexist with others of that type, and adding one removes the other. All other behaviours can coexist.
//Behaviours store the UnitContainer of the unit they modify the actions of, so they have access.
//Behaviours are activated by the UnitContainer calling it's Enact function
//UnitContainer and Behaviour both have access to each other - i'm not the most proud of that data system, but because behaviours never swap between UnitContainers, it's not a big issue. Should probably come up with another system though.

//Types of Behaviours:
//Idle - Behaviours intended to have a unit carry on a current action when their target is lost. For example: If building a building and you complete that building, the unit should move to build a nearby building if one is available. Idle behaviours are temporary and disable themselves after failing to locate a target for a few seconds.
//Passive - Behaviours intended to be an action that the unit will take when sighting nearby targets and aren't doing anything. Similar to Idle, except it does not disable itself after a period of time, and is intended as a permanent behaviour instead of a temporary one.
//Hit - Behaviours that specifically handle when a unit is attacked. I should probably make these passive behaviours, they function similarly.

public abstract class Behaviours {

	public string name { get; protected set; }
	public bool active { get; set; }
	public string behaviourType { get; protected set; }
	protected UnitContainer unitInfo { get; set; }

	public abstract void enact ();
}
