using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionBox : MonoBehaviour {

	static Texture2D whiteTexture;

	//Create the Texture for Selection Box
	public static void initPostCreate () {
		whiteTexture = new Texture2D (1, 1);
		whiteTexture.SetPixel (0, 0, Color.white);
		whiteTexture.Apply ();
	}

	//Check if a GameObject is inside SelectionBox based on _mousePosition1 and Input.mousePosition
	public static bool isInBox (GameObject _gameObject, Vector3 _mousePosition1) {
		var bounds = getBounds (_mousePosition1, Input.mousePosition);

		return bounds.Contains (Camera.main.WorldToScreenPoint (_gameObject.transform.position));
	}

	//Get the bounds of the SelectionBox based on _mousePosition1 and _mousePosition2
	public static Bounds getBounds (Vector3 _mousePosition1, Vector3 _mousePosition2) {
		var min = Vector3.Min (_mousePosition1, _mousePosition2);
		var max = Vector3.Max (_mousePosition1, _mousePosition2);
		min.z = Camera.main.nearClipPlane;
		max.z = Camera.main.farClipPlane;

		var bounds = new Bounds();
		bounds.SetMinMax (min, max);
		return bounds;
	}

	//Get the Screen Rect of the SelectionBox based on __mousePosition1 and __mousePosition2
	public static Rect getScreenRect (Vector3 _mousePosition1, Vector3 _mousePosition2) {
		// Move origin from bottom left to top left
		_mousePosition1.y = Screen.height - _mousePosition1.y;
		_mousePosition2.y = Screen.height - _mousePosition2.y;
		// Calculate corners
		var topLeft = Vector3.Min (_mousePosition1, _mousePosition2);
		var bottomRight = Vector3.Max (_mousePosition1, _mousePosition2);
		// Create Rect
		return Rect.MinMaxRect (topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
	}

	//Draw the Screen Rect
	public static void drawScreenRect (Rect rect, Color color) {
		GUI.color = color;
		GUI.DrawTexture (rect, whiteTexture);
		GUI.color = Color.white;
	}

	//Draw border around the Screen Rect
	public static void drawScreenRectBorder (Rect rect, float thickness, Color color) {
		// Top
		drawScreenRect (new Rect (rect.xMin, rect.yMin, rect.width, thickness), color);
		// Left
		drawScreenRect (new Rect (rect.xMin, rect.yMin, thickness, rect.height), color);
		// Right
		drawScreenRect (new Rect (rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
		// Bottom
		drawScreenRect (new Rect (rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
	}
}
