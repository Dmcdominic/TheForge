using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorial_controller : MonoBehaviour {

	// Public fields
	public item final_target_item;

	// Static vars
	public static item next_item = null;
	public static bool this_station_next(station Station) {
		return (next_item != null && next_item.station == Station);
	}
	

	// Called once per frame
	void Update() {
		if (menu_scroll.play_button_placed) {
			next_item = null;
			return;
		}
		
		next_item = final_target_item;
		while (!are_all_ingredients_ready(next_item)) {
			bool updated_ingred = false;
			foreach (item ingredient in next_item.ingredients) {
				if (!is_item_in_possession(ingredient)) {
					next_item = ingredient;
					updated_ingred = true;
					break;
				}
			}
			if (!updated_ingred) {
				break;
			}
		}
	}

	// Reset next_item when we leave the tutorial (i.e. start the game)
	private void OnDestroy() {
		next_item = null;
	}

	// Check if any dwarf is holding a particular item
	public static bool is_item_in_possession(item Item) {
		foreach (player dwarf in dwarf_spawner.dwarves) {
			if (dwarf == null) {
				continue;
			}
			if (dwarf.items_carried.Contains(Item)) {
				return true;
			}
		}
		return false;
	}

	// Check if any dwarf is holding all the ingredients for a particular item
	public static bool are_all_ingredients_ready(item Item) {
		if (Item.is_base_item) {
			return true;
		}

		foreach (player dwarf in dwarf_spawner.dwarves) {
			if (dwarf == null) {
				continue;
			}
			bool has_all_ingred = true;
			foreach (item ingredient in next_item.ingredients) {
				if (!dwarf.items_carried.Contains(ingredient)) {
					has_all_ingred = false;
					break;
				}
			}
			if (has_all_ingred) {
				return true;
			}
		}
		return false;
	}
}
