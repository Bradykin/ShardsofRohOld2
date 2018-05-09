using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour {

	public static LayerMask defaultMask { get; private set; }

	public static void setup () {
		defaultMask =~ LayerMask.GetMask ("MinimapShow");
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
