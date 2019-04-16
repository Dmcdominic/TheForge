using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class forge : MonoStation {

	// Public fields
	public SpriteRenderer cooking_sr;
	public GameObject completed_indicator;
	public spritesheet cooking_sprites;

	public physical_item physical_item_prefab;

	// Private vars
	private player current_owner;
	private bool finished_cooking;

	private TextMeshPro player_indicator;

	// Static settings
	public static float cook_interval = 0.9f;
	public static float ejection_time = 6f;
	public static float flashing_time = 3f;
	public static float eject_speed = 6f;

	// Ejection management
	private bool flashing = false;
	private float flash_timer;


	// Init
	private void Awake() {
		player_indicator = completed_indicator.GetComponentInChildren<TextMeshPro>();

		current_owner = null;
		finished_cooking = false;
		cooking_sr.enabled = false;
		completed_indicator.SetActive(false);
		flashing = false;
	}

	public override void on_interact(player Player) {
		StopAllCoroutines();
		if (current_owner == null) {
			base.on_interact(Player);
			take_ingredients_only();
			start_cooking(Player);
		} else if (Player == current_owner) {
			user = Player;
			give_product_only();
			finished_cooking = false;
			current_owner = null;
			cooking_sr.enabled = false;
			flashing = false;
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
		StartCoroutine(eject_after_delay());
	}

	// More specific can_interact
	public override bool can_interact(player Player) {
		return (current_owner == null && base.can_interact(Player)) || (current_owner == Player && !current_owner.hands_full && finished_cooking);
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

	// Eject the ingot after the appropriate delay
	private IEnumerator eject_after_delay() {
		flashing = false;
		yield return new WaitForSeconds(ejection_time - flashing_time);
		flashing = true;
		flash_timer = 0;
		yield return new WaitForSeconds(flashing_time);
		eject_item();
	}

	// Spit the item out, in a random direction
	private void eject_item() {
		// Calculate the desired velocity
		float x_sign = Random.value > 0.5f ? -1f : 1f;
		Vector2 velocity = new Vector2(x_sign * Random.Range(0.3f, 1f), Random.Range(0.3f, 1f));
		velocity = velocity.normalized * eject_speed;

		// Create and eject the physical item
		physical_item physical_Item = Instantiate(physical_item_prefab);

		physical_Item.Item = working_on;
		physical_Item.thrower = null;
		physical_Item.just_thrown = true;

		physical_Item.transform.position = cooking_sr.transform.position;
		physical_Item.GetComponent<Rigidbody2D>().velocity = velocity;

		// Internal cleanup
		finished_cooking = false;
		current_owner = null;
		cooking_sr.enabled = false;
		flashing = false;
		completed_indicator.SetActive(false);

		user = null;
		working_on = null;
	}

	// Called every frame
	protected override void Update() {
		base.Update();
		// Your stuff here
		if (flashing) {
			float t = flash_timer % 2f;
			t = Mathf.Abs(1f - t);
			color_util.set_alpha(cooking_sr, Mathf.Lerp(physical_item_visuals.min_alpha, physical_item_visuals.max_alpha, t));
			flash_timer += Time.deltaTime / physical_item_visuals.flash_time;
		} else {
			color_util.set_alpha(cooking_sr, 1f);
			flash_timer = 0;
		}
	}
}
