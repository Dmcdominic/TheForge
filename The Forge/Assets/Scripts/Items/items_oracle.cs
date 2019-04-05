using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class items_oracle {

	private static List<item> _all;
	public static List<item> all {
		get {
			if (_all == null) {
				item[] items = Resources.LoadAll<item>("");
				_all = new List<item>(items);
			}
			return _all;
		}
	}

}
