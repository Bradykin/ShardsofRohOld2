using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFactory : MonoBehaviour {

	public static Villager createVillager (Player _owner) {
		Villager unit = new Villager (_owner);
		unit.initPostCreate ();
		return unit;
	}

	public static Spearman createSpearman (Player _owner) {
		Spearman unit = new Spearman (_owner);
		unit.initPostCreate ();
		return unit;
	}

	public static Swordsman createSwordsman (Player _owner) {
		Swordsman unit = new Swordsman (_owner);
		unit.initPostCreate ();
		return unit;
	}

	public static Axeman createAxeman (Player _owner) {
		Axeman unit = new Axeman (_owner);
		unit.initPostCreate ();
		return unit;
	}

	public static HeavyInfantry createHeavyInfantry (Player _owner) {
		HeavyInfantry unit = new HeavyInfantry (_owner);
		unit.initPostCreate ();
		return unit;
	}

	public static Archer createArcher (Player _owner) {
		Archer unit = new Archer (_owner);
		unit.initPostCreate ();
		return unit;
	}

	public static LightCavalry createLightCavalry (Player _owner) {
		LightCavalry unit = new LightCavalry (_owner);
		unit.initPostCreate ();
		return unit;
	}

	public static SpearCavalry createSpearCavalry (Player _owner) {
		SpearCavalry unit = new SpearCavalry (_owner);
		unit.initPostCreate ();
		return unit;
	}

	public static HeavyCavalry createHeavyCavalry (Player _owner) {
		HeavyCavalry unit = new HeavyCavalry (_owner);
		unit.initPostCreate ();
		return unit;
	}

	public static BowCavalry createBowCavalry (Player _owner) {
		BowCavalry unit = new BowCavalry (_owner);
		unit.initPostCreate ();
		return unit;
	}

	public static Catapult createCatapult (Player _owner) {
		Catapult unit = new Catapult (_owner);
		unit.initPostCreate ();
		return unit;
	}




	public static WatchTower createWatchTower (Player _owner, bool _isBuilt) {
		WatchTower building = new WatchTower (_owner);
		building.initPostCreate (_isBuilt);
		return building;
	}

	public static Stables createStables (Player _owner, bool _isBuilt) {
		Stables building = new Stables (_owner);
		building.initPostCreate (_isBuilt);
		return building;
	}

	public static Gold createGold (Player _owner, bool _isBuilt) {
		Gold building = new Gold (_owner);
		building.initPostCreate (_isBuilt);
		return building;
	}

	public static Unit createUnitByName (string _name, Player _owner) {
		if (_name == "Villager" || _name == "Villager(Clone)") {
			return createVillager (_owner);
		} else if (_name == "Spearman" || _name == "Spearman(Clone)") {
			return createSpearman (_owner);
		} else if (_name == "Swordsman" || _name == "Swordsman(Clone)") {
			return createSwordsman (_owner);
		} else if (_name == "Axeman" || _name == "Axeman(Clone)") {
			return createAxeman (_owner);
		} else if (_name == "HeavyInfantry" || _name == "HeavyInfantry(Clone)") {
			return createHeavyInfantry (_owner);
		} else if (_name == "Archer" || _name == "Archer(Clone)") {
			return createArcher (_owner);
		} else if (_name == "LightCavalry" || _name == "LightCavalry(Clone)") {
			return createLightCavalry (_owner);
		} else if (_name == "SpearCavalry" || _name == "SpearCavalry(Clone)") {
			return createSpearCavalry (_owner);
		} else if (_name == "HeavyCavalry" || _name == "HeavyCavalry(Clone)") {
			return createHeavyCavalry (_owner);
		} else if (_name == "BowCavalry" || _name == "BowCavalry(Clone)") {
			return createBowCavalry (_owner);
		} else if (_name == "Catapult" || _name == "Catapult(Clone)") {
			return createCatapult (_owner);
		}

		print ("Fail to find name - ObjectFactory");
		return createArcher (_owner);
	}

	public static Building createBuildingByName (string _name, Player _owner, bool _isBuilt = true) {
		if (_name == "WatchTower" || _name == "WatchTower(Clone)") {
			return createWatchTower (_owner, _isBuilt);
		} else if (_name == "Stables" || _name == "Stables(Clone)") {
			return createStables (_owner, _isBuilt);
		
		} else if (_name == "Gold" || _name == "Gold(Clone)") {
			return createGold (_owner, _isBuilt);
		}

		print ("Fail to find name - ObjectFactory: " + _name);
		return createWatchTower (_owner, _isBuilt);
	}
}
