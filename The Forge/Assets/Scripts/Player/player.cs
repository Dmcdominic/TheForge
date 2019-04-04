using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour {

	// Fields
	public int index;
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

	public static int[] scores = new int[4] { 0, 0, 0, 0 };


	// Init
	private void Awake() {
		Movement = GetComponent<movement>();
	}
	private void Start() {
		scores[index] = 0;
	}

	// Called every frame
	private void Update() {
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
}
