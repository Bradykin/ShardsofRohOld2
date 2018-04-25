using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Behaviours {

	protected string name;

	public abstract void enact(UnitContainer unitInfo);

	public string getName()
	{
		return name;
	}
}
