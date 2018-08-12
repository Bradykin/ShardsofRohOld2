using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarManager : MonoBehaviour {

	private Camera mainCamera;
	RectTransform curHealth;
	RectTransform health;
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
		curHealth = gameObject.transform.GetChild (1).GetComponent<RectTransform> ();
		health = gameObject.transform.GetChild (0).GetComponent<RectTransform> ();
		unit = gameObject.transform.parent.GetComponent<UnitContainer> ().unit;
		curHealth.anchoredPosition = new Vector2 (unit.healthbarDimensions.x, unit.healthbarDimensions.y);
		curHealth.sizeDelta = new Vector2 (unit.healthbarDimensions.z, unit.healthbarDimensions.w);
		health.anchoredPosition = new Vector2 (unit.healthbarDimensions.x, unit.healthbarDimensions.y);
		health.sizeDelta = new Vector2 (unit.healthbarDimensions.z, unit.healthbarDimensions.w);
		startPosX = curHealth.anchoredPosition.x;
		startPosY = curHealth.anchoredPosition.y;
		startWidth = curHealth.sizeDelta.x;
		startHeight = curHealth.sizeDelta.y;
	}
	
	// Update is called once per frame
	void Update () {
		if (setupCheck == false) {
			setup ();
		}

		gameObject.transform.LookAt (gameObject.transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
		curHealth.anchoredPosition = new Vector2 (startPosX * ((float) unit.curHealth / (float) unit.health), startPosY);
		curHealth.sizeDelta = new Vector2 (startWidth * ((float)unit.curHealth / (float)unit.health), startHeight);
	}
}
