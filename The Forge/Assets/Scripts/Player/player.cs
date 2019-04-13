using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class player : MonoBehaviour {

	// Fields
	public int index;
	public TextMeshPro player_indicator;
	public List<SpriteRenderer> carried_item_srs;

	// Public vars
	public string display_index { get { return (index + 1).ToString(); } }
	[HideInInspector]
	public List<item> items_carried;
	public bool carrying_items { get { return items_carried.Count > 0; } }
	public bool hands_full { get { return items_carried.Count >= item_limit; } }

	[HideInInspector]
	public MonoStation current_station = null;

	[HideInInspector]
	public movement Movement { get; private set; }

	// Static vars
	public static int item_limit = 2;


	// Init
	private void Awake() {
		Movement = GetComponent<movement>();
		player_indicator.text = get_indicator_string(index);
	}

	// Called every frame
	private void Update() {
		// Check if you wanna switch your items
		if (input.p[index].swap_items) {
			if (items_carried.Count > 1) {
				item[] temp_items = items_carried.ToArray();
				for (int i = 0; i < temp_items.Length; i++) {
					items_carried[i] = temp_items[(i + 1) % temp_items.Length];
				}
			}
			foreach(SpriteRenderer sr in carried_item_srs) {
				sr.transform.Rotate(Vector3.up, 180f);
			}
		}

		// Update the visual carried-item indicators
		for (int i=0; i < carried_item_srs.Count; i++) {
			if (carried_item_srs[i] != null) {
				if (items_carried.Count <= i) {
					carried_item_srs[i].sprite = null;
				} else {
					item Item = items_carried[i];
					carried_item_srs[i].sprite = (Item != null && Item.icon != null) ? Item.icon : null;
				}
			}
		}
	}

	// Kick this player out of whatever station they are at, and remove all their items
	public void reset_item_state() {
		current_station.abort_items_swap();
		// Todo - stations need to override the abort_items_swap in order to close the quicktime event?
		items_carried.Clear();
	}

	// Play a footstep sound effect
	public void play_footstep_sfx(int index) {
		sound_manager.play_from_set(sound_manager.instance.footsteps, index, .1f);
	}

	// Get the player's indicator string
	public static string get_indicator_string(int index) {
		return "P" + (index + 1).ToString();
	}
}
