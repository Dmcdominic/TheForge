using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main_menu_manager : MonoBehaviour {

	// Public fields
	public player[] dwarves;

	// Private vars
	private Vector3[] init_dwarves_pos;

	// Static vars
	public static bool in_main_menu { get; private set; }


	// Static instance setup
	public static main_menu_manager instance;

	private void Awake() {
		if (instance != null && instance != this) {
			Destroy(gameObject);
			return;
		}

		// Init
		instance = this;
		DontDestroyOnLoad(gameObject);
		in_main_menu = true;

		// Store the dwarves initial positions
		init_dwarves_pos = new Vector3[dwarves.Length];
		for (int i=0; i < dwarves.Length; i++) {
			init_dwarves_pos[i] = dwarves[i].transform.position;
		}
	}

	// Update is called once per frame
	void Update() {
		if (in_main_menu) {
			for (int i = 0; i < dwarves.Length; i++) {
				if (dwarves[i].gameObject.activeSelf && input.p[i].back) {
					dwarves[i].reset_item_state();
					dwarves[i].gameObject.SetActive(false);
				} else if (!dwarves[i].gameObject.activeSelf && input.p[i].start) {
					dwarves[i].gameObject.SetActive(true);
				}
			}
		}
	}

	// Enter/exit the main menu
	public void set_main_menu(bool main_menu) {
		in_main_menu = main_menu;
		for (int i=0; i < dwarves.Length; i++) {
			if (dwarves[i].gameObject.activeInHierarchy) {
				dwarves[i].reset_item_state();
			}
		}

		if (!main_menu) {
			reset_dwarves_pos();
		}

		// Todo - swap menu scrolls with gameplay scrolls
		// Todo - any other visual polish? Some cloud animation, or lava transition?
	}

	// Start a match from the main menu
	public void start_a_match() {
		set_main_menu(false);
		// Todo - reset timer, and other initial match setup
	}

	// End a match (could be prematurely)
	public void quit_a_match() {
		// Todo - stop timer
		set_main_menu(true);
	}

	private void reset_dwarves_pos() {
		for (int i = 0; i < dwarves.Length; i++) {
			dwarves[i].transform.position = init_dwarves_pos[i];
		}
	}
}
