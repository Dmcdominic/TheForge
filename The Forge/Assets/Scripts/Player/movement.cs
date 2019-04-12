using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(Animator))]
[RequireComponent(typeof(player))]
public class movement : MonoBehaviour {
	// Public fields
	public float x_mult;

	public float jump_velo;
	public float falling_grav_mult;
	public float ghost_jump_delay;
	public float raycast_dist;

	public float ladder_velo;

	[HideInInspector]
	private bool movement_enabled;
	public static bool all_players_frozen = false;
	public bool can_move {
		get { return movement_enabled && !all_players_frozen; }
		set { movement_enabled = value; }
	}

	// Private vars
	private int index;

	private float base_grav_scale;
	private bool jump_held = false;
	private bool jump_grounded_check = false;
	private float jump_grounded_delay;
	private bool just_jumped_off_ladder;

	private int ladder_count = 0;
	private bool touching_ladder { get { return ladder_count > 0; } }
	private bool holding_onto_ladder = false;

	// Component references
	private Rigidbody2D rb;
	private Collider2D col;
	private player Player;
	private Animator animator;


	// Init
	private void Awake() {
		rb = GetComponent<Rigidbody2D>();
		col = GetComponent<Collider2D>();
		animator = GetComponent<Animator>();
		Player = GetComponent<player>();

		base_grav_scale = rb.gravityScale;
		index = Player.index;

		movement_enabled = true;
	}

	// Update is called once per frame
	void FixedUpdate() {
		if (!can_move) {
			rb.velocity = new Vector2(0, rb.velocity.y);
			col.isTrigger = false;
			animator.SetBool("running", false);
			animator.SetBool("carrying", Player.carrying_items);
			update_grav_scale();
			return;
		}
		
		// Get input
		float x_input = input.p[index].h_axis;
		bool jump_pressed = input.p[index].jump;
		float y_input = input.p[index].v_axis;
		//bool jump_pressed = y_input > 0;

		// Horizontal movement
		rb.velocity = new Vector2(x_input * x_mult, rb.velocity.y);

		// Ladder check
		bool up_ladder = touching_ladder && y_input > 0;
		bool down_ladder = touching_ladder && y_input < 0;
		if (rb.velocity.y <= -0.1f && !touching_ladder) {
			just_jumped_off_ladder = false;
		}

		holding_onto_ladder = !just_jumped_off_ladder && (up_ladder || down_ladder || (touching_ladder && holding_onto_ladder));
		if (holding_onto_ladder) {
			col.isTrigger = true;
			update_velo_on_ladder(up_ladder, down_ladder);
		} else {
			col.isTrigger = false;
		}

		// Jump controls
		Vector3 left_foot_pos = new Vector3(col.bounds.min.x, col.bounds.min.y);
		Vector3 right_foot_pos = new Vector3(col.bounds.max.x, col.bounds.min.y);
		
		bool has_footing = false;
		RaycastHit2D[] hits = Physics2D.RaycastAll(left_foot_pos, Vector2.down, raycast_dist);
		for (int i=0; i < hits.Length; i++) {
			if (!hits[i].collider.isTrigger && hits[i].transform.gameObject != gameObject) {
				has_footing = true;
				break;
			}
		}

		if (!has_footing) {
			hits = Physics2D.RaycastAll(right_foot_pos, Vector2.down, raycast_dist);
			for (int i = 0; i < hits.Length; i++) {
				if (!hits[i].collider.isTrigger && hits[i].transform.gameObject != gameObject) {
					has_footing = true;
					break;
				}
			}
		}
		
		//if ((has_footing && rb.velocity.y <= 0.1f) || touching_ladder) {
		if (has_footing || touching_ladder) {
			jump_grounded_check = true;
			jump_grounded_delay = ghost_jump_delay;
		} else if (jump_grounded_delay <= 0 || rb.velocity.y > jump_velo * 0.2) {
			jump_grounded_check = false;
			jump_grounded_delay = 0;
		} else {
			jump_grounded_delay -= Time.deltaTime;
		}

		if (jump_grounded_check && jump_pressed) {
			jump();
		}

		//jump_held = jump_pressed;
		jump_held = input.p[index].jump_held;

		// Fall through platforms if you are holding down
		//col.enabled = input.p[index].v_axis >= 0;

		// Check if you should be walking
		bool running = Mathf.Abs(x_input) > 0 && Mathf.Abs(rb.velocity.y) < 0.1f;
		animator.SetBool("running", running);

		// Check if you should be jumping
		bool jumping = rb.velocity.y >= 0.1f;
		//animator.SetBool("jumping", jumping);

		update_grav_scale();

		// Transform flip
		if (x_input > 0) {
			transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, 180f, transform.rotation.z));
		} else if (x_input < 0) {
			transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, 0, transform.rotation.z));
		}

		// Update animations
		animator.SetBool("carrying", Player.carrying_items);
	}

	// Jump!
	private void jump() {
		rb.velocity = new Vector2(rb.velocity.x, jump_velo);
		holding_onto_ladder = false;
		if (touching_ladder) {
			just_jumped_off_ladder = true;
		}
		// todo - jump sound
		//SoundManager.instance.playJump();
	}

	// Ladder usage
	private void update_velo_on_ladder(bool up_ladder, bool down_ladder) {
		if (!holding_onto_ladder) {
			return;
		}
		if (up_ladder) {
			rb.velocity = new Vector2(rb.velocity.x, ladder_velo);
		} else if (down_ladder) {
			rb.velocity = new Vector2(rb.velocity.x, -ladder_velo);
		} else {
			rb.velocity = new Vector2(rb.velocity.x, 0);
		}
	}

	// Update the rigidbody's gravity scale based on whether or not you're falling
	private void update_grav_scale() {
		//float y_input = input.p[index].v_axis;
		//bool jump_held = y_input > 0;
		bool jump_held = input.p[index].jump_held;
		if (holding_onto_ladder) {
			rb.gravityScale = 0;
		} else if (rb.velocity.y < 0) {
			rb.gravityScale = base_grav_scale * falling_grav_mult;
			//animator.SetBool("falling", true);
		} else {
			//animator.SetBool("falling", false);
			if (jump_held) {
				rb.gravityScale = base_grav_scale;
			} else {
				rb.gravityScale = base_grav_scale * falling_grav_mult;
			}
		}
	}

	// Ladder management
	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.CompareTag("Ladder")) {
			ladder_count++;
		}
	}
	private void OnTriggerExit2D(Collider2D collision) {
		if (collision.CompareTag("Ladder")) {
			ladder_count--;
		}
	}
}
