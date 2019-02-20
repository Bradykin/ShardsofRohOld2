using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatCodes : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static void inputCheatCode (string _input) {
		GameManager.print (_input);
		string input1 = "";
		string input2 = "";
		try {
			input1 = _input.Substring (0, _input.IndexOf (" "));
			input2 = _input.Substring (_input.IndexOf (" ") + 1);
		} catch {
		}
		
		if (input1 == "Focus") {
			if (GameManager.findPlayer (input2) != null) {
				ResourceDisplay.playerSetting = input2;
			}
		}

		if (_input == "Destruction") {
			GameManager.print ("DESTROY DESTROY DESTROY");
		}
	}
}
