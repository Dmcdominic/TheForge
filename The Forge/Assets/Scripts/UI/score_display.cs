using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class score_display : MonoBehaviour {
	
	public TextMeshPro[] team_score_TMPs;

	private int[] current_display_val;
	private float[] delay_timer;

	// Static settings
	public static float display_incr_delay = 0.04f;
	public static int gold_sfx_interval = 3;

	// private vars
	private int incr_since_gold_sfx = 0;


	// Init
	private void Start() {
		current_display_val = new int[team_score_TMPs.Length];
		delay_timer = new float[team_score_TMPs.Length];
		for (int t = 0; t < team_score_TMPs.Length; t++) {
			current_display_val[t] = 0;
			delay_timer[t] = 0f;
			team_score_TMPs[t].color = teams.colors[t];
		}

		if (!game_controller.pvp) {
			team_score_TMPs[0].color = teams.colors[game_controller.the_team];
			team_score_TMPs[1].color = Color.white;
		}
	}

	// Regularly update the text displays
	private void Update() {
		update_team_scores();
	}

	// Update the team scores display
	private void update_team_scores() {
		if (game_controller.pvp) {
			for (int t = 0; t < game_controller.team_scores.Length && t < team_score_TMPs.Length; t++) {
				update_one_team_score(t, t);
			}
		} else {
			update_one_team_score(game_controller.the_team, 0);
			set_team_score_display(-1, 1);
		}
	}

	// Update a single team's score
	private void update_one_team_score(int team, int TMP_index) {
		int score = game_controller.team_scores[team];
		// TODO - replace this score with the high score
		if (current_display_val[TMP_index] < score) {
			if (delay_timer[TMP_index] <= 0) {
				current_display_val[TMP_index]++;
				set_team_score_display(team, TMP_index);
				if (incr_since_gold_sfx > 0) {
					incr_since_gold_sfx--;
				} else {
					sound_manager.play_one_shot(sound_manager.instance.coin);
					incr_since_gold_sfx = gold_sfx_interval;
				}
				delay_timer[TMP_index] = display_incr_delay;
			} else {
				delay_timer[TMP_index] -= Time.deltaTime;
			}
		} else if (current_display_val[TMP_index] >= score) {
			current_display_val[TMP_index] = score;
			set_team_score_display(team, TMP_index);
		}
	}

	// Set a team's score display directly
	private void set_team_score_display(int team, int TMP_index) {
		if (game_controller.pvp || TMP_index == 0) {
			team_score_TMPs[TMP_index].text = teams.names[team] + ": " + current_display_val[TMP_index].ToString();
		} else {
			team_score_TMPs[TMP_index].text = "High Score: A lot";
			// TODO - display the real high score
		}
	}
}
