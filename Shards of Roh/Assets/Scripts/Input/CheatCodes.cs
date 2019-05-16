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
		GameManager.print ("Code entered: " + _input);
		string input1 = "";
		string input2 = "";
		try {
			input1 = _input.Substring (0, _input.IndexOf (" "));
			input2 = _input.Substring (_input.IndexOf (" ") + 1);
		} catch {
		}

		if (input1 == "EnableLogging") {
			if (input2 == "GatherResources" || input2 == "All") {
				GameManager.logging.gatherResources = true;
			}
			if (input2 == "ObjectPlannerValues" || input2 == "All") {
				GameManager.logging.objectPlannerValues = true;
			}
			if (input2 == "ObjectPlannerResults" || input2 == "All") {
				GameManager.logging.objectPlannerResults = true;
			}
			if (input2 == "CreateNewObject" || input2 == "All") {
				GameManager.logging.createNewObject = true;
			}
		}

		if (input1 == "DisableLogging") {
			if (input2 == "GatherResources" || input2 == "All") {
				GameManager.logging.gatherResources = false;
			}
			if (input2 == "ObjectPlannerValues" || input2 == "All") {
				GameManager.logging.objectPlannerValues = false;
			}
			if (input2 == "ObjectPlannerResults" || input2 == "All") {
				GameManager.logging.objectPlannerResults = false;
			}
			if (input2 == "CreateNewObject" || input2 == "All") {
				GameManager.logging.createNewObject = false;
			}
		}
		
		if (input1 == "Focus") {
			if (GameManager.findPlayer (input2) != null) {
				ResourceDisplay.setPlayerSetting (input2);
			}
		}

		if (_input == "Destruction") {
			GameManager.print ("DESTROY DESTROY DESTROY");
		}
	}
}
