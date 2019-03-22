using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enum {

	//Direction that WASD input instructs the camera to move
	public enum CameraDirection {FrontLeft, Front, FrontRight, Right, BackRight, Back, BackLeft, Left, None}

	//Type of Unit
	public enum UnitType {None, Villager, Infantry, Cavalry, Siege}

	//AttackType
	public enum AttackType {None, Slashing, Piercing, Bludgeoning, Ranged, Siege, Magic}

	//Type of target for abilities
	public enum TargetType {None, Unit, Building, Ground}

	//Type of resource
	public enum ResourceType {None, Food, Wood, Gold, Metal, Build}

	//Type of food resource
	public enum FoodType {None, Animal, Forage, Farm}

	//Purpose of a Research Effect
	public enum ResearchPurpose {Offense, Defense, Combat, Economic, CombatEconomic}

	//Races
	public enum RaceType {None, Humans, Barbarians, Elves, Orcs, Undead, Explorer, All}
}
