using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class UpdateScoutingGrid : Strategies {

	public UpdateScoutingGrid (AIController _AI) {
		name = "UpdateScoutingGrid";
		active = true;
		AI = _AI;
		interval = 0.25f;
	}

	public override void enact () {
		interval += Time.deltaTime;
		if (interval >= 0.25) {
			//GameManager.print ("Check");
			interval = 0;

			foreach (var r in AI.player.visibleObjects.rememberedEnemyUnitsNew) {
				AI.scoutingGrid.getGridSpot (r.unit.curLoc).addToGrid (r.unit);
			}

			foreach (var r in AI.player.visibleObjects.rememberedEnemyBuildingsNew) {
				AI.scoutingGrid.getGridSpot (r.building.curLoc).addToGrid (r.building);
			}

			foreach (var r in AI.player.visibleObjects.rememberedResourceUnitsNew) {
				AI.scoutingGrid.getGridSpot (r.unit.curLoc).addToGrid (r.unit);
			}

			foreach (var r in AI.player.visibleObjects.rememberedResourceBuildingsNew) {
				AI.scoutingGrid.getGridSpot (r.building.curLoc).addToGrid (r.building);
			}
		}

		foreach (var r in AI.player.units) {
			AI.scoutingGrid.getGridSpot (r.unit.curLoc).addToGrid (r.unit);
		}
	}
}

