using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitQueue {

	private Unit unit;
	private int size;
	private int maxSize;

	public UnitQueue (Unit _unit, int _size) {
		unit = _unit;
		size = _size;
		maxSize = 5;
	}

	public Unit getUnit () {
		return unit;
	}

	public int getSize () {
		return size;
	}

	public bool getFull () {
		if (size >= maxSize) {
			return true;
		} else {
			return false;
		}
	}

	public void addSize (int _add) {
		size += _add;
	}

	public void remSize (int _rem) {
		size -= _rem;
	}
}
