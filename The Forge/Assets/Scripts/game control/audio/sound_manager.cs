using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sound_manager : MonoBehaviour {
	// Songs
	public AudioSource Royal_Entrance;
	public AudioSource Powerhouse;

	// One-shots
	public AudioSource anvil_hit;
	public AudioSource bu_bu;
	public AudioSource coin;
	public AudioSource thunderclap;
	public AudioSource walk_into_player;
	public AudioSource opening_box;
	public AudioSource punch;
	public AudioSource oven_ding;
	public AudioSource forge_eject;
	public AudioSource scroll_unfurling;

	// Loops
	public AudioSource furnace_loop;
	public AudioSource ticking_loop;
	public AudioSource sizzling_loop;
	public AudioSource whetstone_loop;
	public AudioSource crafting_table_loop;

	// Sfx-sets
	public List<AudioSource> footsteps;

	// Voicelines
	public AudioSource pre_battle_line;
	public AudioSource bow_line;
	public AudioSource hammer_line;
	public AudioSource shield_line;
	public AudioSource sword_line;
	public List<AudioSource> entrance_lines;


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
		SceneManager.activeSceneChanged += OnSceneSwitch;
	}

	// Make sure the looping audio doesn't continue if we switch scenes
	private void OnSceneSwitch(Scene oldScene, Scene newScene) {
		update_loop(furnace_loop, false);
		update_loop(ticking_loop, false);
		update_loop(sizzling_loop, false);
		update_loop(whetstone_loop, false);
		update_loop(crafting_table_loop, false);
		StopAllCoroutines();

		if (newScene.buildIndex == game_controller.mm_scene) {
			//Royal_Entrance.Play();
			play_one_shot(thunderclap);
			StartCoroutine(start_track_delayed(Royal_Entrance, thunderclap.clip.length - 0.7f));
			Powerhouse.Stop();
		} else if (newScene.buildIndex > game_controller.mm_scene) {
			Royal_Entrance.Stop();
			//Powerhouse.Play();
			if (newScene.buildIndex == game_controller.gameplay_scene) {
				play_one_shot(pre_battle_line);
				StartCoroutine(start_track_delayed(Powerhouse, pre_battle_line.clip.length / pre_battle_line.pitch - 0.3f));
			}
		}
	}



	// ===== ONE-SHOTS =====

	// Plays a OneShot of a certain AudioSource's clip
	public static void play_one_shot(AudioSource AS, float pitch_variation = 0) {
		if (pitch_variation != 0) {
			AS.pitch = Random.Range(1f - pitch_variation, 1f + pitch_variation);
		}
		AS.PlayOneShot(AS.clip);
	}

	// Plays a OneShot with some probability 0 <= p <= 1
	public static void maybe_play_one_shot(AudioSource AS, float p, float pitch_variation = 0) {
		float rand = Random.Range(0, 1f);
		if (rand <= p) {
			play_one_shot(AS, pitch_variation);
		}
	}

	// Play an audiosource from the start
	public static void play_source(AudioSource AS) {
		AS.Stop();
		AS.Play();
	}



	// ===== LOOPS =====

	// Starts or stops a (looping) AudioSource
	public static void update_loop(AudioSource AS, bool start) {
		if (start) {
			AS.loop = true;
			AS.Play();
		} else {
			AS.Stop();
		}
	}

	// Play a loop for a set amount of time
	public static void play_loop(AudioSource AS, float duration) {
		instance.StartCoroutine(play_loop_for_duration(AS, duration));
	}
	private static IEnumerator play_loop_for_duration(AudioSource AS, float duration) {
		update_loop(AS, true);
		yield return new WaitForSeconds(duration);
		update_loop(AS, false);
	}



	// ===== SFX-SETS =====

	public static void play_from_set(List<AudioSource> ass, int index, float pitch_variation = 0f, bool one_shot = true) {
		if (one_shot) {
			play_one_shot(ass[index], pitch_variation);
		} else {
			play_source(ass[index]);
			if (ass == instance.entrance_lines) {
				int opposite_index = index >= 4 ? index - 4 : index + 4;
				ass[opposite_index].Stop();
			}
		}
	}

	public static void play_random_from_set(List<AudioSource> ass, float pitch_variation = 0f) {
		int index = Random.Range(0, ass.Count);
		play_one_shot(ass[index], pitch_variation);
	}


	// ===== DELAYED COROUTINES =====
	public IEnumerator play_one_shot_delayed(AudioSource AS, float delay) {
		yield return new WaitForSeconds(delay);
		play_one_shot(AS);
	}

	public IEnumerator start_track_delayed(AudioSource AS, float delay) {
		yield return new WaitForSeconds(delay);
		AS.Play();
	}
}
