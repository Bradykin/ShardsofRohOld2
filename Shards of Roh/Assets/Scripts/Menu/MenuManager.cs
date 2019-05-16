using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Class that handles logic during the Menu Scene. Has a set of functions that are each linked to a different button in the UI. Does not control the escape menu in game.
public class MenuManager : MonoBehaviour {

	public GameObject menuMain;
	public GameObject menuOptions;

	// Use this for initialization
	void Start () {
		GlobalVariables.setup ();

		//menuMain = GameObject.Find ("MenuMain");
		if (menuMain == null) {
			print ("Can't find Main Menu - MenuManager");
		} else {
			menuMain.SetActive (true);
		}

		//menuOptions = GameObject.Find ("MenuOptions");
		if (menuOptions == null) {
			print ("Can't find Menu Options - MenuManager");
		} else {
			menuOptions.SetActive (false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void menuMainPlay () {
		SceneManager.LoadScene ("Map");
	}

	public void menuMainOptions () {
		menuMain.SetActive (false);
		menuOptions.SetActive (true);
	}

	public void menuMainQuit () {
		Application.Quit ();
	}

	public void menuOptionsBack () {
		menuOptions.SetActive (false);
		menuMain.SetActive (true);
	}
}
