using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeMenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void resume () {
		GameObject.Find ("EscapeMenu").SetActive (false);
	}

	public void loadMenu () {
		//CameraController.
		SceneManager.LoadScene ("Menu");
	}

	public void quitGame () {
		Application.Quit ();
	}
}
