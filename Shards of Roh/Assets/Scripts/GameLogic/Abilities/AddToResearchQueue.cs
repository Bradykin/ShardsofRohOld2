using System.Collections;
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

	//If you have the required resources and prerequisites for this research, add it to the queue of the source building
	public override void enact (Player owner, Unit targetUnit = null, Building targetBuilding = null, Vector3 targetPos = new Vector3 ()) {
		Research newResearch = ResearchFactory.createResearchByName (researchName, owner);
		if (source.isBuilt) {
			if (owner.hasResearch (newResearch.name) == false) {
				if (owner.resource.hasEnough (newResearch.cost)) {
					bool isQueued = false;
					foreach (var r in owner.buildings) {
						foreach (var u in r.building.researchQueue) {
							if (u.research.name == newResearch.name) {
								isQueued = true;
							}
						}
					}
					if (isQueued == false) {
						bool hasResearch = true;
						foreach (var r in newResearch.neededResearch) {
							if (owner.hasResearch (r) == false) {
								hasResearch = false;
							}
						}
						if (hasResearch == true) {
							owner.resource.spend (newResearch.cost);
							source.researchQueue.Add (new ResearchQueue (newResearch));
						}
					}
				}
			}
		}
	}
}
