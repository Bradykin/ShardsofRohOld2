using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Strategies {

	public string name { get; protected set; }
	public bool active { get; set; }
	protected AIController AI { get; set; }

	public abstract void enact();
}