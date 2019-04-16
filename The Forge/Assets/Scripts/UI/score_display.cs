using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class score_display : MonoBehaviour {
	
	public TextMeshPro[] player_score_TMPs;
	public TextMeshPro[] team_score_TMPs;

	private int[] current_display_val;
	private float[] delay_timer;

	// Static settings
	public static float display_incr_delay = 0.04f;
	public static int gold_sfx_interval = 2;

	// private vars
	private int incr_since_gold_sfx = 0;


	// Init
	private void Awake() {
		if (game_controller.teams) {
			current_display_val = new int[team_score_TMPs.Length];
			delay_timer = new float[team_score_TMPs.Length];
			for (int t = 0; t < team_score_TMPs.Length; t++) {
				current_display_val[t] = 0;
				delay_timer[t] = 0f;
				team_score_TMPs[t].color = teams.colors[t];
			}
		} else {
			current_display_val = new int[player_score_TMPs.Length];
			delay_timer = new float[player_score_TMPs.Length];
			for (int p = 0; p < player_score_TMPs.Length; p++) {
				current_display_val[p] = 0;
				delay_timer[p] = 0f;
			}
		}
	}

	// Regularly update the text displays
	private void Update() {
		if (game_controller.teams) {
			update_team_scores();
		} else {
			update_player_scores();
		}
	}

	// Update the team scores display
	private void update_team_scores() {
		for (int t = 0; t < game_controller.team_scores.Length && t < team_score_TMPs.Length; t++) {
			int score = game_controller.team_scores[t];
			if (current_display_val[t] < score) {
				if (delay_timer[t] <= 0) {
					current_display_val[t]++;
					team_score_TMPs[t].text = "Team " + (t + 1).ToString() + ": " + current_display_val[t].ToString();
					if (incr_since_gold_sfx > 0) {
						incr_since_gold_sfx--;
					} else {
						sound_manager.play_one_shot(sound_manager.instance.coin);
						incr_since_gold_sfx = gold_sfx_interval;
					}
					delay_timer[t] = display_incr_delay;
				} else {
					delay_timer[t] -= Time.deltaTime;
				}
			} else if (current_display_val[t] >= score) {
				current_display_val[t] = score;
				team_score_TMPs[t].text = "Team " + (t + 1).ToString() + ": " + current_display_val[t].ToString();
			}
		}
	}

	// Update the player scores display
	private void update_player_scores() {
		for (int p = 0; p < game_controller.player_scores.Length && p < player_score_TMPs.Length; p++) {
			int score = game_controller.player_scores[p];
			if (current_display_val[p] < score) {
				if (delay_timer[p] <= 0) {
					current_display_val[p]++;
					player_score_TMPs[p].text = "P" + (p + 1).ToString() + ": " + current_display_val[p].ToString();
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
				player_score_TMPs[p].text = "P" + (p + 1).ToString() + ": " + current_display_val[p].ToString();
			}
		}
	}
}
