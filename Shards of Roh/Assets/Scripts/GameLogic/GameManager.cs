using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static PlayerContainer playerContainer;
	public static List<Player> playersInGame = new List<Player> ();

	// Use this for initialization
	void Start () {
		playersInGame.Clear ();
		SceneManager.LoadScene ("TestMap", LoadSceneMode.Additive);
		SelectionBox.initPostCreate ();
		GlobalVariables.setup ();

		GameObject instance = Instantiate (Resources.Load ("Player", typeof(GameObject)) as GameObject);
		playerContainer = instance.GetComponent<PlayerContainer> ();
		playersInGame.Add (playerContainer.player);
		playerContainer.player.resource.add (new Resource (1000, 1000, 1000));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Add the player to the game if it does not yet exist, or return it if it does
	public static Player addPlayerToGame (string newPlayerName) {
		Player newPlayer = new Player (newPlayerName);

		if (playersInGame [0] == null) {
			playersInGame.Add (newPlayer);
			return newPlayer;
		}

		for (int i = 0; i < playersInGame.Count; i++) {
			if (playersInGame [i].name == newPlayerName) {
				return playersInGame [i];
			}
		}

		GameObject.Find ("AI").AddComponent<AIController> ().player = newPlayer;
		playersInGame.Add (newPlayer);
		return newPlayer;
	}

	public static bool isEnemies (Player _player1, Player _player2) {
		if (_player1.name != _player2.name) {
			return true;
		} else {
			return false;
		}
	}

	public static void destroyUnit (UnitContainer _toDestroy, Player _player) {
		_player.curUnitTarget.Remove (_toDestroy);
		_player.units.Remove (_toDestroy);
		Destroy (_toDestroy.gameObject);
	}

	public static void destroyBuilding (BuildingContainer _toDestroy, Player _player) {
		_player.buildings.Remove (_toDestroy);
		Destroy (_toDestroy.gameObject);
	}

	public static PlayerContainer getPlayer () {
		return playerContainer;
	}
}
