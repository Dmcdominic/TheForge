using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Konami : MonoBehaviour {

	public static bool bow_on = false;

	private static readonly float axis_thresh = 0.75f;
	public enum dir { none, up, down, left, right };

	private dir[] prev_dir;
	private int[] progress;

	// Static instance setup
	public static Konami instance;

	private void Awake() {
		if (instance != null && instance != this) {
			Destroy(gameObject);
			return;
		}

		// Init
		instance = this;
		DontDestroyOnLoad(gameObject);

		prev_dir = new dir[game_controller.max_dwarves];
		progress = new int[game_controller.max_dwarves];
		for (int p = 0; p < game_controller.max_dwarves; p++) {
			prev_dir[p] = dir.none;
			progress[p] = 0;
		}
	}


	// Update is called once per frame
	void Update() {
		for (int p=0; p < game_controller.max_dwarves; p++) {
			dir current_dir = dir.none;
			dir previous_dir = prev_dir[p];

			if (input.p[p].h_axis > axis_thresh) {
				current_dir = dir.right;
			} else if (input.p[p].h_axis < -axis_thresh) {
				current_dir = dir.left;
			} else if (input.p[p].v_axis > axis_thresh) {
				current_dir = dir.up;
			} else if (input.p[p].v_axis < -axis_thresh) {
				current_dir = dir.down;
			}

			bool up = current_dir == dir.up && previous_dir != dir.up;
			bool down = current_dir == dir.down && previous_dir != dir.down;
			bool left = current_dir == dir.left && previous_dir != dir.left;
			bool right = current_dir == dir.right && previous_dir != dir.right;
			bool b = input.p[p].b;
			bool a = input.p[p].a;
			bool start = input.p[p].start;

			bool some_input = up || down || left || right || b || a || start;

			prev_dir[p] = current_dir;

			if (!some_input) {
				continue;
			}

			if (progress[p] == 0 && up) {
				progress[p]++;
			} else if (progress[p] == 1 && up) {
				progress[p]++;
			} else if (progress[p] == 2 && down) {
				progress[p]++;
			} else if (progress[p] == 3 && down) {
				progress[p]++;
			} else if (progress[p] == 4 && left) {
				progress[p]++;
			} else if (progress[p] == 5 && right) {
				progress[p]++;
			} else if (progress[p] == 6 && left) {
				progress[p]++;
			} else if (progress[p] == 7 && right) {
				progress[p]++;
			} else if (progress[p] == 8 && b) {
				progress[p]++;
			} else if (progress[p] == 9 && a) {
				progress[p]++;
			} else if (progress[p] == 10 && start) {
				bow_on = !bow_on;
				progress[p] = 0;
			}
		}
	}
}
