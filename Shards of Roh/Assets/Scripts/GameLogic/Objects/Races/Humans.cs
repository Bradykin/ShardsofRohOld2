using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class Humans : Race {

	//Variables that must be declared in subclass

	//Variables that will default if not declared

	//Variables that adjust during gameplay

	public Humans (Player _player) {
		setup ();

		unitTypes.Add (ObjectFactory.createUnitByName ("Archer", _player));
		unitTypes.Add (ObjectFactory.createUnitByName ("Axeman", _player));
		unitTypes.Add (ObjectFactory.createUnitByName ("BowCavalry", _player));
		unitTypes.Add (ObjectFactory.createUnitByName ("Catapult", _player));
		unitTypes.Add (ObjectFactory.createUnitByName ("HeavyCavalry", _player));
		unitTypes.Add (ObjectFactory.createUnitByName ("HeavyInfantry", _player));
		unitTypes.Add (ObjectFactory.createUnitByName ("LightCavalry", _player));
		unitTypes.Add (ObjectFactory.createUnitByName ("Mage", _player));
		unitTypes.Add (ObjectFactory.createUnitByName ("MageCavalry", _player));
		unitTypes.Add (ObjectFactory.createUnitByName ("Priest", _player));
		unitTypes.Add (ObjectFactory.createUnitByName ("PriestCavalry", _player));
		unitTypes.Add (ObjectFactory.createUnitByName ("Scout", _player));
		unitTypes.Add (ObjectFactory.createUnitByName ("SpearCavalry", _player));
		unitTypes.Add (ObjectFactory.createUnitByName ("Spearman", _player));
		unitTypes.Add (ObjectFactory.createUnitByName ("Swordsman", _player));
		unitTypes.Add (ObjectFactory.createUnitByName ("Worker", _player));

		buildingTypes.Add (ObjectFactory.createBuildingByName ("Barracks", _player));
		buildingTypes.Add (ObjectFactory.createBuildingByName ("Blacksmith", _player));
		buildingTypes.Add (ObjectFactory.createBuildingByName ("Cathedral", _player));
		buildingTypes.Add (ObjectFactory.createBuildingByName ("CityHall", _player));
		buildingTypes.Add (ObjectFactory.createBuildingByName ("Fort", _player));
		buildingTypes.Add (ObjectFactory.createBuildingByName ("House", _player));
		buildingTypes.Add (ObjectFactory.createBuildingByName ("Stables", _player));
		buildingTypes.Add (ObjectFactory.createBuildingByName ("Tavern", _player));
		buildingTypes.Add (ObjectFactory.createBuildingByName ("TownCentre", _player));
		buildingTypes.Add (ObjectFactory.createBuildingByName ("Wall", _player));
		buildingTypes.Add (ObjectFactory.createBuildingByName ("WatchTower", _player));
		buildingTypes.Add (ObjectFactory.createBuildingByName ("Windmill", _player));

		researchTypes.Add (ResearchFactory.createResearchByName ("Age2", _player));
		researchTypes.Add (ResearchFactory.createResearchByName ("Age3", _player));
		researchTypes.Add (ResearchFactory.createResearchByName ("Age4", _player));
		researchTypes.Add (ResearchFactory.createResearchByName ("AnimalTracking", _player));
		researchTypes.Add (ResearchFactory.createResearchByName ("Forestry", _player));
		researchTypes.Add (ResearchFactory.createResearchByName ("Horseshoes", _player));
		researchTypes.Add (ResearchFactory.createResearchByName ("ImprovedArchers", _player));
		researchTypes.Add (ResearchFactory.createResearchByName ("ImprovedAxemen", _player));
		researchTypes.Add (ResearchFactory.createResearchByName ("ImprovedShields", _player));
		researchTypes.Add (ResearchFactory.createResearchByName ("ImprovedSpearmen", _player));
		researchTypes.Add (ResearchFactory.createResearchByName ("ImprovedSwordsmen", _player));
		researchTypes.Add (ResearchFactory.createResearchByName ("Industrialization", _player));
		researchTypes.Add (ResearchFactory.createResearchByName ("MineralExtraction", _player));
		researchTypes.Add (ResearchFactory.createResearchByName ("WorkerCoats", _player));
	}
}
