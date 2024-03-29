﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectFactory {

	public static Worker createWorker (Player _owner) {
		Worker unit = new Worker (_owner);
		unit.initPostCreate ();
		return unit;
	}

	public static Scout createScout (Player _owner) {
		Scout unit = new Scout (_owner);
		unit.initPostCreate ();
		return unit;
	}

	public static Axeman createAxeman (Player _owner) {
		Axeman unit = new Axeman (_owner);
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

	public static Mage createMage (Player _owner) {
		Mage unit = new Mage (_owner);
		unit.initPostCreate ();
		return unit;
	}

	public static MageCavalry createMageCavalry (Player _owner) {
		MageCavalry unit = new MageCavalry (_owner);
		unit.initPostCreate ();
		return unit;
	}

	public static Priest createPriest (Player _owner) {
		Priest unit = new Priest (_owner);
		unit.initPostCreate ();
		return unit;
	}

	public static PriestCavalry createPriestCavalry (Player _owner) {
		PriestCavalry unit = new PriestCavalry (_owner);
		unit.initPostCreate ();
		return unit;
	}

	public static Catapult createCatapult (Player _owner) {
		Catapult unit = new Catapult (_owner);
		unit.initPostCreate ();
		return unit;
	}





	public static TownCentre createTownCentre (Player _owner, bool _isBuilt) {
		TownCentre building = new TownCentre (_owner);
		building.initPostCreate (_isBuilt);
		return building;
	}

	public static Windmill createWindmill (Player _owner, bool _isBuilt) {
		Windmill building = new Windmill (_owner);
		building.initPostCreate (_isBuilt);
		return building;
	}

	public static Wall createWall (Player _owner, bool _isBuilt) {
		Wall building = new Wall (_owner);
		building.initPostCreate (_isBuilt);
		return building;
	}

	public static House createHouse (Player _owner,  bool _isBuilt) {
		House building = new House (_owner);
		building.initPostCreate (_isBuilt);
		return building;
	}

	public static Barracks createBarracks (Player _owner, bool _isBuilt) {
		Barracks building = new Barracks (_owner);
		building.initPostCreate (_isBuilt);
		return building;
	}

	public static Stables createStables (Player _owner, bool _isBuilt) {
		Stables building = new Stables (_owner);
		building.initPostCreate (_isBuilt);
		return building;
	}

	public static WatchTower createWatchTower (Player _owner, bool _isBuilt) {
		WatchTower building = new WatchTower (_owner);
		building.initPostCreate (_isBuilt);
		return building;
	}

	public static Blacksmith createBlacksmith (Player _owner, bool _isBuilt) {
		Blacksmith building = new Blacksmith (_owner);
		building.initPostCreate (_isBuilt);
		return building;
	}

	public static Tavern createTavern (Player _owner, bool _isBuilt) {
		Tavern building = new Tavern (_owner);
		building.initPostCreate (_isBuilt);
		return building;
	}

	public static Fort createFort (Player _owner, bool _isBuilt) {
		Fort building = new Fort (_owner);
		building.initPostCreate (_isBuilt);
		return building;
	}

	public static Cathedral createCathedral (Player _owner, bool _isBuilt) {
		Cathedral building = new Cathedral (_owner);
		building.initPostCreate (_isBuilt);
		return building;
	}

	public static CityHall createCityHall (Player _owner, bool _isBuilt) {
		CityHall building = new CityHall (_owner);
		building.initPostCreate (_isBuilt);
		return building;
	}




	public static Food createFood (Player _owner, bool _isBuilt) {
		Food building = new Food (_owner);
		building.initPostCreate (_isBuilt);
		return building;
	}

	public static Wood createWood (Player _owner, bool _isBuilt) {
		Wood building = new Wood (_owner);
		building.initPostCreate (_isBuilt);
		return building;
	}

	public static Gold createGold (Player _owner, bool _isBuilt) {
		Gold building = new Gold (_owner);
		building.initPostCreate (_isBuilt);
		return building;
	}

	public static Metal createMetal (Player _owner, bool _isBuilt) {
		Metal building = new Metal (_owner);
		building.initPostCreate (_isBuilt);
		return building;
	}

	public static Unit createUnitByName (string _name, Player _owner) {
		if (_name == "Worker" || _name == "Worker(Clone)") {
			return createWorker (_owner);
		} else if (_name == "Scout" || _name == "Scout(Clone)") {
			return createScout (_owner);
		} else if (_name == "Axeman" || _name == "Axeman(Clone)") {
			return createAxeman (_owner);
		} else if (_name == "Spearman" || _name == "Spearman(Clone)") {
			return createSpearman (_owner);
		} else if (_name == "Swordsman" || _name == "Swordsman(Clone)") {
			return createSwordsman (_owner);
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
		} else if (_name == "Mage" || _name == "Mage(Clone)") {
			return createMage (_owner);
		} else if (_name == "MageCavalry" || _name == "MageCavalry(Clone)") {
			return createMageCavalry (_owner);
		} else if (_name == "Priest" || _name == "Priest(Clone)") {
			return createPriest (_owner);
		} else if (_name == "PriestCavalry" || _name == "PriestCavalry(Clone)") {
			return createPriestCavalry (_owner);
		} else if (_name == "Catapult" || _name == "Catapult(Clone)") {
			return createCatapult (_owner);
		}

		GameManager.print ("Fail to find name - ObjectFactory");
		return createArcher (_owner);
	}

	public static Building createBuildingByName (string _name, Player _owner, bool _isBuilt = true) {
		if (_name == "TownCentre" || _name == "TownCentre(Clone)") {
			return createTownCentre (_owner, _isBuilt);
		} else if (_name == "Windmill" || _name == "Windmill(Clone)") {
			return createWindmill (_owner, _isBuilt);
		} else if (_name == "Wall" || _name == "Wall(Clone)") {
			return createWall (_owner, _isBuilt);
		} else if (_name == "House" || _name == "House(Clone)") {
			return createHouse (_owner, _isBuilt);
		} else if (_name == "Barracks" || _name == "Barracks(Clone)") {
			return createBarracks (_owner, _isBuilt);
		} else if (_name == "Stables" || _name == "Stables(Clone)") {
			return createStables (_owner, _isBuilt);
		} else if (_name == "WatchTower" || _name == "WatchTower(Clone)") {
			return createWatchTower (_owner, _isBuilt);
		} else if (_name == "Blacksmith" || _name == "Blacksmith(Clone)") {
			return createBlacksmith (_owner, _isBuilt);
		} else if (_name == "Tavern" || _name == "Tavern(Clone)") {
			return createTavern (_owner, _isBuilt);
		} else if (_name == "Fort" || _name == "Fort(Clone)") {
			return createFort (_owner, _isBuilt);
		} else if (_name == "Cathedral" || _name == "Cathedral(Clone)") {
			return createCathedral (_owner, _isBuilt);
		} else if (_name == "CityHall" || _name == "CityHall(Clone)") {
			return createCityHall (_owner, _isBuilt);
		
		} else if (_name == "Food" || _name == "Food(Clone)") {
			return createFood (_owner, _isBuilt);
		} else if (_name == "Wood" || _name == "Wood(Clone)") {
			return createWood (_owner, _isBuilt);
		} else if (_name == "Gold" || _name == "Gold(Clone)") {
			return createGold (_owner, _isBuilt);
		} else if (_name == "Metal" || _name == "Metal(Clone)") {
			return createMetal (_owner, _isBuilt);
		}

		GameManager.print ("Fail to find name - ObjectFactory: " + _name);
		return createWatchTower (_owner, _isBuilt);
	}
}
