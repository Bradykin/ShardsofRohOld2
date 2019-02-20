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

	//


	//AI related Enums to denote object/research purpose

	//Denotes general object purpose
	public enum GeneralPurpose {Combat, Economic, Scouting}

	//Denotes whether the object is best for immediate or long term results
	public enum ScalingPurpose {General, Immediate, LongTerm}

	//Denotes Combat Purpose
	public enum CombatPurpose {None, General, Offensive, Defensive}

	//Denotes Combat Countering Purpose
	public enum CombatCounterPurpose {None, Villager, Infantry, Cavalry, Slashing, Piercing, Bludgeoning, Ranged, Siege, Magic, Building}

	//Denotes Economic Purpose
	public enum EconomicPurpose {None, General, Food, Animal, Forage, Farm, Wood, Gold, Metal}


}
