using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scroll : MonoInteractable {

	public int index;

	[HideInInspector]
	public item requested_item;

	public SpriteRenderer highlight_sr;

	// Private vars
	private scroll_visuals visuals;

	// Static settings
	private static readonly float scroll_respawn_min = 4f;
	private static readonly float scroll_respawn_max = 8f;
	private static readonly float initial_spawn_interval = 2f;

	// Static vars
	public static item[] current_requests = new item[3];


	// Init
	private void Awake() {
		// Disable this component if we are in the main menu
		if (SceneManager.GetActiveScene().buildIndex == game_controller.mm_scene) {
			enabled = false;
			return;
		}
		visuals = GetComponent<scroll_visuals>();
	}

	private void Start() {
		// for testing. TODO - spawn scrolls better
		StartCoroutine(init_first_order_delayed());
	}

	// Initialize this scroll to request a certain item
	public void init_order(item _requested_item) {
		requested_item = _requested_item;
		current_requests[index] = requested_item;
		visuals.init_visuals(requested_item);
	}
	// Initialize this scroll with a random order
	public void init_order() {
		List<item> tier_1_items = items_oracle.all.Where<item>(x => x.tier > 0).ToList<item>();
		item chosen_item = tier_1_items[Random.Range(0, tier_1_items.Count)];
		// Try a few times to avoid choosing an item that's already up there
		for (int i=0; i < 2; i++) {
			if (!current_requests.Contains(chosen_item)) {
				break;
			}
			chosen_item = tier_1_items[Random.Range(0, tier_1_items.Count)];
		}
		init_order(chosen_item);
	}

	// Clears away the scroll
	public void clear_scroll(bool fulfilled, bool init_clear = false) {
		requested_item = null;
		current_requests[index] = null;
		visuals.clear_scroll();
		if (init_clear) {
			return;
		}

		if (fulfilled) {
			// Todo - completed order animation
			// Todo - completed order sfx?
		} else {
			// Todo - order expired animation?
			// Todo - order expired sfx?
		}
		StartCoroutine(refresh_scroll_delayed());
	}

	protected override void on_set_indicator(bool active) {
		if (highlight_sr) {
			highlight_sr.gameObject.SetActive(active);
		}
	}

	// Returns true iff Player is holding the requested item
	public override bool can_interact(player Player) {
		return requested_item != null && Player.items_carried.Contains(requested_item);
	}

	public override void on_interact(player Player) {
		game_controller.increment_player_score(Player, requested_item.computed_gold_val);
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

	// Init this order once the pre_game is over, and after the appropriate delay
	private IEnumerator init_first_order_delayed() {
		clear_scroll(false, true);
		yield return new WaitUntil(() => !game_controller.pre_game);
		float delay = initial_spawn_interval * index;
		yield return new WaitForSeconds(delay);
		init_order();
	}
}
