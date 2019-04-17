using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class recipe_indicator_control : MonoBehaviour {

	// Public fields
	public GameObject recipe_indicator;
	public SpriteRenderer ingredient_single_sr;
	public SpriteRenderer ingredient_1_sr;
	public SpriteRenderer ingredient_2_sr;
	public SpriteRenderer product_sr;

	// Component references
	private SpriteRenderer sr;


	// Init
	private void Awake() {
		sr = GetComponent<SpriteRenderer>();
		display_recipe_for(null);
	}

	// Set the indicator to display the recipe for a certain product.
	// If product is null, it hides the indicator
	public void display_recipe_for(item product) {
		if (product == null) {
			recipe_indicator.SetActive(false);
			return;
		}

		recipe_indicator.SetActive(true);
		set_sr(product_sr, product);
		if (product.ingredients.Count > 0) {
			if (product.ingredients.Count == 1) {
				set_sr(ingredient_single_sr, product.ingredients[0]);
				set_sr(ingredient_1_sr, null);
				set_sr(ingredient_2_sr, null);
			} else {
				set_sr(ingredient_single_sr, null);
				set_sr(ingredient_1_sr, product.ingredients[0]);
				set_sr(ingredient_2_sr, product.ingredients[1]);
			}
		} else {
			set_sr(ingredient_single_sr, null);
			set_sr(ingredient_1_sr, null);
			set_sr(ingredient_2_sr, null);
		}
	}

	// Util for setting each sr
	private void set_sr(SpriteRenderer sr, item Item) {
		if (Item == null) {
			sr.gameObject.SetActive(false);
		} else {
			sr.gameObject.SetActive(true);
			sr.sprite = Item.icon;
		}
	}
}
