using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour {

	public static LayerMask defaultMask { get; private set; }
	public static LayerMask healthbarMask { get; private set; }
	public static string mapName { get; private set; }
	public static int mapSizeX { get; private set; }
	public static int mapSizeZ { get; private set; }

	public static void setup () {
		string [] ignoreLayers = new string[2];
		ignoreLayers [0] = "MinimapShow";
		ignoreLayers [1] = "IgnoreLayer";
		defaultMask =~ LayerMask.GetMask ("MinimapShow");
		healthbarMask =~ LayerMask.GetMask (ignoreLayers);

		//Temporary, this should be set from the menu process / <mapName> manager process
		mapName = "TestMap";
		mapSizeX = 500;
		mapSizeZ = 500;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
