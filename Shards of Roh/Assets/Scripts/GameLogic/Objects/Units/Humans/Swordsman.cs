using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swordsman : Unit {

	public Swordsman (Player _owner) {
		
		//Variables from inherited class
		name = "Swordsman";
		race = "Humans";
		owner = _owner;
		health = 100;
		cost = new Resource (0, 0, 50);

		unitSetup ();

		//Variables from current class
		attack = 10;
		attackRange = 1.0f;
		attackSpeed = 1.333f;
		damageCheck = 0.5f;
		moveSpeed = 7;
		populationCost = 1;
		sightRadius = 15;
		isVillager = false;
		queueTime = 5.0f;
		batchSize = 5;
	}
}
