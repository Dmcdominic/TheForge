using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class MonoStation : MonoInteractable {

	public station station_info;
	public SpriteRenderer highlight_sr;
	public SpriteRenderer tutorial_indicator;

	protected player user = null;
	protected item working_on = null;
	public item get_working_on {
		get {
			return working_on;
		}
	}


	// Called every frame
	protected override void Update() {
		base.Update();
		set_large_highlight(tutorial_controller.this_station_next(station_info));
		//if (tutorial_controller.this_station_next(station_info)) {
		//	on_set_indicator(true);
		//}
	}

	// Returns an item that this player could craft right now at this station.
	// Returns null if no item exists.
	protected override item get_craftable_item(player Player) {
		if (station_info.crate) {
			return station_info.products[0];
		}

		bool in_main_menu = SceneManager.GetActiveScene().buildIndex == game_controller.mm_scene;
		foreach (item Item in station_info.products) {
			if (!in_main_menu && !Item.craftable_outside_mm) {
				continue;
			}

			bool craftable = true;
			foreach (item ingredient in Item.ingredients) {
				if (!Player.items_carried.Contains(ingredient)) {
					craftable = false;
					break;
				}
			}
			if (craftable) {
				return Item;
			}
		}
		return null;
	}

	protected override void on_set_indicator(bool active) {
		//if (tutorial_controller.this_station_next(station_info)) {
		//	active = true;
		//}
		if (highlight_sr) {
			highlight_sr.sprite = station_info.highlight;
			highlight_sr.color = station_info.highlight_col;
			highlight_sr.gameObject.SetActive(active);
		}
	}

	private void set_large_highlight(bool active) {
		if (tutorial_indicator) {
			//tutorial_indicator.color = station_info.highlight_col;
			tutorial_indicator.gameObject.SetActive(active);
		}
	}

	// Should return true iff a player is already using this object
	public override bool occupied() {
		return user != null;
	}

	// Give the player the items that they crafted
	public void complete_items_swap() {
		if (user == null || working_on == null) {
			Debug.LogError("Tried to complete_items_swap at station " + this.gameObject + "but user or working_on was null");
		}

		foreach (item ingredient in working_on.ingredients) {
			user.items_carried.Remove(ingredient);
		}
		user.items_carried.Add(working_on);
		user.Movement.can_move = true;
		user.set_to_station(null);

		user = null;
		working_on = null;
	}

	// Take away the ingredients from the player, and let them move, but no product yet
	public void take_ingredients_only() {
		if (user == null || working_on == null) {
			Debug.LogError("Tried to take_ingredients_only at station " + this.gameObject + "but user or working_on was null");
		}

		foreach (item ingredient in working_on.ingredients) {
			user.items_carried.Remove(ingredient);
		}
		//user.items_carried.Add(working_on);
		user.Movement.can_move = true;
		user.set_to_station(null);

		user = null;
		//working_on = null;
	}

	// Give product only
	public void give_product_only() {
		if (user == null || working_on == null) {
			Debug.LogError("Tried to give_product_only at station " + this.gameObject + "but user or working_on was null");
		}

		//foreach (item ingredient in working_on.ingredients) {
		//	user.items_carried.Remove(ingredient);
		//}
		user.items_carried.Add(working_on);
		user.Movement.can_move = true;
		user.set_to_station(null);

		user = null;
		working_on = null;
	}

	// Kick the player out of this station, without exchanging the items
	public virtual void abort_items_swap() {
		if (user == null || working_on == null) {
			Debug.LogError("Tried to abort_items_swap at station " + this.gameObject + "but user or working_on was null");
		}

		user.Movement.can_move = true;

		user = null;
		working_on = null;
	}

	public override bool can_interact(player Player) {
		return !occupied() && get_craftable_item(Player) != null && !(station_info.crate && Player.hands_full);
	}

	public override void on_interact(player Player) {
		user = Player;
		working_on = get_craftable_item(Player);
		Player.set_to_station(this);
		Player.Movement.can_move = false;
	}
}
