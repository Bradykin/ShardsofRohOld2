﻿using System.Collections;
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

	public override void enact (Player owner, Unit targetUnit = null, Building targetBuilding = null, Vector3 targetPos = new Vector3 ()) {
		if (GameManager.player.buildToggleSetting == buildName) {
			GameManager.player.buildToggleActive = false;
			GameManager.player.buildToggleSetting = null;
			GameManager.player.buildToggle.transform.GetChild (0).gameObject.SetActive (false);

			for (int i = 0; i < GameManager.player.buildToggle.transform.childCount; i++) {
				GameManager.player.buildToggle.transform.GetChild (i).gameObject.SetActive (false);
			}
		} else {
			GameManager.player.buildToggleActive = true;
			GameManager.player.buildToggleSetting = buildName;
			GameManager.player.buildToggle.transform.GetChild (0).gameObject.SetActive (true);

			for (int i = 0; i < GameManager.player.buildToggle.transform.childCount; i++) {
				if (GameManager.player.buildToggle.transform.GetChild (i).gameObject.name == buildName) {
					GameManager.player.buildToggle.transform.GetChild (i).gameObject.SetActive (true);
				} else {
					GameManager.player.buildToggle.transform.GetChild (i).gameObject.SetActive (false);
				}
			}
		}
	}
}