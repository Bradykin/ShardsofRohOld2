using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipLogic : MonoBehaviour
{
	public GameObject toolTip;
	public Text toolTipText;

	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{
		updateToolTip ();
	}

	private void updateToolTip () {
		if (GameManager.playerContainer.tooltipTarget == null) {
			toolTipText.text = "MISSING";
			toolTip.SetActive (false);
		} else {
			toolTip.SetActive (true);
			toolTipText.text = GameManager.playerContainer.tooltipTarget.tooltipString;
		}
	}
}
