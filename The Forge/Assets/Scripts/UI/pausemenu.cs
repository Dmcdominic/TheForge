using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class pausemenu : MonoBehaviour {
	public static bool isPaused = false;
	public GameObject pauseMenuUI;
	private float prev_timeScale = 1f;


	// Start is called before the first frame update
	void Awake() {
		pauseMenuUI.SetActive(false);
	}

	// Update is called once per frame
	void Update() {
		for (int p = 0; p < game_controller.max_dwarves; p++) {
			if (input.p[p].start && game_controller.game_playing) {
				if (isPaused) {
					Resume();
				} else {
					Pause();
				}
			}
		}
	}

	public void Resume() {
		pauseMenuUI.SetActive(false);
		logical_resume();
	}

	private void logical_resume() {
		Time.timeScale = prev_timeScale;
		isPaused = false;
	}

	void Pause() {
		pauseMenuUI.SetActive(true);
		prev_timeScale = Time.timeScale;
		Time.timeScale = 0f;
		isPaused = true;
	}

}