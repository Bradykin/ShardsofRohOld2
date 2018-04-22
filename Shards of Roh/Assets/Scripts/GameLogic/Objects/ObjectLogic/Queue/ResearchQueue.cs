using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchQueue {

	private Research research;

	public ResearchQueue (Research _research) {
		research = _research;
	}

	public Research getResearch () {
		return research;
	}
}