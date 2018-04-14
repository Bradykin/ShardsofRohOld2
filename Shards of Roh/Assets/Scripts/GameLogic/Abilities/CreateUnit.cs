using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class CreateUnit : Ability {

	private string unitName;
	private Building source;

	public CreateUnit (string _unitName, Building _source) {
		unitName = _unitName;
		source = _source;
		name = "Spawn " + _unitName;
		targetType = TargetType.None;
	}

	public override void enact (Player owner, Unit targetUnit = null, Building targetBuilding = null, Vector3 targetPos = new Vector3 ()) {
		Vector3 targetLoc = source.getCurLoc ();
		targetLoc.x += (source.getColliderSize ().x / 2);
		targetLoc.z -= (source.getColliderSize ().z / 2);
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Camera.main.WorldToScreenPoint (targetLoc));
		if (Physics.Raycast (ray, out hit, 1000)) {
			Unit newUnit = ObjectFactory.createUnitByName (unitName, GameManager.addPlayerToGame (owner.getName ()));
			GameObject instance = GameManager.Instantiate (Resources.Load (newUnit.getPrefabPath (), typeof(GameObject)) as GameObject);
			instance.GetComponent<UnitContainer> ().setUnit (newUnit);
			if (instance.GetComponent<UnityEngine.AI.NavMeshAgent> () != null) {
				instance.GetComponent<UnityEngine.AI.NavMeshAgent> ().Warp (hit.point);
			}

			GameManager.addPlayerToGame (owner.getName ()).addUnitToPlayer (instance.GetComponent<UnitContainer> ());
		}
	}
}
