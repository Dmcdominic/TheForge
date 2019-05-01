using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class escape_to_quit : MonoBehaviour {
	public float escape_time;

	private float escape_timer = 0f;

	// Start is called before the first frame update
	void Awake() {
		DontDestroyOnLoad(gameObject);
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetAxisRaw("Cancel") > 0) {
			escape_timer += Time.deltaTime;
			if (escape_timer >= escape_time) {
				if (SceneManager.GetActiveScene().buildIndex >= 2) {
					SceneManager.LoadScene(1);
				} else {
					quit_util.quit_game();
				}
			}
		} else {
			escape_timer = 0f;
		}
	}
}
