using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class items_oracle {

	private static List<item> _all;
	public static List<item> all {
		get {
			if (_all == null) {
				string[] guids = AssetDatabase.FindAssets("t:" + typeof(item).Name);
				_all = new List<item>();
				for (int i = 0; i < guids.Length; i++) {
					string path = AssetDatabase.GUIDToAssetPath(guids[i]);
					_all.Add(AssetDatabase.LoadAssetAtPath<item>(path));
				}
			}
			return _all;
		}
	}

}
