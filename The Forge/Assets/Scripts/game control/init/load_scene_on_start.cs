using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class load_scene_on_start : MonoBehaviour {

	public int scene_to_load;

	void Start() {
		SceneManager.LoadScene(scene_to_load);
	}
}
