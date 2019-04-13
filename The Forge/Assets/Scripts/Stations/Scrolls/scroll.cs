using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class scroll : MonoInteractable {

	[HideInInspector]
	public item requested_item;

	// Private vars
	private scroll_visuals visuals;

	// Static settings
	private static float scroll_respawn_min = 4f;
	private static float scroll_respawn_max = 8f;


	// Init
	private void Awake() {
		visuals = GetComponent<scroll_visuals>();
	}

	private void Start() {
		init_order();
	}

	// Initialize this scroll to request a certain item
	public void init_order(item _requested_item) {
		requested_item = _requested_item;
		visuals.init_visuals(requested_item);
	}
	// Initialize this scroll with a random order?
	public void init_order() {
		List<item> tier_1_items = items_oracle.all.Where<item>(x => x.tier > 0).ToList<item>();
		item chosen_item = tier_1_items[Random.Range(0, tier_1_items.Count)];
		init_order(chosen_item);
	}

	// Clears away the scroll
	public void clear_scroll(bool fulfilled) {
		if (fulfilled) {
			// Todo - completed order animation
		} else {
			// Todo - order expired animation?
			// Todo - order expired sfx?
		}
		requested_item = null;
		visuals.clear_scroll();
		StartCoroutine(refresh_scroll_delayed());
	}

	// Returns true iff Player is holding the requested item
	public override bool can_interact(player Player) {
		return requested_item != null && Player.items_carried.Contains(requested_item);
	}

	public override void on_interact(player Player) {
		game_controller.scores[Player.index] += requested_item.computed_gold_val;
		Player.items_carried.Remove(requested_item);
		clear_scroll(true);
	}

	// Scrolls are never occupied
	public override bool occupied() {
		return false;
	}

	// After a random delay, replaces this scroll
	private IEnumerator refresh_scroll_delayed() {
		yield return new WaitForSeconds(Random.Range(scroll_respawn_min, scroll_respawn_max));
		init_order();
	}
}
