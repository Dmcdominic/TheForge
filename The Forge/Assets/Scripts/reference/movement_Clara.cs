using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(Animator))]
public class movement_Clara : MonoBehaviour {

	// Public fields
	public float x_mult;

	public float jump_velo;
	public float falling_grav_mult;
	public float ghost_jump_delay;
	public float raycast_dist;

	// Static settings
	public static bool movement_enabled = true;
	public static int current_scene = 1;
	public static movement_Clara player_instance;

	// Private vars
	private readonly string horizontal_axis = "Horizontal";
	private readonly string vertical_axis = "Vertical";

	private float base_grav_scale;
	private bool jump_held = false;
	private bool jump_grounded_check = false;
	private float jump_grounded_delay;

	private int landing_layer_mask;

	// Component references
	private Rigidbody2D rb;
	private Collider2D col;
	private Animator animator;


	// Init
	void Awake() {
		if (player_instance == null) {
			player_instance = this;
		} else if (player_instance != this) {
			Destroy(gameObject);
			return;
		}

		rb = GetComponent<Rigidbody2D>();
		col = GetComponent<Collider2D>();
		animator = GetComponent<Animator>();

		base_grav_scale = rb.gravityScale;
		landing_layer_mask = LayerMask.GetMask(new string[] { "platform", "object" });

		//camera_controller.focus = transform;
	}

	public static void full_init() {
		movement_enabled = true;
		current_scene = 1;
		// Call other full init methods here(?)
	}

	// Called once per frame
	void FixedUpdate() {
		// Reset the current_scene
		current_scene = SceneManager.GetActiveScene().buildIndex;

		if (!movement_enabled) {
			//rb.velocity = Vector3.zero;
			rb.velocity = new Vector2(0, rb.velocity.y);
			animator.SetBool("walking", false);
			animator.SetBool("jumping", rb.velocity.y > 0.1f);
			animator.SetBool("falling", rb.velocity.y < -0.1f);
			return;
		}

		// Get input
		float x_input = Input.GetAxis(horizontal_axis);
		float x_input_raw = Input.GetAxisRaw(horizontal_axis);
		float y_input_raw = Input.GetAxisRaw(vertical_axis);

		// Horizontal movement
		transform.Translate(Mathf.Abs(x_input) * (-1f) * x_mult * Time.deltaTime, 0, 0);
		//rb.velocity += new Vector2(x_input * x_mult * Time.deltaTime, 0);

		// Jump controls
		//Vector3 feet_pos = new Vector3(transform.position.x, col.bounds.min.y);
		Vector3 left_foot_pos = new Vector3(col.bounds.min.x + 0.05f, col.bounds.min.y);
		Vector3 right_foot_pos = new Vector3(col.bounds.max.x - 0.05f, col.bounds.min.y);
		RaycastHit2D raycast = Physics2D.Raycast(left_foot_pos, Vector2.down, raycast_dist, landing_layer_mask);
		if (raycast.collider == null) {
			raycast = Physics2D.Raycast(right_foot_pos, Vector2.down, raycast_dist, landing_layer_mask);
		}

		if (raycast.collider != null && rb.velocity.y <= 0) {
			jump_grounded_check = true;
			jump_grounded_delay = ghost_jump_delay;
		} else if (jump_grounded_delay <= 0 || rb.velocity.y > jump_velo * 0.2) {
			jump_grounded_check = false;
			jump_grounded_delay = 0;
		} else {
			jump_grounded_delay -= Time.deltaTime;
		}
		
		if (jump_grounded_check && y_input_raw > 0 && !jump_held) {
			jump();
		}

		if (y_input_raw <= 0) {
			jump_held = false;
		} else {
			jump_held = true;
		}

		// Fall through platforms if you are holding down
		//col.enabled = y_input >= 0;

		// Check if you should be walking
		if (Mathf.Abs(x_input_raw) > 0 && Mathf.Abs(rb.velocity.y) < 0.1f) {
			animator.SetBool("walking", true);
		} else {
			animator.SetBool("walking", false);
		}

		// Check if you should be jumping
		if (rb.velocity.y >= 0.1f) {
			animator.SetBool("jumping", true);
		} else {
			animator.SetBool("jumping", false);
		}

		// Check if you should be falling
		if (rb.velocity.y < 0) {
			rb.gravityScale = base_grav_scale * falling_grav_mult;
			animator.SetBool("falling", true);
		} else {
			animator.SetBool("falling", false);
			if (y_input_raw > 0) {
				rb.gravityScale = base_grav_scale;
			} else {
				rb.gravityScale = base_grav_scale * falling_grav_mult;
			}
		}

		// Transform flip
		if (x_input > 0) {
			transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, 180f, transform.rotation.z));
		} else if (x_input < 0) {
			transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, 0, transform.rotation.z));
		}
	}

	public static void set_movement_enabled(bool enabled, bool no_gravity = false) {
		movement_enabled = enabled;
		if (no_gravity) {
			player_instance.rb.gravityScale = enabled ? player_instance.base_grav_scale : 0;
		}
	}

	// Jump!
	private void jump() {
		rb.velocity = new Vector2(rb.velocity.x, jump_velo);
		//SoundManager.instance.playJump();
		// todo - jump sound
	}
}

