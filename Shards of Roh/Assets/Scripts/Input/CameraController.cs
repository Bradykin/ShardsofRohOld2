﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class CameraController : MonoBehaviour {

	private static CameraDirection moveDirection;
	private static float zoomValue;
	private static float xPos;
	private static float zPos;

	private static float scrollSpeed;
	private static float zoomSpeed;
	private static float scrollWidth;
	private static float screenWidth;
	private static float screenHeight;
	private static Vector3 min;
	private static Vector3 max;
	private static bool boundsSet = false;

	// Use this for initialization
	void Start () {
		scrollSpeed = 50.0f;
		zoomSpeed = -375000.0f;
		scrollWidth = 25.0f;
		screenWidth = Screen.width;
		screenHeight = Screen.height;
		moveDirection = CameraDirection.None;
	}
	
	// Update is called once per frame
	void Update () {
		moveCamera ();
		RotateCamera ();
	}
		

	//Set the bounds of camera movement
	public static void setBounds (GameObject _map) {
		if (_map != null) {
			if (_map.transform.position != new Vector3 (-250, 0, -250)) {
				_map.transform.position = new Vector3 (-250, 0, -250);
			}
			min = new Vector3 (_map.transform.position.x - 25.0f, 5.0f, _map.transform.position.z - 25.0f);
			max = new Vector3 (_map.transform.position.x + Terrain.activeTerrain.terrainData.size.x + 25.0f, 100.0f, _map.transform.position.z + Terrain.activeTerrain.terrainData.size.z + 25.0f);
			boundsSet = true;
		} else {
			print ("Missing Terrain - CameraController.setBounds");
		}
	}

	//For hotKeyController to set moveDirection
	public static void setMoveDirection (CameraDirection _moveDirection) {
		moveDirection = _moveDirection;
	}

	//Calculate new camera location using hotkey, mouse location, and mouse wheel zoom
	private Vector3 calcLocation (Vector3 _origin) {
		Vector3 movement = new Vector3 (0, 0, 0);
		//Process hotKey moveDirection
		if (moveDirection != CameraDirection.None) {
			if ((moveDirection == CameraDirection.FrontLeft) || (moveDirection == CameraDirection.Front) || (moveDirection == CameraDirection.FrontRight)) {
				movement.z = movement.z + scrollSpeed;
			}
			if ((moveDirection == CameraDirection.BackLeft) || (moveDirection == CameraDirection.Back) || (moveDirection == CameraDirection.BackRight)) {
				movement.z = movement.z - scrollSpeed;
			}
			if ((moveDirection == CameraDirection.FrontRight) || (moveDirection == CameraDirection.Right) || (moveDirection == CameraDirection.BackRight)) {
				movement.x = movement.x + scrollSpeed;
			}
			if ((moveDirection == CameraDirection.FrontLeft) || (moveDirection == CameraDirection.Left) || (moveDirection == CameraDirection.BackLeft)) {
				movement.x = movement.x - scrollSpeed;
			}
		}

		//Process mouse location
		if (xPos >= 0 && xPos <= scrollWidth) {
			if (moveDirection == CameraDirection.Front || moveDirection == CameraDirection.None || moveDirection == CameraDirection.Back) {
				movement.x = movement.x - scrollSpeed;
			}
		} else if (xPos <= screenWidth && xPos >= screenWidth - scrollWidth) {
			if (moveDirection == CameraDirection.Front || moveDirection == CameraDirection.None || moveDirection == CameraDirection.Back) {
				movement.x = movement.x + scrollSpeed;
			}
		}
		if (zPos >= 0 && zPos <= scrollWidth) {
			if (moveDirection == CameraDirection.Left || moveDirection == CameraDirection.None || moveDirection == CameraDirection.Right) {
				movement.z = movement.z - scrollSpeed;
			}
		} else if (zPos <= screenHeight && zPos >= screenHeight - scrollWidth) {
			if (moveDirection == CameraDirection.Left || moveDirection == CameraDirection.None || moveDirection == CameraDirection.Right) {
				movement.z = movement.z + scrollSpeed;
			}
		}
			
		//Rotate camera angle to camera facing direction
		movement = Camera.main.transform.TransformDirection (movement);

		//Process mouse wheel zoom
		movement.y = 0;
		movement.y = movement.y + (zoomValue * zoomSpeed * Time.deltaTime);


		return _origin + (movement * Time.deltaTime);
	}

	//Check if camera is inside bounds
	private Vector3 checkBounds (Vector3 _origin, Vector3 _destination) {
		if (boundsSet == true) {
			if (Terrain.activeTerrain != null) {
				float destinationHeight = Terrain.activeTerrain.SampleHeight (_destination);
				float originHeight = Terrain.activeTerrain.SampleHeight (_origin);

				if (_destination.x < min.x) {
					_destination.x = min.x;
				} else if (_destination.x > max.x) {
					_destination.x = max.x;
				}

				if (_destination.y < min.y + destinationHeight) {
					_destination.y = min.y + destinationHeight;
				} else if (_destination.y > max.y + destinationHeight) {
					_destination.y = max.y + destinationHeight;
				} else {
					_destination.y = _destination.y + destinationHeight - originHeight;
				}

				if (_destination.z < min.z) {
					_destination.z = min.z;
				} else if (_destination.z > max.z) {
					_destination.z = max.z;
				}
			} else {
				print ("Missing Terrain - CameraController.checkBounds");
			}
		}
		return _destination;
	}

	private void moveCamera () {
		//Update values
		Vector3 origin = Camera.main.transform.position;
		zoomValue = MouseController.getZoomValue ();
		xPos = MouseController.getMousePosition ().x;
		zPos = MouseController.getMousePosition ().y;

		Vector3 destination = calcLocation (origin);
		destination = checkBounds (origin, destination);
		Camera.main.transform.position = destination;
	}

	private void RotateCamera () {
		if (MouseController.getRotateCamera ()) {
			Vector3 origin = Camera.main.transform.eulerAngles;
			Vector3 destination = origin;

			destination.x -= Input.GetAxis ("Mouse Y") * 15;
			destination.y += Input.GetAxis ("Mouse X") * 15;

			if (destination != origin) {
				Camera.main.transform.eulerAngles = Vector3.MoveTowards (origin, destination, Time.deltaTime * 100);
			}
		}
	}
}