using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class VillagerAllocation {
	public int food;
	public int wood;
	public int gold;
	public int metal;
	public int build;

	public VillagerAllocation () {
		clearAllocation ();
	}

	public void clearAllocation () {
		food = 0;
		wood = 0;
		gold = 0;
		metal = 0;
		build = 0;
	}

	public void printAllocation () {
		GameManager.print ("FOOD: " + food  + " WOOD: " + wood + " GOLD: " + gold + " METAL: " + metal + " BUILD: " + build);
	}
}

