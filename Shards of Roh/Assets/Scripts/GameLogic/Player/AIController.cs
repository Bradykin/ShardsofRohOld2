using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour {

	public Player player { get; set; }
	public AIPersonality personality { get; set; }
	public List<Strategies> strategies { get; protected set; }
	public ScoutingGrid scoutingGrid { get; protected set; }

	//Variables that denote and track the AI goals
	public List<Purchaseable> creationQueue { get; set; }

	public Vector3 constructionPriorities { get; set; }
	public Vector3 commandingPriorities { get; set; } 
	public Resource resourcePriorities { get; protected set; }
	public float buildPriorities;

	//These variables are inelegent tracking variables for strategies, i'd like to find a way around them
	public bool isNewBuilding;

	// Use this for initialization
	void Start () {
		creationQueue = new List<Purchaseable> ();
		strategies = new List<Strategies> ();
		personality = new AIHumanPersonality1 ();
		scoutingGrid = new ScoutingGrid (this);
		if (player.name != "Nature") {
			strategies.Add (new ObjectQueuePlanner (this));
			strategies.Add (new GatherResources (this));
			strategies.Add (new CreateObjects (this));
			//strategies.Add (new UpdateScoutingGrid (this));
			//strategies.Add (new AssignScoutsToResources (this));
		}

		resourcePriorities = new Resource (0, 0, 0, 0);
		isNewBuilding = false;

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

		//THINK OF BEST WAY TO CALCULATE THIS
		int cap = Mathf.Min (5, creationQueue.Count - 1);

		for (int i = 0; i < cap; i++) {
			resourcePriorities.add (creationQueue [i].cost);
		}

		buildPriorities = 0;
		foreach (var r in player.buildings) {
			if (r.building.isBuilt == false) {
				buildPriorities += Mathf.Max (r.building.health / 2.0f, (r.building.health - r.building.curHealth) / 1.0f);
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
