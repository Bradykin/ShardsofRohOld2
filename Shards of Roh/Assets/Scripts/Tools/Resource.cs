using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource {
	public float food { get; private set; }
	public float wood { get; private set; }
	public float gold { get; private set; }
	public float metal { get; private set; }

	//Set value of food/wood/gold
	public Resource (float _food, float _wood, float _gold, float _metal) {
		food = _food;
		wood = _wood;
		gold = _gold;
		metal = _metal;
	}

	//Add the value of another Resource to this Resource
	public void add (Resource _add) {
		food += _add.food;
		wood += _add.wood;
		gold += _add.gold;
		metal += _add.metal;
	}

	public void add (Vector4 _add) {
		food += _add.x;
		wood += _add.y;
		gold += _add.z;
		metal += _add.w;
	}

	//Spend the value of another Resource from this Resource
	public void spend (Resource _cost) {
		food -= _cost.food;
		wood -= _cost.wood;
		gold -= _cost.gold;
		metal -= _cost.metal;

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
		if (metal < 0) {
			metal = 0;
			GameManager.print ("Overspent metal");
		}
	}

	//Check if this Resource has enough to spend the value of another Resource
	public bool hasEnough (Resource _cost) {
		if (food >= _cost.food && wood >= _cost.wood && gold >= _cost.gold && metal >= _cost.metal) {
			return true;
		} else {
			return false;
		}
	}

	public Vector4 getResources () {
		return new Vector4 (food, wood, gold, metal);
	}

	public Vector4 getNormalized () {
		float total = getTotal ();
		return new Vector4 (food * 100 / total, wood * 100 / total, gold * 100 / total, metal * 100 / total);
	}

	//Get added value of all resources
	public float getTotal () {
		return food + wood + gold + metal;
	}

	//Unused Function
	public string toString () {
		return "Food: " + food + ", Wood: " + wood + ", Gold: " + gold + ", Metal: " + metal;
	}
}