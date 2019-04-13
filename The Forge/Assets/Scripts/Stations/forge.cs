using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class forge : MonoStation {

	// Public fields
	public SpriteRenderer cooking_sr;
	public GameObject completed_indicator;
	public spritesheet cooking_sprites;

	// Private vars
	private player current_owner;
	private bool finished_cooking;

	private TextMeshPro player_indicator;

	// Static settings
	public static float cook_interval = 1f;


	// Init
	private void Awake() {
		player_indicator = completed_indicator.GetComponentInChildren<TextMeshPro>();

		current_owner = null;
		finished_cooking = false;
		cooking_sr.enabled = false;
		completed_indicator.SetActive(false);
	}

	public override void on_interact(player Player) {
		if (current_owner == null) {
			base.on_interact(Player);
			take_ingredients_only();
			start_cooking(Player);
		} else if (Player == current_owner) {
			user = Player;
			complete_items_swap();
			finished_cooking = false;
			current_owner = null;
			cooking_sr.enabled = false;
			completed_indicator.SetActive(false);
		} else {
			Debug.LogError("Unexpected interaction with forge");
		}
	}

	// Start the forge cooking
	private void start_cooking(player Player) {
		current_owner = Player;
		finished_cooking = false;
		StartCoroutine(cook());
		sound_manager.update_loop(sound_manager.instance.furnace_loop, true);
	}

	// Called when the product is done cooking
	private void on_done_cooking() {
		finished_cooking = true;
		completed_indicator.SetActive(true);
		player_indicator.text = player.get_indicator_string(current_owner.index);
		sound_manager.update_loop(sound_manager.instance.furnace_loop, false);
	}

	// More specific can_interact
	public override bool can_interact(player Player) {
		return (current_owner == null && base.can_interact(Player)) || (current_owner == Player && finished_cooking);
	}

	// Coroutine for cooking the product
	private IEnumerator cook() {
		int cooking_stage = 0;
		cooking_sr.sprite = cooking_sprites.sprites[cooking_stage];
		cooking_sr.enabled = true;
		while (cooking_stage + 1 < cooking_sprites.sprites.Count) {
			yield return new WaitForSeconds(cook_interval);
			cooking_stage++;
			cooking_sr.sprite = cooking_sprites.sprites[cooking_stage];
		}
		on_done_cooking();
	}
}
