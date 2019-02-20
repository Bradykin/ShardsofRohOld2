using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MinimapLogic : MonoBehaviour, IPointerClickHandler {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnPointerClick (PointerEventData eventData) {
		if (eventData.button == PointerEventData.InputButton.Left) {
			BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D> ();
			Vector2 actDist = MouseController.getMousePosition () - new Vector2 (collider.bounds.center.x, collider.bounds.center.y);
			Vector3 bounds = collider.bounds.max - collider.bounds.center;
			CameraController.moveToMinimapLocation (new Vector2 ((actDist.x / bounds.x) * 250.0f, (actDist.y / bounds.y) * 250.0f));
		} else if (eventData.button == PointerEventData.InputButton.Right) {
			//Check if rotating camera
			if (!Input.GetKey (KeyCode.LeftControl)) {
				if (GameManager.playerContainer.player.curUnitTarget.Count > 0) {
					BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D> ();
					Vector2 actDist = MouseController.getMousePosition () - new Vector2 (collider.bounds.center.x, collider.bounds.center.y);
					Vector3 bounds = collider.bounds.max - collider.bounds.center;
						
					Vector2 _location = new Vector2 ((actDist.x / bounds.x) * 250.0f, (actDist.y / bounds.y) * 250.0f);

					Vector3 destination = new Vector3 (_location.x, Terrain.activeTerrain.SampleHeight (new Vector3 (_location.x, Camera.main.transform.position.y, _location.y)), _location.y);
					GameManager.playerContainer.processRightClickUnitCommand (destination, Terrain.activeTerrain.gameObject, Input.GetKey (KeyCode.LeftShift));
					}
			} else if (GameManager.playerContainer.player.curBuildingTarget.Count > 0) {

			} else {

			}
		}
	}
}
