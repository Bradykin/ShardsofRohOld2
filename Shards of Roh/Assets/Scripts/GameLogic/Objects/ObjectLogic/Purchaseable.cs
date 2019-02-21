using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public abstract class Purchaseable {

	//Variables that must be declared in subclass
	public string name { get; protected set; }
	public string race { get; protected set; }
	public Player owner { get; set; }
	public Resource cost { get; protected set; }
	public AIInterpret interpret { get; protected set; }


	protected void purchaseSetup () {
		interpret = new AIInterpret ();
	}
}

