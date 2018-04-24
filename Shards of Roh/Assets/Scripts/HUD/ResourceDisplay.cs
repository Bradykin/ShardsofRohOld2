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
		Player player = GameManager.player.getPlayer ();
		Resource playerResource = player.getResource ();
		foodText.text = playerResource.getFood ().ToString ();
		woodText.text = playerResource.getWood ().ToString ();
		goldText.text = playerResource.getGold ().ToString ();
		populationText.text = player.getpopulation () + "/" + player.getmaxPopulation ();
	}
}
