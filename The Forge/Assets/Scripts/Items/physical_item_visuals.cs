using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class physical_item_visuals : MonoBehaviour {

	// Static settings
	public static float flash_time = 0.2f;
	public static float min_alpha = 0.2f;
	public static float max_alpha = 0.9f;

	// Component references
	private physical_item physical_Item;
	private SpriteRenderer sr;

	// Private vars
	private float flash_timer;


	// Init
	void Awake() {
		physical_Item = GetComponent<physical_item>();
		sr = GetComponent<SpriteRenderer>();
	}

	// Called once per frame
	void Update() {
		if (physical_Item.flashing) {
			float t = flash_timer % 2f;
			t = Mathf.Abs(1f - t);
			color_util.set_alpha(sr, Mathf.Lerp(min_alpha, max_alpha, t));
			flash_timer += Time.deltaTime / flash_time;
		} else {
			color_util.set_alpha(sr, 1f);
			flash_timer = 0;
		}
	}
}
