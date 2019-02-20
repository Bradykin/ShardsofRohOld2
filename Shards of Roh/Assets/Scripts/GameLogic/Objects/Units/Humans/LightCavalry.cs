using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class LightCavalry : Unit {

	public LightCavalry (Player _owner) {

		//Variables from inherited class
		name = "LightCavalry";
		race = "Humans";
		owner = _owner;
		health = 100;
		cost = new Resource (0, 0, 50, 0);

		unitSetup ();

		//Variables from current class
		attack = 10;
		attackRange = 1.0f;
		attackSpeed = 1.0f;
		damageCheck = 0.5f;
		moveSpeed = 11;
		populationCost = 1;
		sightRadius = 15;
		unitType = UnitType.Cavalry;
		queueTime = 5.0f;
		batchSize = 5;
	}
}
