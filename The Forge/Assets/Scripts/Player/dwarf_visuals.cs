using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dwarf_visuals : MonoBehaviour {

	// Public fields
	public SpriteRenderer bow_sr;

	// Static settings
	public static float flash_time = 0.2f;
	public static float min_alpha = 0.2f;
	public static float max_alpha = 0.9f;

	// Component references
	private movement Movement;
	private SpriteRenderer sr;

	// Private vars
	private float flash_timer;


	// Init
	void Awake() {
		Movement = GetComponentInParent<movement>();
		sr = GetComponent<SpriteRenderer>();
	}

	// Called once per frame
	void Update() {
		if (Movement.invuln) {
			float t = flash_timer % 2f;
			t = Mathf.Abs(1f - t);
			color_util.set_alpha(sr, Mathf.Lerp(min_alpha, max_alpha, t));
			flash_timer += Time.deltaTime / flash_time;
		} else {
			color_util.set_alpha(sr, 1f);
			flash_timer = 0;
		}

		bow_sr.enabled = Konami.bow_on;
	}
}
