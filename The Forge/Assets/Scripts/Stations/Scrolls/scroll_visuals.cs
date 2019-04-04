﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class scroll_visuals : MonoBehaviour {

	// Fields
	public float recipe_steps_spacing;

	public GameObject visuals_parent;
	public TextMeshPro gold_TMP;
	public SpriteRenderer requested_item_sr;
	public GameObject steps_parent;
	public recipe_step step_prefab;

	// Private vars
	private item requested_item;


	// Initialize the visuals of the scroll according to the requested item
	public void init_visuals(item _requested_item) {
		requested_item = _requested_item;
		requested_item_sr.sprite = requested_item.icon;
		gold_TMP.text = requested_item.computed_gold_val.ToString() + " G";
		generate_recipe_steps();
		visuals_parent.SetActive(true);
	}

	// Hide all scroll visuals
	public void clear_scroll() {
		foreach(Transform trans in steps_parent.transform) {
			Destroy(trans.gameObject);
		}
		visuals_parent.SetActive(false);
	}

	// ========== Recipes Steps ==========
	// Set up the recipe steps visuals
	private void generate_recipe_steps() {
		int total_steps = instantiate_step(requested_item, 0);
		steps_parent.transform.Translate(recipe_steps_spacing * (total_steps - 1) / 2f, 0, 0);
	}

	// Instantiate a new recipe step
	private int instantiate_step(item product, float x_pos) {
		recipe_step new_step = Instantiate(step_prefab, steps_parent.transform);
		Vector3 pos = new_step.transform.position;
		new_step.transform.position = new Vector3(x_pos, pos.y, pos.z);

		item next_item = null;

		new_step.station_sr.sprite = product.station.icon;
		item item1 = product.ingredients[0];
		if (product.ingredients.Count == 2) {
			item item2 = product.ingredients[1];
			item easier_item = get_easier_item(item1, item2);
			new_step.ingredient_sr.sprite = easier_item.icon;
			next_item = (easier_item == item1) ? item2 : item1;
		} else if (item1.is_base_item) {
			new_step.ingredient_sr.sprite = item1.icon;
		} else {
			next_item = item1;
		}

		if (next_item) {
			return 1 + instantiate_step(next_item, x_pos - recipe_steps_spacing);
		}
		return 1;
	}

	// A quick, minimal check for which item is easier to make
	private item get_easier_item(item item1, item item2) {
		if (item1.is_base_item) {
			return item1;
		} else if (item2.is_base_item) {
			return item2;
		}
		return item1;
	}
}
