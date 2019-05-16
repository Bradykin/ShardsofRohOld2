using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceDisplay : MonoBehaviour
{
	public Text foodText;
	public Text woodText;
	public Text goldText;
	public Text metalText;
	public Text populationText;
	public static string playerSetting;
	private static Player focusPlayer;

	// Use this for initialization
	void Start ()
	{
		playerSetting = "Player";
		focusPlayer = GameManager.findPlayer (playerSetting);
	}

	// Update is called once per frame
	void Update ()
	{
		updateResourceDisplay();
	}

	public static void setPlayerSetting (string _input)
	{
		playerSetting = _input;
		focusPlayer = GameManager.findPlayer (playerSetting);
	}

	private void updateResourceDisplay ()
	{
		foodText.text = ((int) focusPlayer.resource.food).ToString ();
		woodText.text = ((int) focusPlayer.resource.wood).ToString ();
		goldText.text = ((int) focusPlayer.resource.gold).ToString ();
		metalText.text = ((int) focusPlayer.resource.metal).ToString ();
		populationText.text = focusPlayer.population + "/" + focusPlayer.maxPopulation;
	}
}
