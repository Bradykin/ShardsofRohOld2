using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enum {

	//Direction that WASD input instructs the camera to move
	public enum CameraDirection {FrontLeft, Front, FrontRight, Right, BackRight, Back, BackLeft, Left, None}

	//Type of target for abilities
	public enum TargetType {None, Unit, Building, Ground}

	//Type of resource
	public enum ResourceType {None, Food, Wood, Gold}
}
