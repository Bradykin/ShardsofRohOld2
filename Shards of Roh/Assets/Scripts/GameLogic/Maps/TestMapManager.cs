using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NavMeshBuilder = UnityEngine.AI.NavMeshBuilder;
using UnityEngine.SceneManagement;

public class TestMapManager : MonoBehaviour {



	void Start () {
		CameraController.setBounds (gameObject);
	}

	void onEnable () {
		print ("ENABLE");
	}

	// Update is called once per frame
	void Update () {
		
	}
}
