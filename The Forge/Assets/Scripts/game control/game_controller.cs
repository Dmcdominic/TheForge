using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class game_controller : MonoBehaviour {

	// Public fields
	public player[] dwarves;

	// Static vars
	public static bool teams = true;
	public static int[] player_scores = new int[4] { 0, 0, 0, 0 };
	public static int[] team_scores = new int[2] { 0, 0 };
	public static float game_timer;
	public static bool game_playing;

	// Static settings
	public static float game_time_in_s = 180f;

	public static int mm_scene = 1;
	public static int gameplay_scene = 2;


	// Static instance setup
	public static game_controller instance;

	private void Awake() {
		if (instance != null && instance != this) {
			Destroy(gameObject);
			return;
		}

		// Init
		instance = this;
		DontDestroyOnLoad(gameObject);

		// For testing (TODO - remove):
		start_game();
	}

	// Start a game
	private void start_game() {
		game_timer = game_time_in_s;
		game_playing = true;
		movement.all_players_frozen = false;

		// Reset scores
		for (int p=0; p < player_scores.Length; p++) {
			player_scores[p] = 0;
		}
		for (int t = 0; t < team_scores.Length; t++) {
			player_scores[t] = 0;
		}
	}

	// End a game
	private void end_game(bool times_up) {
		game_playing = false;
		movement.all_players_frozen = true;
		// TODO - end game screen here
	}

	// Update is called once per frame
	void Update() {
		if (game_playing) {
			if (game_timer > 0) {
				game_timer -= Time.deltaTime;
			} else {
				end_game(true);
			}
		}
	}

	// Add to a player's personal score, AND team score
	public static void increment_player_score(player Player, int incr) {
		player_scores[Player.index] += incr;
		team_scores[Player.team] += incr;
	}
}
