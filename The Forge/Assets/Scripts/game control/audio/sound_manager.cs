using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sound_manager : MonoBehaviour {
	// One-shots
	public AudioSource anvil_hit;
	public AudioSource bu_bu;
	public AudioSource coin;
	public AudioSource thunderclap;
	public AudioSource walk_into_player;

	// Loops
	public AudioSource furnace_loop;
	public AudioSource whetstone_loop;



	// Static instance setup
	public static sound_manager instance;
	
	private void Awake() {
		if (instance != null && instance != this) {
			Destroy(gameObject);
			return;
		}

		// Init
		instance = this;
		DontDestroyOnLoad(gameObject);
	}



	// ===== One-shots =====
	// Plays a OneShot of a certain AudioSource's clip
	public void play_one_shot(AudioSource AS) {
		AS.PlayOneShot(AS.clip);
	}



	// ===== Loops =====
	// Starts or stops a (looping) AudioSource
	private void update_loop(AudioSource AS, bool start) {
		if (start) {
			AS.loop = true;
			AS.Play();
		} else {
			AS.Stop();
		}
	}
}
