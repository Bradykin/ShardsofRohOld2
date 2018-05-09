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
		if (GameManager.player.buildToggleActive == true && Input.GetMouseButtonDown (0) && EventSystem.current.IsPointerOverGameObject () == false) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (getMousePosition ());
			if (Physics.Raycast (ray, out hit, 1000, buildToggleMask)) {
				Building newBuilding = ObjectFactory.createBuildingByName (GameManager.player.buildToggleSetting, GameManager.addPlayerToGame ("Player"), false);
				if (GameManager.player.player.resource.hasEnough (newBuilding.cost)) {
					GameManager.player.player.resource.spend (newBuilding.cost);
					GameObject instance = Instantiate (Resources.Load (newBuilding.prefabPath, typeof(GameObject)) as GameObject);
					instance.GetComponent<BuildingContainer> ().building = newBuilding;
					if (instance.GetComponent<UnityEngine.AI.NavMeshObstacle> () != null) {
						instance.GetComponent<UnityEngine.AI.NavMeshObstacle> ().transform.position = (new Vector3 (hit.point.x, Terrain.activeTerrain.SampleHeight (hit.point), hit.point.z));
					}
							
					if (instance.transform.GetChild (0).gameObject.name == "Model" && instance.transform.GetChild (1).gameObject.name == "Foundation") {
						instance.transform.GetChild (0).gameObject.SetActive (false);
						instance.transform.GetChild (1).gameObject.SetActive (true);
						instance.GetComponent<BoxCollider> ().center = instance.transform.GetChild (1).GetComponent <BoxCollider> ().center;
						instance.GetComponent<BoxCollider> ().size = instance.transform.GetChild (1).GetComponent <BoxCollider> ().size;
						instance.GetComponent<UnityEngine.AI.NavMeshObstacle> ().center = instance.transform.GetChild (1).GetComponent<UnityEngine.AI.NavMeshObstacle> ().center;
						instance.GetComponent<UnityEngine.AI.NavMeshObstacle> ().size = instance.transform.GetChild (1).GetComponent<UnityEngine.AI.NavMeshObstacle> ().size;
					} else {
						print ("Model Child problem - MouseController");
					}
				
					GameManager.player.buildToggleActive = false;
					GameManager.player.buildToggleSetting = null;
					for (int i = 0; i < GameManager.player.buildToggle.transform.childCount; i++) {
						GameManager.player.buildToggle.transform.GetChild (i).gameObject.SetActive (false);
					}

					GameManager.addPlayerToGame ("Player").buildings.Add (instance.GetComponent<BuildingContainer> ());
				}
			}
		} else {
			if (Input.GetMouseButtonDown (0) && EventSystem.current.IsPointerOverGameObject () == false) {
				isSelecting = true;
				mousePosition1 = Input.mousePosition;
				GameManager.player.player.setCurUnitTarget (new List<UnitContainer> ());
				GameManager.player.player.setCurBuildingTarget (new List<BuildingContainer> ());
			} else if (Input.GetMouseButtonUp (0)) {
				isSelecting = false;
			}

			//Check if units are inside selection box
			if (isSelecting == true) {
				if (Vector3.Distance (Camera.main.ScreenToViewportPoint (mousePosition1), Camera.main.ScreenToViewportPoint (Input.mousePosition)) > 0) {
					foreach (var r in GameManager.player.player.units) {
						if (r != null) {
							if (SelectionBox.isInBox (r.gameObject, mousePosition1)) {
								GameManager.player.player.addCurUnitTarget (r);
							} else {
								GameManager.player.player.remCurUnitTarget (r);
							}
						}
					}

					if (GameManager.player.player.curUnitTarget.Count == 0) {
						foreach (var r in GameManager.player.player.buildings) {
							if (r != null) {
								if (SelectionBox.isInBox (r.gameObject, mousePosition1)) {
									GameManager.player.player.addCurBuildingTarget (r);
								} else {
									GameManager.player.player.remCurBuildingTarget (r);
								}
							}
						}
					} else {
						GameManager.player.player.setCurBuildingTarget (new List<BuildingContainer> ());
					}
				}

				//Select unit clicked on
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay (mousePosition1);
				if (Physics.Raycast (ray, out hit, 1000, GlobalVariables.defaultMask)) {
					if (hit.collider.gameObject.GetComponent<UnitContainer> () != null) {
						if (hit.collider.gameObject.GetComponent<UnitContainer> ().unit.owner.name == "Player") {
							GameManager.player.player.addCurUnitTarget (hit.collider.gameObject.GetComponent<UnitContainer> ());
						}
					}

					if (hit.collider.gameObject.GetComponent<BuildingContainer> () != null && GameManager.player.player.curUnitTarget.Count == 0) {
						if (hit.collider.gameObject.GetComponent<BuildingContainer> ().building.owner.name == "Player") {
							GameManager.player.player.addCurBuildingTarget (hit.collider.gameObject.GetComponent<BuildingContainer> ());
						}
					}
				}
			}
		}
	}

	public static void handleRightClick () {
		//Check if clicked on minimap or UI
		if (EventSystem.current.IsPointerOverGameObject () == false) {
			//Move currently selected units to clicked location
			if (Input.GetMouseButtonDown (1)) {
				//Check if rotating camera
				if (!Input.GetKey (KeyCode.LeftControl)) {
					//Process for selecting Units
					if (GameManager.player.player.curUnitTarget.Count > 0) {
						RaycastHit hit;
						Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
						if (Physics.Raycast (ray, out hit, 1000, GlobalVariables.defaultMask)) {
							Vector3 targetLoc = hit.point;
							GameObject clicked = hit.collider.gameObject;

							GameManager.player.processRightClickUnitCommand (targetLoc, clicked);
						}
					} else if (GameManager.player.player.curBuildingTarget.Count > 0) {
					
					} else {
					
					}
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
