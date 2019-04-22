using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class dwarf_spawner : MonoBehaviour {

	// Public fields
	public player dwarf_prefab;
	public Transform mm_spawn_point;
	public List<Transform> gameplay_spawn_points;

	// Static vars
	public static int[] dwarf_teams;
	public static player[] dwarves;


	// When the scene is loaded, check whether we're in the main menu or gameplay scene.
	private void Start() {
		dwarves = new player[game_controller.max_dwarves];
		if (SceneManager.GetActiveScene().buildIndex == game_controller.mm_scene) {
			full_init();
		} else {
			// Check if we're loading directly into a gameplay scene in the editor
			if (dwarf_teams == null) {
				dwarf_teams = new int[4] { 0, 1, 0, 1 };
			}
			spawn_dwarves();
		}
	}

	// Init
	private void full_init() {
		dwarf_teams = new int[game_controller.max_dwarves];
		for (int p=0; p < dwarf_teams.Length; p++) {
			dwarf_teams[p] = -1;
		}
	}

	// Spawn all the dwarves in dwarf_teams
	private void spawn_dwarves() {
		list_util.shuffle<Transform>(gameplay_spawn_points);
		for (int p = 0; p < dwarf_teams.Length; p++) {
			int team = dwarf_teams[p];
			if (team >= 0) {
				spawn_a_dwarf(p, team, gameplay_spawn_points[p].position);
			}
		}
	}

	// Spawn a particular dwarf at a particular position
	private void spawn_a_dwarf(int index, int team, Vector2 position) {
		player new_dwarf = Instantiate(dwarf_prefab);
		new_dwarf.index = index;
		new_dwarf.team = team;
		new_dwarf.transform.position = position;
		dwarves[index] = new_dwarf;
	}
	
	// Delete a particular dwarf
	private void delete_a_dwarf(int index) {
		player dwarf = dwarves[index];
		if (dwarf == null) {
			Debug.LogError("Tried to delete a non-existent dwarf at index: " + index);
			return;
		}
		// TODO - make sure this doesn't bug anything out? The forge?
		if (dwarf.current_station != null) {
			dwarf.current_station.abort_items_swap();
		}
		Destroy(dwarf.gameObject);
	}

	// Check for players trying to join (or back out) in the main menu
	void Update() {
		if (SceneManager.GetActiveScene().buildIndex != game_controller.mm_scene) {
			return;
		}

		for (int p=0; p < dwarf_teams.Length; p++) {
			if (dwarf_teams[p] < 0) {
				if (input.p[p].back) {
					dwarf_teams[p] = 0;
					spawn_a_dwarf(p, 0, mm_spawn_point.position);
				} else if (input.p[p].start) {
					dwarf_teams[p] = 1;
					spawn_a_dwarf(p, 1, mm_spawn_point.position);
				}
			} else {
				if (input.p[p].back || input.p[p].start) {
					delete_a_dwarf(p);
					dwarf_teams[p] = -1;
				}
			}
		}
	}
}
