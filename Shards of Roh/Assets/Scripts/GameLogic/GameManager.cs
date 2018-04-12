using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static PlayerContainer player;
	public static List<Player> playersInGame = new List<Player> ();

	// Use this for initialization
	void Start () {
		SceneManager.LoadScene ("TestMap", LoadSceneMode.Additive);
		SelectionBox.initPostCreate ();

		player = GameObject.Find ("Player").GetComponent<PlayerContainer> ();
		playersInGame.Add (player.getPlayer ());
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
			if (playersInGame [i].getName () == newPlayerName) {
				return playersInGame [i];
			}
		}

		playersInGame.Add (newPlayer);
		return newPlayer;
	}

	public static void destroyUnit (UnitContainer _toDestroy, Player _player) {
		_player.getUnits ().Remove (_toDestroy);
		Destroy (_toDestroy.gameObject);
	}

	public static void destroyBuilding (BuildingContainer _toDestroy, Player _player) {
		_player.getBuildings ().Remove (_toDestroy);
		Destroy (_toDestroy.gameObject);
	}
}
