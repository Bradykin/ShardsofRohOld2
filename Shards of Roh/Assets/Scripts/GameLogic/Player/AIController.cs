using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour {

	public Player player { get; set; }
	public List<Strategies> strategies { get; protected set; }

	//Variables that denote and track the AI goals
	public List<AIQueue> creationQueue { get; protected set; }

	public Vector3 constructionPriorities { get; set; }
	public Vector3 commandingPriorities { get; set; } 
	public Resource resourcePriorities { get; protected set; }
	public float buildPriorities;

	// Use this for initialization
	void Start () {
		creationQueue = new List<AIQueue> ();
		strategies = new List<Strategies> ();
		if (player.name != "Nature") {
			//	strategies.Add (new StrategyPriorityCalculator (this));
			strategies.Add (new ObjectQueuePlanner (this));
			strategies.Add (new GatherResources (this));
			strategies.Add (new CreateObjects (this));
		}

		resourcePriorities = new Resource (0, 0, 0, 0);

		creationQueue.Add (new AIQueue ("Research", null, ResearchFactory.createResearchByName ("Forestry", player)));
		creationQueue.Add (new AIQueue ("Unit", ObjectFactory.createUnitByName ("Worker", player)));
		creationQueue.Add (new AIQueue ("Unit", ObjectFactory.createUnitByName ("Worker", player)));
		creationQueue.Add (new AIQueue ("Unit", ObjectFactory.createUnitByName ("Worker", player)));

		creationQueue.Add (new AIQueue ("Research", null, ResearchFactory.createResearchByName ("Age2", player)));

		creationQueue.Add (new AIQueue ("Building", ObjectFactory.createBuildingByName ("Barracks", player)));

		creationQueue.Add (new AIQueue ("Unit", ObjectFactory.createUnitByName ("Spearman", player)));
		creationQueue.Add (new AIQueue ("Unit", ObjectFactory.createUnitByName ("Spearman", player)));
		creationQueue.Add (new AIQueue ("Unit", ObjectFactory.createUnitByName ("Spearman", player)));
		creationQueue.Add (new AIQueue ("Unit", ObjectFactory.createUnitByName ("Spearman", player)));

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
			if (i == 0) {
				for (int p = 0; p < cap; p++) {
					resourcePriorities.add (creationQueue [i].getCost ().getNormalized ());
				}
			}
			resourcePriorities.add (creationQueue [i].getCost ().getNormalized ());
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
