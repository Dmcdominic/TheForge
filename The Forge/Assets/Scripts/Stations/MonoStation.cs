using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoStation : MonoInteractable {

	public station station_info;

	protected player user = null;
	protected item working_on = null;

	// Returns an item that this player could craft right now at this station.
	// Returns null if no item exists.
	public item get_craftable_item(player Player) {
		if (station_info.crate) {
			return station_info.products[0];
		}

		foreach (item Item in station_info.products) {
			bool can_craft = true;
			foreach (item ingredient in Item.ingredients) {
				if (!Player.items_carried.Contains(ingredient)) {
					can_craft = false;
				}
			}
			if (can_craft) {
				return Item;
			}
		}
		return null;
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
	public void abort_items_swap() {
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
