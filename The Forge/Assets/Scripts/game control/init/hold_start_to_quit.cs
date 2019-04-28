using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class hold_start_to_quit : MonoBehaviour {
	public float escape_time;

	private List<float> escape_timer = new List<float>();

	// Start is called before the first frame update
	void Awake() {
		DontDestroyOnLoad(gameObject);
		for (int i=0; i < game_controller.max_dwarves; i++) {
			escape_timer.Add(0);
		}
	}

	// Update is called once per frame
	void Update() {
		for (int i=0; i < game_controller.max_dwarves; i++) {
			if (input.p[i].start_held) {
				escape_timer[i] += Time.deltaTime;
				if (escape_timer[i] >= escape_time) {
					if (SceneManager.GetActiveScene().buildIndex >= 2) {
						SceneManager.LoadScene(1);
					} else {
						quit_util.quit_game();
					}
				}
			} else {
				escape_timer[i] = 0f;
			}
		}
	}
}
