using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource {
	private int food = 0;
	private int wood = 0;
	private int gold = 0;

	public Resource (int _food = 0, int _wood = 0, int _gold = 0) {
		food = _food;
		wood = _wood;
		gold = _gold;
	}

	public void add (Resource _other) {
		food += _other.food;
		wood += _other.wood;
		gold += _other.gold;
	}

	public void add (int _food, int _wood, int _gold) {
		food += _food;
		wood += _wood;
		gold += _gold;
	}

	public void spend (Resource _other) {
		food -= _other.food;
		wood -= _other.wood;
		gold -= _other.gold;

		if (food <= 0) {
			food = 0;
		}
		if (wood <= 0) {
			wood = 0;
		}
		if (gold <= 0) {
			gold = 0;
		}
	}

	public bool hasEnough (Resource _cost) {
		if (food >= _cost.food && wood >= _cost.wood && gold >= _cost.gold) {
			return true;
		}

		return false;
	}

	public int GetFoodCount () {
		return food;
	}

	public int GetWoodCount () {
		return wood;
	}
		
	public int GetGoldCount () {
		return gold;
	}

	public string toString () {
		return "Food: " + food + ", Wood: " + wood + ", Gold: " + gold;
	}
}