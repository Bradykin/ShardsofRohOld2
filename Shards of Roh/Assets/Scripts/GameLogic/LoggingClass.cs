using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoggingClass {

	public bool gatherResources;
	public bool objectPlannerValues;
	public bool objectPlannerResults;
	public bool createNewObject;

	public LoggingClass () {
		gatherResources = false;
		objectPlannerValues = false;
		objectPlannerResults = false;
		createNewObject = false;
	}
}

