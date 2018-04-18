using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMaterial : MonoBehaviour {

	//Get material value for the team's units
	public static string getMaterial (string _playerName) {
		string returnValue = "Materials/WK_Standard_Units";
		switch (_playerName) {
			case "Player":
				returnValue = "Materials/WK_Standard_Units_blue";
				break;
			case "Enemy1":
				returnValue = "Materials/WK_Standard_Units_red";
				break;
			case "Enemy2":
				returnValue = "Materials/WK_Standard_Units_green";
				break;
			case "Enemy3":
				returnValue = "Materials/WK_Standard_Units_white";
				break;
			}

		if (returnValue == "Materials/WK_Standard_Units") {
			print ("Missing texture for player: " + _playerName);
		}
		return returnValue;
	}
}
