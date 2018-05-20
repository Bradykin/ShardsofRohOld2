using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour {

	public Player player { get; set; }
	public List<Strategies> strategies { get; protected set; }

	//Variables that denote and track the AI goals
	public List<Object> objectCreationPriorities { get; protected set; }
	public Resource resourcePriorities { get; protected set; }

	// Use this for initialization
	void Start () {
		objectCreationPriorities = new List<Object> ();
		strategies = new List<Strategies> ();
		if (player.name != "Nature") {
			strategies.Add (new GatherResources (this));
			strategies.Add (new CreateObjects (this));
		}

		resourcePriorities = new Resource (0, 0, 0);

		Building newBuilding0 = ObjectFactory.createBuildingByName ("Stables", player, false);
		Building newBuilding1 = ObjectFactory.createBuildingByName ("Stables", player, false);
		Building newBuilding2 = ObjectFactory.createBuildingByName ("Stables", player, false);
		Building newBuilding3 = ObjectFactory.createBuildingByName ("Stables", player, false);
		Building newBuilding4 = ObjectFactory.createBuildingByName ("Stables", player, false);
		Building newBuilding5 = ObjectFactory.createBuildingByName ("Stables", player, false);
		Building newBuilding6 = ObjectFactory.createBuildingByName ("Stables", player, false);
		Building newBuilding7 = ObjectFactory.createBuildingByName ("Stables", player, false);
		Building newBuilding8 = ObjectFactory.createBuildingByName ("Stables", player, false);
		Building newBuilding9 = ObjectFactory.createBuildingByName ("Stables", player, false);

		objectCreationPriorities.Add (newBuilding0);
		objectCreationPriorities.Add (newBuilding1);
		objectCreationPriorities.Add (newBuilding2);
		objectCreationPriorities.Add (newBuilding3);
		objectCreationPriorities.Add (newBuilding4);
		objectCreationPriorities.Add (newBuilding5);
		objectCreationPriorities.Add (newBuilding6);
		objectCreationPriorities.Add (newBuilding7);
		objectCreationPriorities.Add (newBuilding8);
		objectCreationPriorities.Add (newBuilding9);

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
