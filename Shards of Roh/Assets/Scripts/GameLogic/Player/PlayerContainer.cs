using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContainer : MonoBehaviour {

	public string playerName;
	public GameObject buildToggle;
	public bool buildToggleActive;
	public string buildToggleSetting;
	LayerMask buildToggleMask;

	private Player player;

	// Use this for initialization
	void Awake () {
		player = new Player (playerName);
		buildToggle = Instantiate (Resources.Load ("Prefabs/Miscellaneous/BuildToggle", typeof(GameObject)) as GameObject);
		buildToggleActive = false;
		buildToggleMask = 1 << LayerMask.NameToLayer ("Terrain");
	}
	
	// Update is called once per frame
	void Update () {
		if (buildToggle != null) {
			
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (MouseController.getMousePosition ());
			if (Physics.Raycast (ray, out hit, 1000, buildToggleMask)) {
				buildToggle.transform.position = (new Vector3 (hit.point.x, Terrain.activeTerrain.SampleHeight(hit.point), hit.point.z));
			}
		}
	}

	public Player getPlayer () {
		return player;
	}
}
