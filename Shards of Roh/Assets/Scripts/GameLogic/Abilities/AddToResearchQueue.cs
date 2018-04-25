﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class AddToResearchQueue : Ability {

	private string researchName;
	private Building source;

	public AddToResearchQueue (string _researchName, Building _source) {
		researchName = _researchName;
		source = _source;
		name = "Research " + _researchName;
		targetType = TargetType.None;
	}

	//If you have the required resources and required research for this research, add it to the queue of the source building
	public override void enact (Player owner, Unit targetUnit = null, Building targetBuilding = null, Vector3 targetPos = new Vector3 ()) {
		Research newResearch = ResearchFactory.createResearchByName (researchName, owner);
		if (source.getIsBuilt ()) {
			if (owner.hasResearch (newResearch) == false) {
				if (owner.getResource ().hasEnough (newResearch.getCost ())) {
					bool isQueued = false;
					foreach (var r in owner.getBuildings ()) {
						foreach (var u in r.getBuilding ().getResearchQueue ()) {
							if (u.getResearch ().getName () == newResearch.getName ()) {
								isQueued = true;
							}
						}
					}
					if (isQueued == false) {
						bool hasResearch = true;
						foreach (var r in newResearch.getNeededResearch ()) {
							if (owner.hasResearch (r) == false) {
								hasResearch = false;
							}
						}
						if (hasResearch == true) {
							owner.getResource ().spend (newResearch.getCost ());
							source.addToResearchQueue (newResearch);
						}
					}
				}
			}
		}
	}
}