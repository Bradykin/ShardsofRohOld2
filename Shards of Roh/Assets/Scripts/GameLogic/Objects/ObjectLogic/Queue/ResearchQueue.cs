using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchQueue {

	public Research research { get; private set; }

	public ResearchQueue (Research _research) {
		research = _research;
	}
}