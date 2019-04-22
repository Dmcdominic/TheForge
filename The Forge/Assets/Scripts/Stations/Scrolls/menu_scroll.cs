using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public enum menu_options { Play, Credits, Quit }

public class menu_scroll : MonoInteractable {

	// Public fields
	public menu_options option;
	public TextMeshPro menu_TMP;
	public item required_item;
	public GameObject completed_play;

	public SpriteRenderer highlight_sr;

	// Static settings
	private static readonly float really_time = 5f;

	// Static vars
	public static bool play_button_placed;

	// Private vars
	private bool quit_button_really;

	// Component references
	private scroll_visuals visuals;


	// Only leave this component enabled if we are in the main menu
	private void Awake() {
		if (SceneManager.GetActiveScene().buildIndex != game_controller.mm_scene) {
			enabled = false;
			return;
		}

		visuals = GetComponent<scroll_visuals>();

		if (option == menu_options.Play) {
			play_button_placed = false;
			menu_TMP.text = "?";
			visuals.init_visuals(required_item);
		} else {
			menu_TMP.text = option.ToString();
		}
		menu_TMP.gameObject.SetActive(true);
	}


	public override void on_interact(player Player) {
		switch (option) {
			case menu_options.Play:
				if (play_button_placed) {
					SceneManager.LoadScene(game_controller.gameplay_scene);
				} else if (Player.items_carried.Contains(required_item)) {
					Player.items_carried.Remove(required_item);
					play_button_placed = true;

					// Update scroll visuals
					visuals.clear_recipe_steps();
					completed_play.SetActive(true);
					menu_TMP.enabled = false;
					//menu_TMP.text = option.ToString();
				}
				break;
			case menu_options.Credits:
				// Todo - display credits
				break;
			case menu_options.Quit:
				if (quit_button_really) {
					quit_util.quit_game();
				} else {
					menu_TMP.text = "Really?";
					quit_button_really = true;
					StartCoroutine(un_really());
				}
				break;
		}
	}

	protected override void on_set_indicator(bool active) {
		if (highlight_sr) {
			highlight_sr.gameObject.SetActive(active);
		}
	}

	public override bool can_interact(player Player) {
		if (option != menu_options.Play || play_button_placed) {
			return true;
		}
		if (Player.items_carried.Contains(required_item)) {
			return true;
		}
		return false;
	}

	public override bool occupied() {
		return false;
	}

	// Coroutine to switch the "Quit" button back to "Quit" from "Really?"
	private IEnumerator un_really() {
		yield return new WaitForSeconds(really_time);
		quit_button_really = false;
		if (menu_TMP != null) {
			menu_TMP.text = "Quit";
		}
	}
}
