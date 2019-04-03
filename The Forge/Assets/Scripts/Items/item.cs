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

	public static int gold_val_per_item;
	public static int gold_val_per_step;

	private int _computed_gold_val = -1;
	public int computed_gold_val {
		get {
			if (is_base_item && _computed_gold_val < 0) {
				_computed_gold_val = gold_val_per_item;
			} else if (_computed_gold_val < 0) {
				_computed_gold_val = gold_val_per_item + gold_val_per_step;
				foreach(item ingred in ingredients) {
					_computed_gold_val += ingred.computed_gold_val;
				}
			}
			return _computed_gold_val;
		}
	}

}
