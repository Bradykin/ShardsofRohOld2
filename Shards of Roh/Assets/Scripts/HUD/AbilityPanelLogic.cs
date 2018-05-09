using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AbilityPanelLogic : MonoBehaviour, IPointerClickHandler  {

	public int panelIndex;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnPointerClick (PointerEventData eventData) {
		if (eventData.button == PointerEventData.InputButton.Left) {
			GameManager.player.player.useCurTargetAbility (panelIndex);
		}
	}
}
