using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
#if UNITY_EDITOR
					EditorApplication.isPlaying = false;
#endif
#if UNITY_STANDALONE
					Application.Quit();
#endif
				}
			}
		} else {
			escape_timer = 0f;
		}
	}
}
