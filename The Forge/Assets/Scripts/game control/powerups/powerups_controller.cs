using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum powerups { speed, jump, quick_craft };

public class powerups_controller : MonoBehaviour {

	public Dictionary<int, List<powerup_status>> team_powerups;

	// Static settings
	private static float speed_multiplier = 1.9f;
	private static float speed_ladder_multiplier = 2.3f;

	private static float jump_multiplier = 2.2f;
	private static float jump_grav_multiplier = 1.5f;

	public static float powerup_duration = 30f;

	//public static Dictionary<powerups, float> durations;


	// Static instance setup
	public static powerups_controller instance;

	private void Awake() {
		if (instance != null && instance != this) {
			Destroy(gameObject);
			return;
		}

		// Init
		instance = this;
		//DontDestroyOnLoad(gameObject);

		//durations = new Dictionary<powerups, float>();
		//durations.Add(powerups.jump, powerup_duration);
		//durations.Add(powerups.speed, powerup_duration);

		SceneManager.activeSceneChanged += onActiveSceneChanged;
	}

	private void Update() {
		for (int t=0; t < game_controller.max_teams; t++) {
			List<powerups> to_remove = new List<powerups>();
			for (int ps=0; ps < team_powerups[t].Count; ps++) {
				powerup_status PS = team_powerups[t][ps];
				PS.time_remaining -= Time.deltaTime;
				team_powerups[t][ps] = PS;
				if (PS.time_remaining <= 0) {
					to_remove.Add(PS.powerup);
				}
			}

			foreach (powerups powerup in to_remove) {
				remove_powerup(t, powerup);
			}
		}
	}

	// Reset all powerups whenever the scene changes
	private void onActiveSceneChanged(Scene prev, Scene next) {
		reset_powerups();

		// TESTING
		//set_powerup(0, powerups.jump, 7f);
		//set_powerup(1, powerups.speed, 20f);
		//set_powerup(0, powerups.quick_craft, 30f);
		//set_powerup(1, powerups.quick_craft, 30f);
	}

	// Reset all powerups
	private void reset_powerups() {
		team_powerups = new Dictionary<int, List<powerup_status>>();
		for (int t=0; t < game_controller.max_teams; t++) {
			team_powerups[t] = new List<powerup_status>();
		}
	}


	// Powerup setters
	public static void set_powerup(int team, powerups powerup, float duration) {
		instance.team_powerups[team].Add(new powerup_status(powerup, duration));
	}

	public static void remove_powerup(int team, powerups powerup) {
		instance.team_powerups[team].RemoveAll(x => x.powerup == powerup);
	}

	// Convenient accessors for powerups and multipliers
	public static bool has_powerup(int team, powerups powerup) {
		return instance.team_powerups[team].Exists(val => val.powerup == powerup);
	}
	public static bool has_powerup(player Player, powerups powerup) {
		return has_powerup(Player.team, powerup);
	}

	public static float get_duration(int team, powerups powerup) {
		if (!has_powerup(team, powerup)) {
			return 0;
		}

		powerup_status PS = instance.team_powerups[team].Find(val => val.powerup == powerup);
		return PS.time_remaining;
	}

	public static float speed_mult(player Player) {
		return has_powerup(Player, powerups.speed) ? speed_multiplier : 1f;
	}
	public static float speed_ladder_mult(player Player) {
		return has_powerup(Player, powerups.speed) ? speed_ladder_multiplier : 1f;
	}

	public static float jump_mult(player Player) {
		return has_powerup(Player, powerups.jump) ? jump_multiplier : 1f;
	}
	public static float jump_grav_mult(player Player) {
		return has_powerup(Player, powerups.jump) ? jump_grav_multiplier : 1f;
	}
}

public struct powerup_status {
	public powerups powerup;
	public float time_remaining;
	public powerup_status (powerups _powerup, float _time_remaining) {
		powerup = _powerup;
		time_remaining = _time_remaining;
	}
}
