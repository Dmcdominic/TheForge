using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A generic item, which may be craftable, and/or used as an ingredient
/// </summary>
[CreateAssetMenu(menuName = "item")]
public class item : ScriptableObject {
	
	public Sprite icon;
	public List<item> ingredients;
	public station station;
	public int gold_value;

	public bool is_base_item { get { return ingredients.Count == 0; } }
	
}
