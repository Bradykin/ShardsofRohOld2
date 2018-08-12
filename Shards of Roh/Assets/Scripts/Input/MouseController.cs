using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour {

	public static Vector2 mousePosition;
	public static float zoomValue;

	private static bool isSelecting = false;
	private static Vector3 mousePosition1;
	private static LayerMask buildToggleMask;

	// Use this for initialization
	void Start () {
		mousePosition = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
		buildToggleMask = 1 << LayerMask.NameToLayer ("Terrain");
	}
	
	// Update is called once per frame
	void Update () {
		mousePosition.x = Input.mousePosition.x;
		mousePosition.y = Input.mousePosition.y;
		zoomValue = Input.GetAxis ("Mouse ScrollWheel");

		handleLeftClick ();
		handleRightClick ();
	}

	void OnGUI() {
		if (isSelecting == true) {
			var rect = SelectionBox.getScreenRect (mousePosition1, Input.mousePosition);
			SelectionBox.drawScreenRect (rect, new Color (0.8f, 0.8f, 0.95f, 0.25f));
			SelectionBox.drawScreenRectBorder (rect, 2, new Color (0.8f, 0.8f, 0.95f));
		}
	}
		
	public static void handleLeftClick () {
		//Create selection box while mouse button is held down
		if (GameManager.playerContainer.buildToggleActive == true && Input.GetMouseButtonDown (0) && EventSystem.current.IsPointerOverGameObject () == false) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (getMousePosition ());
			if (Physics.Raycast (ray, out hit, 1000, buildToggleMask)) {
				GameManager.getPlayer ().player.createBuildingFoundation (GameManager.playerContainer.buildToggleSetting, hit.point);

				GameManager.playerContainer.buildToggleActive = false;
				GameManager.playerContainer.buildToggleSetting = null;
				for (int i = 0; i < GameManager.playerContainer.buildToggle.transform.childCount; i++) {
					GameManager.playerContainer.buildToggle.transform.GetChild (i).gameObject.SetActive (false);
				}
			}
		} else {
			if (Input.GetMouseButtonDown (0) && EventSystem.current.IsPointerOverGameObject () == false) {
				isSelecting = true;
				mousePosition1 = Input.mousePosition;
				GameManager.playerContainer.player.setCurUnitTarget (new List<UnitContainer> ());
				GameManager.playerContainer.player.setCurBuildingTarget (new List<BuildingContainer> ());
			} else if (Input.GetMouseButtonUp (0)) {
				isSelecting = false;
			}

			//Check if units are inside selection box
			if (isSelecting == true) {
				if (Vector3.Distance (Camera.main.ScreenToViewportPoint (mousePosition1), Camera.main.ScreenToViewportPoint (Input.mousePosition)) > 0) {
					foreach (var r in GameManager.playerContainer.player.units) {
						if (r != null) {
							if (SelectionBox.isInBox (r.gameObject, mousePosition1)) {
								GameManager.playerContainer.player.addCurUnitTarget (r);
							} else {
								GameManager.playerContainer.player.remCurUnitTarget (r);
							}
						}
					}

					if (GameManager.playerContainer.player.curUnitTarget.Count == 0) {
						foreach (var r in GameManager.playerContainer.player.buildings) {
							if (r != null) {
								if (SelectionBox.isInBox (r.gameObject, mousePosition1)) {
									GameManager.playerContainer.player.addCurBuildingTarget (r);
								} else {
									GameManager.playerContainer.player.remCurBuildingTarget (r);
								}
							}
						}
					} else {
						GameManager.playerContainer.player.setCurBuildingTarget (new List<BuildingContainer> ());
					}
				}

				//Select unit clicked on
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay (mousePosition1);
				if (Physics.Raycast (ray, out hit, 1000, GlobalVariables.defaultMask)) {
					if (hit.collider.gameObject.GetComponent<UnitContainer> () != null) {
						if (hit.collider.gameObject.GetComponent<UnitContainer> ().unit.owner.name == "Player") {
							GameManager.playerContainer.player.addCurUnitTarget (hit.collider.gameObject.GetComponent<UnitContainer> ());
						}
					}

					if (hit.collider.gameObject.GetComponent<BuildingContainer> () != null && GameManager.playerContainer.player.curUnitTarget.Count == 0) {
						if (hit.collider.gameObject.GetComponent<BuildingContainer> ().building.owner.name == "Player") {
							GameManager.playerContainer.player.addCurBuildingTarget (hit.collider.gameObject.GetComponent<BuildingContainer> ());
						}
					}
				}
			}
		}
	}

	public static void handleRightClick () {
		//Check if clicked on minimap or UI
		//Move currently selected units to clicked location
		if (Input.GetMouseButtonDown (1)) {
			//Check if rotating camera
			if (!Input.GetKey (KeyCode.LeftControl)) {
				//Process for selecting Units
				if (GameManager.playerContainer.player.curUnitTarget.Count > 0) {
					RaycastHit hit;
					Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
					if (Physics.Raycast (ray, out hit, 1000, GlobalVariables.defaultMask)) {
						Vector3 targetLoc = hit.point;
						GameObject clicked = hit.collider.gameObject;
						if (EventSystem.current.IsPointerOverGameObject () == false) {
							GameManager.playerContainer.processRightClickUnitCommand (targetLoc, clicked, Input.GetKey (KeyCode.LeftShift));
						} else if (clicked.layer == 11) {
							RaycastHit hit2;
							if (Physics.Raycast (ray, out hit2, 1000, GlobalVariables.healthbarMask)) {
								Vector3 targetLoc2 = hit2.point;
								GameManager.playerContainer.processRightClickUnitCommand (targetLoc2, Terrain.activeTerrain.gameObject, Input.GetKey (KeyCode.LeftShift));
							}
						}
					}
				} else if (GameManager.playerContainer.player.curBuildingTarget.Count > 0) {
					RaycastHit hit;
					Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
					if (Physics.Raycast (ray, out hit, 1000, GlobalVariables.defaultMask)) {
						GameObject clicked = hit.collider.gameObject;
						GameManager.playerContainer.processRightClickBuildingCommand (hit.point, clicked);
					}
				} else {
					
				}
			}
		}

		if (Input.GetMouseButtonUp (1)) {

		}
	}

	public static Vector2 getMousePosition () {
		return mousePosition;
	}

	public static float getZoomValue () {
		return zoomValue;
	}

	public static bool getRotateCamera () {
		if (Input.GetKey (KeyCode.LeftControl) && Input.GetMouseButton (1)) {
			return true;
		}
		return false;
	}

}
