using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Behaviours {

	public string name { get; protected set; }
	public bool active { get; set; }

	public abstract void enact(UnitContainer unitInfo);
}
