using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityPanelDisplay : MonoBehaviour {

	public Image panel1;
	public Image panel2;
	public Image panel3;
	public Image panel4;
	public Image panel5;
	public Image panel6;
	public Image panel7;
	public Image panel8;
	public Image panel9;
	public Image panel10;
	public Image panel11;
	public Image panel12;
	public Image panel13;
	public Image panel14;
	public Image panel15;
	public Image panel16;
	public Image panel17;
	public Image panel18;
	public List<Image> panels = new List<Image> ();

	// Use this for initialization
	void Start () {
		panels.Add (panel1);
		panels.Add (panel2);
		panels.Add (panel3);
		panels.Add (panel4);
		panels.Add (panel5);
		panels.Add (panel6);
		panels.Add (panel7);
		panels.Add (panel8);
		panels.Add (panel9);
		panels.Add (panel10);
		panels.Add (panel11);
		panels.Add (panel12);
		panels.Add (panel13);
		panels.Add (panel14);
		panels.Add (panel15);
		panels.Add (panel16);
		panels.Add (panel17);
		panels.Add (panel18);
	}
	
	// Update is called once per frame
	void Update () {
		updateAbilityPanelDisplay ();
	}

	private void updateAbilityPanelDisplay () {
		Player player = GameManager.playerContainer.player;
		List<Ability> abilities = new List<Ability> ();
		if (player.curUnitFocusIndex != -1) {
			abilities = player.curUnitTarget [player.curUnitFocusIndex].unit.abilities;
		} else if (player.curBuildingFocusIndex != -1) {
			abilities = player.curBuildingTarget [player.curBuildingFocusIndex].building.abilities;
		} 

		for (int i = 0; i < panels.Count; i++) {
			if (abilities.Count > i) {
				panels [i].gameObject.SetActive (true);
			} else {
				panels [i].gameObject.SetActive (false);
			}
		}
	}
}
