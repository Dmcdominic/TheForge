using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scroll : MonoInteractable {

	// For TESTING
	public item init_item;

	[HideInInspector]
	public item requested_item;

	// Private vars
	private scroll_visuals visuals;


	// Init
	private void Awake() {
		visuals = GetComponent<scroll_visuals>();
	}

	private void Start() {
		init_order(init_item);
	}

	// Initialize this scroll to request a certain item
	public void init_order(item _requested_item) {
		requested_item = _requested_item;
		visuals.init_visuals(requested_item);
	}
	// Initialize this scroll with a random order?
	public void init_order() {
		// todo
	}

	// Clears away the scroll
	public void clear_scroll(bool fulfilled) {
		if (fulfilled) {
			// Todo - completed order animation
			sound_manager.instance.play_one_shot(sound_manager.instance.coin);
			// Todo - move this to the visual coin/score display,
			// which also animates coins being gained
		} else {
			// Todo - order expired animation?
			// Todo - order expired sfx?
		}
		requested_item = null;
		visuals.clear_scroll();
		// Todo - start timer to refresh this scroll?
	}

	// Returns true iff Player is holding the requested item
	public override bool can_interact(player Player) {
		return requested_item != null && Player.items_carried.Contains(requested_item);
	}

	public override void on_interact(player Player) {
		player.scores[Player.index] += requested_item.computed_gold_val;
		Player.items_carried.Remove(requested_item);
		clear_scroll(true);
	}

	// Scrolls are never occupied
	public override bool occupied() {
		return false;
	}
}
