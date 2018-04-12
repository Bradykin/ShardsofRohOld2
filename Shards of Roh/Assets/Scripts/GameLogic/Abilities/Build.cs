using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class Build : Ability {

	private string buildName;

	public Build (string _buildName) {
		buildName = _buildName;
		name = "Build " + _buildName;
		targetType = TargetType.None;
	}

	public override void enact (Player owner, Unit targetUnit = null, Building targetBuilding = null, Vector3 targetPos = new Vector3 ()) {
		if (GameManager.player.buildToggleActive == true) {
			GameManager.player.buildToggleActive = false;
			GameManager.player.buildToggle.transform.GetChild (0).gameObject.SetActive (false);
		} else {
			GameManager.player.buildToggleActive = true;
			GameManager.player.buildToggle.transform.GetChild (0).gameObject.SetActive (true);
		}
	}
}
