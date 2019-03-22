using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Strategies class is a template for creating the larger scripts used by the AI to make decisions
//Strategies will each have different purposes, that are smaller parts of the AI processing. 
//AIController and Strategy each have access to each other - i'm not the most proud of that data system, but because each Strategy is unique and is never destroyed, it's not a big deal.
public abstract class Strategies {

	public string name { get; protected set; }
	public float interval { get; protected set; }
	public bool active { get; set; }
	protected AIController AI { get; set; }

	public abstract void enact();
}