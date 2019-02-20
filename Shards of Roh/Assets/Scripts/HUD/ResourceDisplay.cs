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

	// Use this for initialization
	void Start ()
	{
		playerSetting = "Player";
	}

	// Update is called once per frame
	void Update ()
	{
		updateResourceDisplay();
	}

	public void setPlayerSetting (string _input)
	{
		playerSetting = _input;
	}

	private void updateResourceDisplay ()
	{
		Player player = GameManager.findPlayer (playerSetting);
		foodText.text = ((int) player.resource.food).ToString ();
		woodText.text = ((int) player.resource.wood).ToString ();
		goldText.text = ((int) player.resource.gold).ToString ();
		metalText.text = ((int) player.resource.metal).ToString ();
		populationText.text = player.population + "/" + player.maxPopulation;
	}
}
