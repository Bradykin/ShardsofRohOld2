using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour {

	public Player player { get; set; }
	public List<Strategies> strategies { get; protected set; }

	//Variables that denote and track the AI goals
	public List<ObjectBase> objectCreationPriorities { get; protected set; }
	public Resource resourcePriorities { get; protected set; }
	public float buildPriorities;

	// Use this for initialization
	void Start () {
		objectCreationPriorities = new List<ObjectBase> ();
		strategies = new List<Strategies> ();
		if (player.name != "Nature") {
			strategies.Add (new GatherResources (this));
			strategies.Add (new CreateObjects (this));
		}

		resourcePriorities = new Resource (0, 0, 0);

		Unit newUnit0 = ObjectFactory.createUnitByName ("Worker", player);
		Unit newUnit1 = ObjectFactory.createUnitByName ("Worker", player);
		Unit newUnit2 = ObjectFactory.createUnitByName ("Worker", player);
		Unit newUnit3 = ObjectFactory.createUnitByName ("Worker", player);

		objectCreationPriorities.Add (newUnit0);
		objectCreationPriorities.Add (newUnit1);
		objectCreationPriorities.Add (newUnit2);
		objectCreationPriorities.Add (newUnit3);

		Building newBuilding0 = ObjectFactory.createBuildingByName ("Stables", player, false);
		Building newBuilding1 = ObjectFactory.createBuildingByName ("Stables", player, false);
		Building newBuilding2 = ObjectFactory.createBuildingByName ("Barracks", player, false);
		Building newBuilding3 = ObjectFactory.createBuildingByName ("Barracks", player, false);
		Building newBuilding4 = ObjectFactory.createBuildingByName ("Barracks", player, false);

		objectCreationPriorities.Add (newBuilding0);
		objectCreationPriorities.Add (newBuilding1);
		objectCreationPriorities.Add (newBuilding2);
		objectCreationPriorities.Add (newBuilding3);
		objectCreationPriorities.Add (newBuilding4);

		Unit newUnit4 = ObjectFactory.createUnitByName ("Spearman", player);
		Unit newUnit5 = ObjectFactory.createUnitByName ("Spearman", player);
		Unit newUnit6 = ObjectFactory.createUnitByName ("Swordsman", player);

		objectCreationPriorities.Add (newUnit4);
		objectCreationPriorities.Add (newUnit5);
		objectCreationPriorities.Add (newUnit6);

		//Research newResearch = ResearchFactory.createResearchByName ("Age2");

		calculateResourcePriorities ();
	}

	// Update is called once per frame
	void Update () {
		calculateResourcePriorities ();

		checkStrategyLogic ();

		player.update ();
	}

	private void calculateResourcePriorities () {
		resourcePriorities.spend (resourcePriorities);
		foreach (var r in objectCreationPriorities) {
			resourcePriorities.add (r.cost);
		}

		buildPriorities = 0;
		foreach (var r in player.buildings) {
			if (r.building.isBuilt == false) {
				buildPriorities += r.building.health;
			}
		}
	}

	private void checkStrategyLogic () {
		for (int i = 0; i < strategies.Count; i++) {
			if (strategies [i].active == false) {
				strategies.RemoveAt (i);
				i--;
			} else {
				strategies [i].enact ();
			}
		}
	}
}
