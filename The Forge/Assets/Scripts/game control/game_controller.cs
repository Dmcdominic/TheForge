﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class game_controller : MonoBehaviour {

	public temp_end_screen temp_endgame_screen;

	// Static vars
	public static bool pvp = true;
	public static int the_team = 0;
	public static int[] player_scores = new int[4] { 0, 0, 0, 0 };
	public static int[] team_scores = new int[2] { 0, 0 };
	public static float game_timer;
	public static bool game_playing;
	public static bool pre_game;

	// Static settings
	public static readonly float total_game_time = 240f;
	public static readonly float pre_game_time = 15f;

	public static readonly int mm_scene = 1;
	public static readonly int gameplay_scene = 2;

	public static readonly int max_dwarves = 4;
	public static readonly int max_teams = 2;


	// Static instance setup
	public static game_controller instance;

	private void Awake() {
		if (instance != null && instance != this) {
			instance.temp_endgame_screen = this.temp_endgame_screen;
			Destroy(gameObject);
			return;
		}

		// Init
		instance = this;
		DontDestroyOnLoad(gameObject);
		SceneManager.activeSceneChanged += onActiveSceneChanged;
	}

	private void onActiveSceneChanged(Scene next) {
		physical_item.init_all_physicals();
		if (next.buildIndex == mm_scene) {
			pre_game = false;
			game_playing = false;
			movement.all_players_frozen = false;
		} else if (next.buildIndex > mm_scene) {
			// Start the game immediately
			start_game();
		}
	}
	private void onActiveSceneChanged(Scene prev, Scene next) {
		onActiveSceneChanged(next);
	}

	// Start a game
	public void start_game() {
		// Some initial setup
		bool team_0 = false;
		bool team_1 = false;
		if (dwarf_spawner.dwarf_teams != null) {
			for (int dwarf = 0; dwarf < dwarf_spawner.dwarf_teams.Length; dwarf++) {
				int team = dwarf_spawner.dwarf_teams[dwarf];
				team_0 = team_0 || team == 0;
				team_1 = team_1 || team == 1;
			}
		}
		pvp = (team_0 && team_1);
		the_team = team_0 ? 0 : 1;

		// TODO - read high score from disk

		// Reset scores
		for (int p=0; p < player_scores.Length; p++) {
			player_scores[p] = 0;
		}
		for (int t = 0; t < team_scores.Length; t++) {
			team_scores[t] = 0;
		}

		// Get the timer and players going
		game_timer = pre_game_time;
		game_playing = true;
		pre_game = true;
		movement.all_players_frozen = false;
	}

	// End a game
	private void end_game(bool times_up) {
		abort_all_stations();
		game_playing = false;
		movement.all_players_frozen = true;
		display_end_screen();
	}

	// Update is called once per frame
	void Update() {
		if (game_playing) {
			if (game_timer > 0) {
				game_timer -= Time.deltaTime;
			} else if (pre_game) {
				game_timer = total_game_time;
				pre_game = false;
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

	// Abort ALL stations currently being used
	private void abort_all_stations() {
		foreach (player dwarf in dwarf_spawner.dwarves) {
			if (dwarf != null && dwarf.current_station != null) {
				dwarf.current_station.abort_items_swap();
			}
		}
	}

	// Enable the endgame screen and display the victor
	private void display_end_screen() {
		temp_endgame_screen.gameObject.SetActive(true);

		string victor_string = "";
		if (!pvp) {
			victor_string = "Great job! You scored: " + team_scores[the_team] + "!";
		} else {
			bool tie = team_scores[0] == team_scores[1];
			if (tie) {
				victor_string = "It's a tie! Both teams scored: " + team_scores[0];
			} else {
				int winning_team = team_scores[0] > team_scores[1] ? 0 : 1;
				victor_string = "GG! " + teams.names[winning_team] + " won with a score of " + team_scores[winning_team] + "!";
				temp_endgame_screen.victor_tmp.color = teams.colors[winning_team];
			}
		}

		temp_endgame_screen.set_victor_text(victor_string);
	}
}
