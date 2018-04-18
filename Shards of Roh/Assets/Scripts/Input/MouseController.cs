using System.Collections;
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
						bool attacking = false;
						Vector3 targetLoc = hit.point;
						GameObject clicked = hit.collider.gameObject;

						//Handle if clicked on unit
						if (clicked.GetComponent<UnitContainer> () != null) {
							targetLoc = clicked.GetComponent<UnitContainer> ().getUnit ().getCurLoc ();
							foreach (var r in GameManager.player.getPlayer ().getCurUnitTarget ()) {
								if (clicked.GetComponent<UnitContainer> ().getUnit ().getOwner ().getName () != GameManager.player.getPlayer ().getName ()) {
									r.getUnit ().setAttackTarget (clicked.GetComponent<UnitContainer> ());
									attacking = true;
								}
							}
						}
						//Handle is clicked on building
						else if (clicked.GetComponent<BuildingContainer> () != null) {
							targetLoc = clicked.GetComponent<BuildingContainer> ().getBuilding ().getCurLoc ();
							foreach (var r in GameManager.player.getPlayer ().getCurUnitTarget ()) {
								if (r.getUnit ().getVillager () == true) {
									//if (!(r.getUnit ().getBuildingTarget ().getBuilding ().getOwner ().getName () == r.getUnit ().getOwner ().getName ()) || !r.getUnit ().getBuildingTarget ().getBuilding ().getIsBuilt ()) {
										r.getUnit ().setAttackTarget (clicked.GetComponent<BuildingContainer> ());
										attacking = true;
									//}
								} else {
									if (clicked.GetComponent<BuildingContainer> ().getBuilding ().getIsResource () == false) {
										if (clicked.GetComponent<BuildingContainer> ().getBuilding ().getOwner ().getName () != r.getUnit ().getOwner ().getName ()) {
											//print (clicked.GetComponent<BuildingContainer> ().getBuilding ().getOwner ().getName () + " " + r.getUnit ().getOwner ().getName ());
											r.getUnit ().setAttackTarget (clicked.GetComponent<BuildingContainer> ());
											attacking = true;
										}
									}
								}
							}
						}
						//Handle if clicked on nothing
						else {
							foreach (var r in GameManager.player.getPlayer ().getCurUnitTarget ()) {
								r.getUnit ().dropAttackTarget ();
							}
						}


						//Calculate angle vectors for formations
						Vector3 unitVec = new Vector3 (0, 0, 0);
						foreach (var r in GameManager.player.getPlayer ().getCurUnitTarget ()) {
							//This works well, but I worry about performance issues. Think of a better way!
							UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath ();
							UnityEngine.AI.NavMesh.CalculatePath (r.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().transform.position, targetLoc, UnityEngine.AI.NavMesh.AllAreas, path);
							r.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().path = path;
							if (path.corners.Length >= 2) {
								unitVec = unitVec + path.corners [path.corners.Length - 2];
							} else {
								unitVec = unitVec + r.getUnit ().getCurLoc ();
							}
						}
						unitVec = unitVec / GameManager.player.getPlayer ().getCurUnitTarget ().Count;
						unitVec = (unitVec - targetLoc).normalized * 4;
						Vector3 perpVec = Vector3.Cross (unitVec, new Vector3 (0, 1, 0));

						//Calculate formation positions and match each unit to a formation spot.
						List <Formation> formationPositions = FormationController.findFormationPositions (attacking, GameManager.player.getPlayer ().getCurUnitTarget (), targetLoc, unitVec, perpVec);
						GameManager.player.getPlayer ().sortCurUnitTarget (targetLoc, formationPositions);

						//Assign each unit to move to it's formation location
						foreach (var r in GameManager.player.getPlayer ().getCurUnitTarget ()) {
							if (r.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> () != null) {
								if (formationPositions.Count > 0) {
									if (clicked.GetComponent<UnitContainer> () != null || clicked.GetComponent<BuildingContainer> () != null) {
										r.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().destination = formationPositions [0].getPosition ();
										UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath ();
										UnityEngine.AI.NavMesh.CalculatePath (r.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().transform.position, r.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().destination, UnityEngine.AI.NavMesh.AllAreas, path);
										r.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().path = path;
									} else {
										UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath ();
										UnityEngine.AI.NavMesh.CalculatePath (r.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().transform.position, formationPositions [0].getPosition (), UnityEngine.AI.NavMesh.AllAreas, path);
										r.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().path = path;
									}
									formationPositions.RemoveAt (0);
								} else {
									print ("Missing FormationPosition - MouseController");
								}
							} else {
								print ("Missing NavMeshAgent - MouseController");
							}
						}
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
