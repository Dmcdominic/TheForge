using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum item_id { ore, wood, leather, molten_ore, flattened_hot_ore, flattened_cool_ore, blade, sword }

/// <summary>
/// A generic item, which may be craftable, and/or used as an ingredient
/// </summary>
[System.Serializable]
public class item {

	public item_id ID;
	public Sprite icon;
	public List<item_id> ingredients;
	public station_id station;
	
}
