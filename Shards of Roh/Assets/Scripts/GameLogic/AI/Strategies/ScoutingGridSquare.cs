using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class ScoutingGridSquare {

	AIController AI;
	public int squareXArray { get; protected set; }
	public int squareZArray { get; protected set; }
	public int squareXLeft { get; protected set; }
	public int squareXRight { get; protected set; }
	public int squareZLeft { get; protected set; }
	public int squareZRight { get; protected set; }
	public Vector3 squareCenter { get; protected set; }
	public Resource squareResources { get; protected set; }
	public Resource predictedSquareResources { get; protected set; }
	public float exploredValue { get; protected set; }
	public float scoutingValue { get; set; }
	public bool isTileSafe { get; set; }

	public ScoutingGridSquare (AIController _AI, Resource _averageResources, int _squareXArray, int _squareZArray, int squareSize, int mapSizeX, int mapSizeZ) {
		AI = _AI;
		squareXArray = _squareXArray;
		squareZArray = _squareZArray;
		squareXLeft = (squareXArray * squareSize) - (mapSizeX / 2);
		squareXRight = ((1 + squareXArray) * squareSize) - (mapSizeX / 2);
		squareZLeft = (squareZArray * squareSize) - (mapSizeZ / 2);
		squareZRight = ((1 + squareZArray) * squareSize) - (mapSizeZ / 2);
		squareCenter = new Vector3 ((squareXLeft + squareXRight) / 2, 0, (squareZLeft + squareZRight) / 2);
		squareResources = new Resource (0, 0, 0, 0);
		predictedSquareResources = _averageResources;
		exploredValue = 15;
		isTileSafe = true;
	}

	public void addToGrid (Purchaseable input) {
		if (input is Unit) {
			if (input.owner.name == AI.player.name) {
				if ((input as Unit).isMoving == true) {
					exploredValue -= Time.deltaTime;
					//GameManager.print (squareXArray + " " + squareZArray + " --- " + exploredValue);
				}
			}
		} else if (input is Building) {
			if ((input as Building).isResource == true) {
				squareResources.add (ObjectFactory.createBuildingByName (input.name, GameManager.addPlayerToGame ("Nature")).cost);
				//GameManager.print (input.name + " --- " + squareXArray + " " + squareZArray + " --- " + squareResources.toString());
			} else if ((input as Building).owner.name != AI.player.name) {
				GameManager.print ("Tile Not Safe: " + squareXArray + " - " + squareZArray);
				isTileSafe = false;
			}
		}
		//Use the input to tick up tracking variables to track what was in this area. Amount of wood, amount of gold, presence of enemies, etc. Keep a set of ints remembering these, so the AI can determine the best places to look
		//Combine above with a variable tracking the amount of time the AI has had units moving in this area, so it can determine how thoroughly it hhas explored the space.
	}

	public float explorationRemaining () {
		//Return estimate of how thoroughly the square has been explored. 1 = all, 0 = none

		return Mathf.Max (0, exploredValue);
	}
}


