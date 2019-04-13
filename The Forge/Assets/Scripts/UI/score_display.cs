using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class score_display : MonoBehaviour {
	
	public TextMeshPro[] score_TMPs;

	private int[] current_display_val;
	private float[] delay_timer;

	// Static settings
	public static float display_incr_delay = 0.04f;
	public static int gold_sfx_interval = 2;

	// private vars
	private int incr_since_gold_sfx = 0;


	// Init
	private void Awake() {
		current_display_val = new int[score_TMPs.Length];
		delay_timer = new float[score_TMPs.Length];
		for (int p=0; p < score_TMPs.Length; p++) {
			current_display_val[p] = 0;
			delay_timer[p] = 0f;
			score_TMPs[p].text = "P" + (p + 1).ToString() + ": " + current_display_val[p].ToString();
		}
	}

	// Regularly update the text displays
	private void Update() {
		for (int p=0; p < game_controller.scores.Length && p < score_TMPs.Length; p++) {
			int score = game_controller.scores[p];
			if (current_display_val[p] < score) {
				if (delay_timer[p] <= 0) {
					current_display_val[p]++;
					score_TMPs[p].text = "P" + (p + 1).ToString() + ": " + current_display_val[p].ToString();
					if (incr_since_gold_sfx > 0) {
						incr_since_gold_sfx--;
					} else {
						sound_manager.play_one_shot(sound_manager.instance.coin);
						incr_since_gold_sfx = gold_sfx_interval;
					}
					delay_timer[p] = display_incr_delay;
				} else {
					delay_timer[p] -= Time.deltaTime;
				}
			} else if (current_display_val[p] >= score) {
				current_display_val[p] = score;
				score_TMPs[p].text = "P" + (p + 1).ToString() + ": " + current_display_val[p].ToString();
			}
		}
	}
}
