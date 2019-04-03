using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour {

	// Fields
	public int index;
	public List<SpriteRenderer> carried_item_srs;

	// Public vars
	[HideInInspector]
	public List<item> items_carried;
	public bool carrying_items { get { return items_carried.Count > 0; } }
	public bool hands_full { get { return items_carried.Count >= item_limit; } }

	[HideInInspector]
	public MonoStation current_station = null;

	[HideInInspector]
	public int score = 0;

	// Static vars
	public static int item_limit = 2;


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

		// TEMPORARY
		//float x_velo = input.p[index].h_axis;
		//float y_velo = input.p[index].v_axis;
		//Vector3 displacement = new Vector3(x_velo, y_velo, 0).normalized * 4f * Time.deltaTime;
		//transform.Translate(displacement);
	}
}
