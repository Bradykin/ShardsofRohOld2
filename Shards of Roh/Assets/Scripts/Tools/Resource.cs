using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource {
	private int food = 0;
	private int wood = 0;
	private int gold = 0;

	//Set value of food/wood/gold
	public Resource (int _food, int _wood, int _gold) {
		food = _food;
		wood = _wood;
		gold = _gold;

	}

	//Add the value of another Resource to this Resource
	public void add (Resource _add) {
		food += _add.food;
		wood += _add.wood;
		gold += _add.gold;
	}

	//Spend the value of another Resource from this Resource
	public void spend (Resource _cost) {
		food -= _cost.food;
		wood -= _cost.wood;
		gold -= _cost.gold;

		if (food < 0) {
			food = 0;
			GameManager.print ("Overspent food");
		}
		if (wood < 0) {
			wood = 0;
			GameManager.print ("Overspent wood");
		}
		if (gold < 0) {
			gold = 0;
			GameManager.print ("Overspent gold");
		}
	}

	//Check if this Resource has enough to spend the value of another Resource
	public bool hasEnough (Resource _cost) {
		if (food >= _cost.food && wood >= _cost.wood && gold >= _cost.gold) {
			return true;
		} else {
			return false;
		}
	}

	public Vector3 getResources () {
		return new Vector3 (food, wood, gold);
	}

	//Get food value
	public int getFood () {
		return food;
	}

	//Get wood value
	public int getWood () {
		return wood;
	}

	//Get gold value
	public int getGold () {
		return gold;
	}

	//Unused Function
	public string toString () {
		return "Food: " + food + ", Wood: " + wood + ", Gold: " + gold;
	}
}