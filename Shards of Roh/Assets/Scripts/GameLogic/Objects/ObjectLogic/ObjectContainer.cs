using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectContainer : MonoBehaviour {

	public string presetOwnerName;
	public bool started = false;
	public Transform wayPoint;

	protected void setup () {
		started = true;

		foreach (Transform child in gameObject.transform) {
			if (child.name == "Waypoint") {
				wayPoint = child;
				GameManager.print ("Found waypoint1");
			}
		}

		foreach (Transform child in gameObject.transform.parent.transform) {
			if (child.name == "Waypoint") {
				wayPoint = child;
				GameManager.print ("Found waypoint2");
			}
		}

		if (wayPoint == null) {
			GameManager.print ("Couldn't find waypoint");
		}
	}

	public void setWaypointFlagLocation (Vector3 _location) {
		if (wayPoint != null) {
			wayPoint.transform.position = _location;
			wayPoint.transform.rotation = Quaternion.LookRotation (Vector3.forward, Vector3.up);
		} else {
			GameManager.print ("Can't find WayPoint - ObjectContainer.setWaypointFlagLocation");
		}
	}

	public void setWaypointFlagActive (bool _toggle) {
		if (wayPoint != null) {
			wayPoint.gameObject.SetActive (_toggle);
		} else {
			GameManager.print ("Can't find WayPoint - ObjectContainer.setWaypointFlagActive");
		}
	}
}
