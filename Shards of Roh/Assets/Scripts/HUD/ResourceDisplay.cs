using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceDisplay : MonoBehaviour
{
	public Text foodText;
	public Text woodText;
	public Text goldText;
	public Text populationText;

	// Use this for initialization
	void Start ()
	{
		
	}

	// Update is called once per frame
	void Update ()
	{
		updateResourceDisplay();
	}

	private void updateResourceDisplay ()
	{
		Player player = GameManager.player.player;
		foodText.text = player.resource.getFood ().ToString ();
		woodText.text = player.resource.getWood ().ToString ();
		goldText.text = player.resource.getGold ().ToString ();
		populationText.text = player.population + "/" + player.maxPopulation;
	}
}
