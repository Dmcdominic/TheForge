﻿using System.Collections;
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
		int timer_min = Mathf.FloorToInt(timer_var) / 60;
		int timer_sec = Mathf.FloorToInt(timer_var % 60);

		if (timer_sec < 0) {
			timer_sec = 0;
		}

		string seconds_string = timer_sec.ToString("F0");
		if (seconds_string == "60") {
			seconds_string = "59";
		}

		if (seconds_string.Length == 1) {
			//timer.text = timer_min + ":0" + timer_sec.ToString("F1");
			timer.text = timer_min + ":0" + seconds_string;
		} else if (seconds_string.Length == 2) {
			//timer.text = timer_min + ":" + timer_sec.ToString("F1");
			timer.text = timer_min + ":" + seconds_string;
		} else {
			timer.text = timer_min + ":00";
		}
	}

}
