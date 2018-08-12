using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateObjects : Strategies {

	public CreateObjects (AIController _AI) {
		name = "CreateObjects";
		active = true;
		AI = _AI; 
	}

	public override void enact () {
		if (AI.objectCreationPriorities.Count > 0) {
			if (AI.player.resource.hasEnough (AI.objectCreationPriorities [0].cost)) {
				if (AI.objectCreationPriorities [0] is Building) {
					if (AI.player.createBuildingFoundation (AI.objectCreationPriorities [0].name, chooseBuildingLocation ()) == true) {
						AI.objectCreationPriorities.RemoveAt (0);
					}
				} else if (AI.objectCreationPriorities [0] is Unit) {

				} else {
					GameManager.print ("WRONG");
				}
			}
		}
	}

	public Vector3 chooseBuildingLocation () {
		List<Vector3> positionsAround = positionsAroundBuilding ();

		return positionsAround [Random.Range (0, positionsAround.Count - 1)];
	}

	public List<Vector3> positionsAroundBuilding () {
		List<Vector3> positionsAround = new List<Vector3> ();

		foreach (var r in AI.player.buildings) {
			for (int i = 0; i < 360; i += 15) {
				Vector3 pos = new Vector3 ();
				float dist = 20.0f;
				float angle = i; //degrees
				float a = angle * Mathf.PI / 180f;
				pos.x = Mathf.Sin (a) * dist + r.building.curLoc.x;
				pos.z = Mathf.Cos (a) * dist + r.building.curLoc.z;
				pos.y = Terrain.activeTerrain.SampleHeight (pos);
				positionsAround.Add (pos);
			}
		}

		return positionsAround;
	}
}
