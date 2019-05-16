using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class ScoutingGrid {

	AIController AI;
	public List<List<ScoutingGridSquare>> grid { get; protected set; }
	public int squareSize { get; protected set; }
	public int mapSizeX { get; protected set; }
	public int mapSizeZ { get; protected set; }

	public ScoutingGrid (AIController _AI) {
		AI = _AI;
		grid = new List<List<ScoutingGridSquare>> ();
		squareSize = 50;
		mapSizeX = GlobalVariables.mapSizeX;
		mapSizeZ = GlobalVariables.mapSizeZ;

		Resource totalResources = new Resource (0, 0, 0, 0);
		int numResources = 0;

		foreach (var r in GameManager.findPlayer ("Nature").buildings) {
			totalResources.add (r.building.cost);
			numResources++;
		}
		Resource averageResource = new Resource (totalResources.food / numResources, totalResources.wood / numResources, totalResources.gold / numResources, totalResources.metal / numResources);

		for (int i = 0; i < mapSizeX / squareSize; i++) {
			List<ScoutingGridSquare> row = new List<ScoutingGridSquare> ();
			for (int x = 0; x < mapSizeZ / squareSize; x++) {
				row.Add (new ScoutingGridSquare (AI, averageResource, i, x, squareSize, mapSizeX, mapSizeZ));
			}
			grid.Add (row);
		}
	}

	public ScoutingGridSquare getGridSpot (Vector3 location) {
		int locationX = (int) (location.x + (mapSizeX / 2)) / squareSize;
		int locationZ = (int) (location.z + (mapSizeZ / 2)) / squareSize;

		if (locationX < 0) {
			locationX = 0;
		}
		if (locationZ < 0) {
			locationZ = 0;
		}
		if (locationX >= (mapSizeX / squareSize)) {
			locationX = (mapSizeX / squareSize) - 1;
		}
		if (locationZ >= (mapSizeZ / squareSize)) {
			locationZ = (mapSizeZ / squareSize) - 1;
		}

		//GameManager.print (locationX + " " + locationZ);

		return grid [locationX] [locationZ];
	}
}


