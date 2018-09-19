using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectContainer : MonoBehaviour {

	public string presetOwnerName;
	public bool started = false;

	protected void setup () {
		started = true;
	}

	public void setWaypointFlagLocation (Vector3 _location) {
		foreach (Transform child in gameObject.transform) {
			if (child.gameObject.activeSelf == true) {
				if (child.name == "Waypoint") {
					child.transform.position = _location;
					child.transform.rotation = Quaternion.LookRotation (Vector3.forward, Vector3.up);
				}
			}
		}
	}

	public void setWaypointFlagActive (bool _toggle) {
		foreach (Transform child in gameObject.transform) {
			if (child.name == "Waypoint") {
				child.gameObject.SetActive (_toggle);
			}
		}
	}
}
