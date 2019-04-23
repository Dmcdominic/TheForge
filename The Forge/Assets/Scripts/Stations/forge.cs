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
	private int current_team = -1;
	private bool finished_cooking;

	private TextMeshPro player_indicator;
	private SpriteRenderer indicator_caret;

	// Static vars
	public static item cooking_rn;
	public static int total_cooking;
	public static int total_flashing;

	// Static settings
	public static readonly float cook_interval = 0.8f;
	public static readonly float mm_cook_interval = 0.3f;
	public static readonly float ejection_time = 6f;
	public static readonly float flashing_time = 3f;
	public static readonly float eject_speed = 6f;

	// Ejection management
	private bool flashing = false;
	private float flash_timer;


	// Init
	private void Awake() {
		player_indicator = completed_indicator.GetComponentInChildren<TextMeshPro>();
		indicator_caret = completed_indicator.GetComponent<SpriteRenderer>();

		current_owner = null;
		current_team = -1;
		finished_cooking = false;
		cooking_sr.enabled = false;
		completed_indicator.SetActive(false);
		set_flashing(false);
		cooking_rn = null;
		total_cooking = 0;
	}

	public override void on_interact(player Player) {
		StopAllCoroutines();
		//if (current_owner == null) {
		if (current_team == -1) {
			base.on_interact(Player);
			take_ingredients_only();
			cooking_rn = working_on;
			start_cooking(Player);
		} else if (Player.team == current_team) {
			user = Player;
			give_product_only();
			cooking_rn = null;
			finished_cooking = false;
			current_owner = null;
			current_team = -1;
			cooking_sr.enabled = false;
			set_flashing(false);
			completed_indicator.SetActive(false);
		} else {
			Debug.LogError("Unexpected interaction with forge");
		}
	}

	// Start the forge cooking
	private void start_cooking(player Player) {
		current_owner = Player;
		current_team = Player.team;
		finished_cooking = false;
		StartCoroutine(cook());
		total_cooking++;
		total_cooking = total_cooking > 0 ? total_cooking : 1;
		sound_manager.update_loop(sound_manager.instance.furnace_loop, true);
		sound_manager.update_loop(sound_manager.instance.ticking_loop, true);
	}

	// Called when the product is done cooking
	private void on_done_cooking() {
		finished_cooking = true;
		completed_indicator.SetActive(true);
		player_indicator.text = teams.names[current_team];
		player_indicator.color = teams.lighter_colors[current_team];
		indicator_caret.color = teams.lighter_colors[current_team];
		StartCoroutine(eject_after_delay());

		// SFX
		total_cooking--;
		if (total_cooking <= 0) {
			total_cooking = 0;
			sound_manager.update_loop(sound_manager.instance.furnace_loop, false);
			sound_manager.update_loop(sound_manager.instance.ticking_loop, false);
		}
		sound_manager.play_one_shot(sound_manager.instance.oven_ding);
	}

	// More specific can_interact
	public override bool can_interact(player Player) {
		return (current_team == -1 && base.can_interact(Player)) || (current_team == Player.team && !Player.hands_full && finished_cooking);
	}

	// Coroutine for cooking the product
	private IEnumerator cook() {
		bool in_main_menu = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == game_controller.mm_scene;
		int cooking_stage = 0;
		cooking_sr.sprite = cooking_sprites.sprites[cooking_stage];
		cooking_sr.enabled = true;
		while (cooking_stage + 1 < cooking_sprites.sprites.Count) {
			yield return new WaitForSeconds(in_main_menu ? mm_cook_interval : cook_interval);
			cooking_stage++;
			cooking_sr.sprite = cooking_sprites.sprites[cooking_stage];
		}
		on_done_cooking();
	}

	// Eject the ingot after the appropriate delay
	private IEnumerator eject_after_delay() {
		set_flashing(false);
		yield return new WaitForSeconds(ejection_time - flashing_time);
		set_flashing(true);
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
		physical_Item.thrower = current_owner;
		physical_Item.just_thrown = true;

		physical_Item.transform.position = cooking_sr.transform.position;
		physical_Item.GetComponent<Rigidbody2D>().velocity = velocity;

		// SFX
		sound_manager.play_one_shot(sound_manager.instance.forge_eject);

		// Internal cleanup
		finished_cooking = false;
		current_owner = null;
		cooking_rn = null;
		current_team = -1;
		cooking_sr.enabled = false;
		set_flashing(false);
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

	// Set the flashing bool
	private void set_flashing(bool value) {
		if (flashing == value) {
			return;
		}
		flashing = value;
		total_flashing += flashing ? 1 : -1;
		if (total_flashing <= 0) {
			total_flashing = 0;
			sound_manager.update_loop(sound_manager.instance.sizzling_loop, false);
		} else if (total_flashing > 0) {
			sound_manager.update_loop(sound_manager.instance.sizzling_loop, true);
		}
	}
}
