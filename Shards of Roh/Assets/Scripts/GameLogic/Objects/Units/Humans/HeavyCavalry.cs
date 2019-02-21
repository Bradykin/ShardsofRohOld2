using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class HeavyCavalry : Unit {

	public HeavyCavalry (Player _owner) {
		
		//Variables from inherited class
		name = "HeavyCavalry";
		race = "Humans";
		owner = _owner;
		health = 75;
		cost = new Resource (40, 70, 0, 0);

		unitSetup ();

		//Variables from current class
		attack = 8;
		attackRange = 1.0f;
		attackSpeed = 0.75f;
		damageCheck = 0.5f;
		moveSpeed = 11;
		populationCost = 1;
		sightRadius = 30;
		unitType = UnitType.Cavalry;
		attackType = AttackType.Bludgeoning;
		queueTime = 5.0f;
		batchSize = 5;

		//Armour Types - value ranges from 0 to 100. 100 reduces all damage taken, 0 has no effect.
		armourSlashing = 0;
		armourPiercing = 0;
		armourBludgeoning = 0;
		armourRanged = 0;
		armourSiege = 0;
		armourMagic = 0;

		//Abilities

		//Required Research
		neededResearch.Add (ResearchFactory.createResearchByName ("Age2", _owner));
	}
}
