using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResearchFactory {

	public static Age2 createAge2 (Player _owner) {
		Age2 research = new Age2 (_owner);
		return research;
	}

	public static Age3 createAge3 (Player _owner) {
		Age3 research = new Age3 (_owner);
		return research;
	}

	public static Age4 createAge4 (Player _owner) {
		Age4 research = new Age4 (_owner);
		return research;
	}

	public static AnimalTracking createAnimalTracking (Player _owner) {
		AnimalTracking research = new AnimalTracking (_owner);
		return research;
	}

	public static Forestry createForestry (Player _owner) {
		Forestry research = new Forestry (_owner);
		return research;
	}

	public static Horseshoes createHorseshoes (Player _owner) {
		Horseshoes research = new Horseshoes (_owner);
		return research;
	}

	public static ImprovedArchers createImprovedArchers (Player _owner) {
		ImprovedArchers research = new ImprovedArchers (_owner);
		return research;
	}

	public static ImprovedAxemen createImprovedAxemen (Player _owner) {
		ImprovedAxemen research = new ImprovedAxemen (_owner);
		return research;
	}

	public static ImprovedShields createImprovedShields (Player _owner) {
		ImprovedShields research = new ImprovedShields (_owner);
		return research;
	}

	public static ImprovedSpearmen createImprovedSpearmen (Player _owner) {
		ImprovedSpearmen research = new ImprovedSpearmen (_owner);
		return research;
	}

	public static ImprovedSwordsmen createImprovedSwordsmen (Player _owner) {
		ImprovedSwordsmen research = new ImprovedSwordsmen (_owner);
		return research;
	}

	public static Industrialization createIndustrialization (Player _owner) {
		Industrialization research = new Industrialization (_owner);
		return research;
	}

	public static MineralExtraction createMineralExtraction (Player _owner) {
		MineralExtraction research = new MineralExtraction (_owner);
		return research;
	}

	public static WorkerCoats createWorkerCoats (Player _owner) {
		WorkerCoats research = new WorkerCoats (_owner);
		return research;
	}

	public static Research createResearchByName (string _name, Player _owner) {
		if (_name == "Age2") {
			return createAge2 (_owner);
		} else if (_name == "Age3") {
			return createAge3 (_owner);
		} else if (_name == "Age4") {
			return createAge4 (_owner);
		} else if (_name == "AnimalTracking") {
			return createAnimalTracking (_owner);
		} else if (_name == "Forestry") {
			return createForestry (_owner);
		} else if (_name == "Horseshoes") {
			return createHorseshoes (_owner);
		} else if (_name == "ImprovedArchers") {
			return createImprovedArchers (_owner);
		} else if (_name == "ImprovedAxemen") {
			return createImprovedAxemen (_owner);
		} else if (_name == "ImprovedShields") {
			return createImprovedShields (_owner);
		} else if (_name == "ImprovedSpearmen") {
			return createImprovedSpearmen (_owner);
		} else if (_name == "ImprovedSwordsmen") {
			return createImprovedSwordsmen (_owner);
		} else if (_name == "Industrialization") {
			return createIndustrialization (_owner);
		} else if (_name == "MineralExtraction") {
			return createMineralExtraction (_owner);
		} else if (_name == "WorkerCoats") {
			return createWorkerCoats (_owner);
		}


		GameManager.print ("Fail to find name - ResearchFactory");
		return createAge2 (_owner);
	}
}
