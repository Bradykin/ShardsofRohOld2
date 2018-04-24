﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		if (GameManager.player.buildToggleActive == true) {
			if (Input.GetMouseButtonDown (0)) {
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay (getMousePosition());
				if (Physics.Raycast (ray, out hit, 1000, buildToggleMask)) {
					Building newBuilding = ObjectFactory.createBuildingByName (GameManager.player.buildToggleSetting, GameManager.addPlayerToGame ("Player"), false);
					if (GameManager.player.getPlayer ().getResource ().hasEnough (newBuilding.getCost ())) {
						GameManager.player.getPlayer ().getResource ().spend (newBuilding.getCost ());
						GameObject instance = Instantiate (Resources.Load (newBuilding.getPrefabPath (), typeof(GameObject)) as GameObject);
						instance.GetComponent<BuildingContainer> ().setBuilding (newBuilding);
						if (instance.GetComponent<UnityEngine.AI.NavMeshObstacle> () != null) {
							instance.GetComponent<UnityEngine.AI.NavMeshObstacle> ().transform.position = (new Vector3 (hit.point.x, Terrain.activeTerrain.SampleHeight (hit.point), hit.point.z));
						}

						instance.transform.GetChild (1).gameObject.SetActive (true);
						instance.transform.GetChild (2).gameObject.SetActive (false);
						instance.GetComponent<BoxCollider> ().center = instance.transform.GetChild (1).GetComponent <BoxCollider> ().center;
						instance.GetComponent<BoxCollider> ().size = instance.transform.GetChild (1).GetComponent <BoxCollider> ().size;
						instance.GetComponent<UnityEngine.AI.NavMeshObstacle> ().center = instance.transform.GetChild (1).GetComponent<UnityEngine.AI.NavMeshObstacle> ().center;
						instance.GetComponent<UnityEngine.AI.NavMeshObstacle> ().size = instance.transform.GetChild (1).GetComponent<UnityEngine.AI.NavMeshObstacle> ().size;

						GameManager.player.buildToggleActive = false;
						GameManager.player.buildToggleSetting = null;
						for (int i = 0; i < GameManager.player.buildToggle.transform.childCount; i++) {
							GameManager.player.buildToggle.transform.GetChild (i).gameObject.SetActive (false);
						}

						GameManager.addPlayerToGame ("Player").addBuildingToPlayer (instance.GetComponent<BuildingContainer> ());
					}
				}
			}
		} else {
			if (Input.GetMouseButtonDown (0)) {
				isSelecting = true;
				mousePosition1 = Input.mousePosition;
				GameManager.player.getPlayer ().setCurUnitTarget (new List<UnitContainer> ());
				GameManager.player.getPlayer ().setCurBuildingTarget (new List<BuildingContainer> ());
			} else if (Input.GetMouseButtonUp (0)) {
				isSelecting = false;
			}

			//Check if units are inside selection box
			if (isSelecting == true) {
				if (Vector3.Distance (Camera.main.ScreenToViewportPoint (mousePosition1), Camera.main.ScreenToViewportPoint (Input.mousePosition)) > 0) {
					foreach (var r in GameManager.player.getPlayer ().getUnits ()) {
						if (r != null) {
							if (SelectionBox.isInBox (r.gameObject, mousePosition1)) {
								GameManager.player.getPlayer ().addCurUnitTarget (r);
							} else {
								GameManager.player.getPlayer ().remCurUnitTarget (r);
							}
						}
					}

					if (GameManager.player.getPlayer ().getCurUnitTarget ().Count == 0) {
						foreach (var r in GameManager.player.getPlayer ().getBuildings ()) {
							if (r != null) {
								if (SelectionBox.isInBox (r.gameObject, mousePosition1)) {
									GameManager.player.getPlayer ().addCurBuildingTarget (r);
								} else {
									GameManager.player.getPlayer ().remCurBuildingTarget (r);
								}
							}
						}
					} else {
						GameManager.player.getPlayer ().setCurBuildingTarget (new List<BuildingContainer> ());
					}
				}

				//Select unit clicked on
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay (mousePosition1);
				if (Physics.Raycast (ray, out hit, 1000)) {
					if (hit.collider.gameObject.GetComponent<UnitContainer> () != null) {
						if (hit.collider.gameObject.GetComponent<UnitContainer> ().getUnit ().getOwner ().getName () == "Player") {
							GameManager.player.getPlayer ().addCurUnitTarget (hit.collider.gameObject.GetComponent<UnitContainer> ());
						}
					}

					if (hit.collider.gameObject.GetComponent<BuildingContainer> () != null && GameManager.player.getPlayer ().getCurUnitTarget ().Count == 0) {
						if (hit.collider.gameObject.GetComponent<BuildingContainer> ().getBuilding ().getOwner ().getName () == "Player") {
							GameManager.player.getPlayer ().addCurBuildingTarget (hit.collider.gameObject.GetComponent<BuildingContainer> ());
						}
					}
				}
			}
		}
	}

	public static void handleRightClick () {
		//Move currently selected units to clicked location
		if (Input.GetMouseButtonDown (1)) {
			//Check if rotating camera
			if (!Input.GetKey (KeyCode.LeftControl)) {
				//Process for selecting Units
				if (GameManager.player.getPlayer ().getCurUnitTarget ().Count > 0) {
					RaycastHit hit;
					Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
					if (Physics.Raycast (ray, out hit, 1000)) {
						Vector3 targetLoc = hit.point;
						GameObject clicked = hit.collider.gameObject;

						GameManager.player.processRightClickUnitCommand (targetLoc, clicked);
					}
				} else if (GameManager.player.getPlayer ().getCurBuildingTarget ().Count > 0) {
					
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
