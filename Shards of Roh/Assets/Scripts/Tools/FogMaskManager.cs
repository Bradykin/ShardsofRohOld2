using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogMaskManager : MonoBehaviour {

	private Camera mainCamera;
	private Unit unit;
	private float startPosX;
	private float startPosY;
	private float startWidth;
	private float startHeight;
	private bool setupCheck { get; set; }

	// Use this for initialization
	void Start () {
		setupCheck = false;
	}

	public void setup () {
		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera> ();
		unit = gameObject.transform.parent.GetComponent<UnitContainer> ().unit;
		/*startPosX = curHealth.anchoredPosition.x;
		startPosY = curHealth.anchoredPosition.y;
		startWidth = curHealth.sizeDelta.x;
		startHeight = curHealth.sizeDelta.y;*/
	}

	// Update is called once per frame
	void Update () {
		if (setupCheck == false) {
			setup ();
		}

		//gameObject.transform.LookAt (gameObject.transform.position + Vector3.up, Vector3.forward);
	}
}

