using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Unit {

	public Archer (Player _owner) {
		unitSetup ();

		//Variables from inherited class
		name = "Archer";
		race = "Humans";
		owner = _owner;
		health = 100;
		cost = new Resource (0, 0, 50);

		//Variables from current class
		attack = 10;
		attackRange = 20.0f;
		attackSpeed = 0.75f;
		damageCheck = 0.5f;
		moveSpeed = 7;
		populationCost = 1;
		sightRadius = 15;
		isVillager = false;
		queueTime = 5.0f;
		batchSize = 5;
	}
}
