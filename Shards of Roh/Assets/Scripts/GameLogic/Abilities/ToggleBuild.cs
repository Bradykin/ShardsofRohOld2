using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class ToggleBuild : Ability {

	private string buildName;

	public ToggleBuild (string _buildName) {
		buildName = _buildName;
		name = "Build " + _buildName;
		targetType = TargetType.None;
	}

	//Handle the change of the buildToggle
	public override void enact (Player owner, Unit targetUnit = null, Building targetBuilding = null, Vector3 targetPos = new Vector3 ()) {
		if (GameManager.playerContainer.buildToggleSetting == buildName) {
			//If the buildName is equal to the current buildToggleSetting, disable it
			GameManager.playerContainer.buildToggleActive = false;
			GameManager.playerContainer.buildToggleSetting = null;

			for (int i = 0; i < GameManager.playerContainer.buildToggle.transform.childCount; i++) {
				GameManager.playerContainer.buildToggle.transform.GetChild (i).gameObject.SetActive (false);
			}
		} else {
			//If you have the required research for the buildName, set buildToggleSetting to it, otherwise disable it
			for (int i = 0; i < GameManager.playerContainer.buildToggle.transform.childCount; i++) {
				if (GameManager.playerContainer.buildToggle.transform.GetChild (i).gameObject.name == buildName) {
					bool hasResearch = true;
					foreach (var r in ObjectFactory.createBuildingByName (buildName, owner).neededResearch) {
						if (owner.hasResearch (r) == false) {
							hasResearch = false;
						}
					}
					if (hasResearch == true) {
						GameManager.playerContainer.buildToggleActive = true;
						GameManager.playerContainer.buildToggleSetting = buildName;
						GameManager.playerContainer.buildToggle.transform.GetChild (i).gameObject.SetActive (true);
					} else {
						GameManager.playerContainer.buildToggleActive = false;
						GameManager.playerContainer.buildToggleSetting = null;
						GameManager.playerContainer.buildToggle.transform.GetChild (i).gameObject.SetActive (false);
					}
				} else {
					GameManager.playerContainer.buildToggle.transform.GetChild (i).gameObject.SetActive (false);
				}
			}
		}
	}
}
