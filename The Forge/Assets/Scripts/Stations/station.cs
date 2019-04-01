using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum station_id { furnace, anvil, grindstone, table, ore_crate, wood_create, leather_crate }

/// <summary>
/// A generic station, used to craft certain items into other items
/// </summary>
[System.Serializable]
public class station {

	public station_id ID;
	public Sprite icon;
	public ItemID_ItemIDList_Dictionary products;

}