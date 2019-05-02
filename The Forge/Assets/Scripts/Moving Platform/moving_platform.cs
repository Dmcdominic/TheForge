using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class moving_platform : MonoBehaviour {

	// Public fields
	public float max_movespeed;
	public GameObject left_indicator;
	public GameObject right_indicator;

	// Private vars
	private bool touching_left_edge = false;
	private bool touching_right_edge = false;
	private List<player> players_touching = new List<player>();

	// Component references
	private Rigidbody2D rb;
	private crafting_table attached_crafting_Table;


	// Init
	private void Awake() {
		rb = GetComponent<Rigidbody2D>();
		attached_crafting_Table = GetComponentInChildren<crafting_table>();
	}

	// Update is called once per frame
	void Update() {

		int try_move_left = 0;
		int try_move_right = 0;

		foreach (player p in players_touching) {
			p.Movement.platform_velo = rb.velocity;
			if (input.p[p.index].platform_left) {
				try_move_left++;
			}
			if (input.p[p.index].platform_right) {
				try_move_right++;
			}
		}
		
		if (movement.all_players_frozen || attached_table_occupied()) {
			rb.velocity = new Vector2(0, 0);
			left_indicator.SetActive(false);
			right_indicator.SetActive(false);
			return;
		}

		int right_sum = try_move_right - try_move_left;

		if (right_sum < 0 && !touching_left_edge) {
			rb.velocity = new Vector2(max_movespeed * right_sum, 0);
		} else if (right_sum > 0 && !touching_right_edge) {
			rb.velocity = new Vector2(max_movespeed * right_sum, 0);
			//} else if (touching_left_edge || touching_right_edge) {
		} else {
			if (SceneManager.GetActiveScene().buildIndex == game_controller.mm_scene && !touching_right_edge && players_touching.Count == 0) {
				rb.velocity = new Vector2(max_movespeed, 0);
			} else {
				rb.velocity = new Vector2(0, 0);
			}
		}

		left_indicator.SetActive(players_touching.Count > 0 && !touching_left_edge);
		right_indicator.SetActive(players_touching.Count > 0 && !touching_right_edge);
	}


	// Returns true iff there's a crafting table attached to the moving platform, and it is currently occupied
	private bool attached_table_occupied() {
		return attached_crafting_Table != null && attached_crafting_Table.occupied();
	}

	// Keeps track of which players are touching the platform
	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.CompareTag("Player")) {
			player p = collision.gameObject.GetComponent<player>();
			players_touching.Add(p);
			p.Movement.platform_velo = rb.velocity;
		}
	}

	private void OnCollisionExit2D(Collision2D collision) {
		if (collision.gameObject.CompareTag("Player")) {
			player p = collision.gameObject.GetComponent<player>();
			if (p != null) {
				players_touching.Remove(p);
			}
			p.Movement.platform_velo = new Vector2(0, 0);
		}
	}

	// Edge collision management
	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.CompareTag("Left edge")) {
			touching_left_edge = true;
		} else if (collision.CompareTag("Right edge")) {
			touching_right_edge = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision) {
		if (collision.CompareTag("Left edge")) {
			touching_left_edge = false;
		} else if (collision.CompareTag("Right edge")) {
			touching_right_edge = false;
		}
	}
}
