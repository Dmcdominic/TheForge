using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class timer_display : MonoBehaviour {
	// Private vars
	private TextMeshPro timer;


	// Init
	private void Awake() {
		timer = GetComponent<TextMeshPro>();
	}

	// Update is called once per frame
	void Update() {
		float timer_var = game_controller.game_timer;
		float timer_min = Mathf.FloorToInt(timer_var) / 60;
		float timer_sec = timer_var % 60;

		if (timer_sec < 10) {
			//timer.text = timer_min + ":0" + timer_sec.ToString("F1");
			timer.text = timer_min + ":0" + timer_sec.ToString("F0");
		} else {
			//timer.text = timer_min + ":" + timer_sec.ToString("F1");
			timer.text = timer_min + ":" + timer_sec.ToString("F0");
		}
	}

}
