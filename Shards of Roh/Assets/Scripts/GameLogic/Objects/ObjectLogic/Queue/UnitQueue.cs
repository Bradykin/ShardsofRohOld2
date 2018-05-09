using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitQueue {

	public Unit unit { get; private set; }
	public int size { get; set; }
	private int maxSize { get; set; }

	public UnitQueue (Unit _unit, int _size) {
		unit = _unit;
		size = _size;
		maxSize = 5;
	}

	public bool getFull () {
		if (size >= maxSize) {
			return true;
		} else {
			return false;
		}
	}
}
