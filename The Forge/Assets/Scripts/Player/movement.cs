using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(Animator))]
[RequireComponent(typeof(player))]
public class movement : MonoBehaviour {
	// Static settings
	public static float throw_speed_fast = 7f;
	public static float throw_speed_slow = 4f;
	public static float throw_turn_duration = 0.15f;
	public static float ladder_axis_threshold = 0.6f;
	public static float stacking_height_check = 0.55f;

	// Public fields
	public float x_mult;

	public float jump_velo;
	public float falling_grav_mult;
	public float ghost_jump_delay;
	public float raycast_dist;

	public float ladder_velo;

	public Transform no_rotation;

	public physical_item physical_item_prefab;

	[HideInInspector]
	public bool stunned;
	[HideInInspector]
	public bool invuln;

	[HideInInspector]
	public Vector2 platform_velo;
	
	// Movement management
	private bool movement_enabled;
	public static bool all_players_frozen = false;
	public bool can_move {
		get { return movement_enabled && !stunned && !all_players_frozen; }
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
	private float ladder_pos;

	private bool just_threw_left = false;
	private bool just_threw_right = false;

	private List<player> players_touching = new List<player>();

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
	}

	// Complete the initialization after other Awake methods have been called.
	private void Start() {
		index = Player.index;
		movement_enabled = true;
	}

	// Called once per frame
	void Update() {
		if (!can_move) {
			if (stunned) {
				rb.velocity = new Vector2(0, rb.velocity.y) + platform_velo;
			} else {
				rb.velocity = new Vector2(0, 0) + platform_velo;
			}
			col.isTrigger = false;
			animator.SetBool("running", false);
			animator.SetBool("carrying", Player.carrying_items);
			//animator.SetBool("stunned", stunned);
			update_grav_scale();
			return;
		}

		// Get input
		float x_input = input.p[index].h_axis;
		bool jump_pressed = input.p[index].jump;
		float y_input = input.p[index].v_axis;

		// Horizontal movement
		float movespeed = x_input * x_mult * powerups_controller.speed_mult(Player);
		rb.velocity = new Vector2(movespeed, rb.velocity.y) + platform_velo;
		platform_velo = new Vector2(0, 0);

		// Check footing
		Vector3 left_foot_pos = new Vector3(col.bounds.min.x, col.bounds.min.y);
		Vector3 right_foot_pos = new Vector3(col.bounds.max.x, col.bounds.min.y);

		bool has_footing = false;
		RaycastHit2D[] hits = Physics2D.RaycastAll(left_foot_pos, Vector2.down, raycast_dist);
		for (int i = 0; i < hits.Length; i++) {
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

		// Ladder check
		bool up_ladder = touching_ladder && y_input > ladder_axis_threshold;
		bool down_ladder = touching_ladder && y_input < -ladder_axis_threshold;
		if (!touching_ladder || rb.velocity.y <= -0.1f) {
			just_jumped_off_ladder = false;
		}

		holding_onto_ladder = !just_jumped_off_ladder && (up_ladder || down_ladder || (touching_ladder && holding_onto_ladder));
		if (holding_onto_ladder) {
			col.isTrigger = true;
			clear_all_players_touching();
			update_velo_on_ladder(up_ladder, down_ladder, has_footing);
		} else {
			col.isTrigger = false;
			//clear_all_players_touching();
		}

		// Jump controls

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

		// Check for throwing your items
		bool try_throw = input.p[index].throw_left || input.p[index].throw_right;
		if (try_throw && Player.carrying_items) {
			float x_dir = input.p[index].throw_left ? -1f : 1f;

			if (input.p[index].throw_left) {
				StartCoroutine(set_just_threw(false));
			} else {
				StartCoroutine(set_just_threw(true));
			}

			Vector2 direction_fast = new Vector2(x_dir, 1f);
			int fast_index = 0;
			if (Player.items_carried.Count == 2) {
				Vector2 direction_slow = new Vector2(x_dir, 0.5f);
				throw_item(0, direction_slow.normalized * throw_speed_slow);
				fast_index = 1;
			}
			direction_fast = new Vector2(x_dir, 0.4f);
			throw_item(fast_index, direction_fast.normalized * throw_speed_fast);
			Player.items_carried.Clear();
		}

		// Check if you should be walking
		bool running = Mathf.Abs(x_input) > 0 && Mathf.Abs(rb.velocity.y) < 0.1f;
		animator.SetBool("running", running);

		// Check if you should be jumping
		bool jumping = rb.velocity.y >= 0.1f;
		//animator.SetBool("jumping", jumping);

		update_grav_scale();

		// Transform flip
		if (just_threw_right || (!just_threw_left && x_input > 0)) {
			transform.localRotation = Quaternion.Euler(new Vector3(transform.localRotation.x, 180f, transform.localRotation.z));
			no_rotation.localRotation = Quaternion.Euler(new Vector3(no_rotation.localRotation.x, 180f, no_rotation.localRotation.z));
		} else if (just_threw_left || (!just_threw_right && x_input < 0)) {
			transform.localRotation = Quaternion.Euler(new Vector3(transform.localRotation.x, 0, transform.localRotation.z));
			no_rotation.localRotation = Quaternion.Euler(new Vector3(no_rotation.localRotation.x, 0, no_rotation.localRotation.z));
		}
		
		// Adjust the velocity of whoever is standing on you
		foreach (player p in players_touching) {
			if (p.transform.position.y > transform.position.y + stacking_height_check) {
				if (p.Movement.platform_velo.x * rb.velocity.x < 0) {
					p.Movement.platform_velo += new Vector2(rb.velocity.x, 0);
				} else
				
				if (p.Movement.platform_velo.magnitude < Mathf.Abs(rb.velocity.x)) {
					p.Movement.platform_velo = new Vector2(rb.velocity.x, 0);
				}
			}
		}

		// Update animations
		animator.SetBool("carrying", Player.carrying_items);
	}


	// Jump!
	private void jump() {
		rb.velocity = new Vector2(rb.velocity.x, jump_velo * powerups_controller.jump_mult(Player));
		holding_onto_ladder = false;
		if (touching_ladder) {
			just_jumped_off_ladder = true;
		}
		// todo - jump sound
		//SoundManager.instance.playJump();
	}

	// Stun this player briefly
	public void stun(item Item) {
		stunned = true;
		invuln = true;
		rb.velocity = new Vector2(rb.velocity.x, jump_velo * .9f);
		sound_manager.play_one_shot(sound_manager.instance.punch);
		StartCoroutine(unstun_delayed(0.7f + Item.computed_gold_val * 0.01f, 1f));
	}

	private IEnumerator unstun_delayed(float unstun_delay, float extra_invuln_delay) {
		yield return new WaitForSeconds(unstun_delay);
		stunned = false;
		yield return new WaitForSeconds(extra_invuln_delay);
		invuln = false;
	}

	// Throw an item with certain velocity
	private void throw_item(int index, Vector2 velocity) {
		item Item = Player.items_carried[index];
		physical_item physical_Item = Instantiate(physical_item_prefab);

		physical_Item.Item = Item;
		physical_Item.thrower = Player;
		physical_Item.just_thrown = true;

		physical_Item.transform.position = Player.carried_item_srs[index].transform.position;
		physical_Item.GetComponent<Rigidbody2D>().velocity = velocity;
	}

	// Update just_threw (right or left) after short delay
	private IEnumerator set_just_threw(bool right) {
		if (right) {
			just_threw_right = true;
		} else {
			just_threw_left = true;
		}
		yield return new WaitForSeconds(throw_turn_duration);
		if (right) {
			just_threw_right = false;
		} else {
			just_threw_left = false;
		}
	}

	// Ladder usage
	private void update_velo_on_ladder(bool up_ladder, bool down_ladder, bool has_footing) {
		if (!holding_onto_ladder) {
			return;
		}
		if (down_ladder && ladder_pos > transform.position.y && has_footing) {
			rb.velocity = new Vector2(rb.velocity.x, 0);
			return;
		}
		if (up_ladder) {
			rb.velocity = new Vector2(rb.velocity.x, ladder_velo * powerups_controller.speed_ladder_mult(Player));
		} else if (down_ladder) {
			rb.velocity = new Vector2(rb.velocity.x, -ladder_velo * powerups_controller.speed_ladder_mult(Player));
		} else {
			rb.velocity = new Vector2(rb.velocity.x, 0);
		}
	}

	// Update the rigidbody's gravity scale based on whether or not you're falling
	private void update_grav_scale() {
		//float y_input = input.p[index].v_axis;
		//bool jump_held = y_input > 0;

		if (stunned) {
			rb.gravityScale = base_grav_scale * falling_grav_mult * powerups_controller.jump_grav_mult(Player);
			return;
		} else if (!can_move) {
			rb.gravityScale = 0;
			return;
		}

		bool jump_held = input.p[index].jump_held;
		if (holding_onto_ladder) {
			rb.gravityScale = 0;
		} else if (rb.velocity.y < 0) {
			rb.gravityScale = base_grav_scale * falling_grav_mult * powerups_controller.jump_grav_mult(Player);
			//animator.SetBool("falling", true);
		} else {
			//animator.SetBool("falling", false);
			if (jump_held) {
				rb.gravityScale = base_grav_scale * powerups_controller.jump_grav_mult(Player);
			} else {
				rb.gravityScale = base_grav_scale * falling_grav_mult * powerups_controller.jump_grav_mult(Player);
			}
		}
	}

	// Ladder management
	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.CompareTag("Ladder")) {
			ladder_count++;
			ladder_pos = collision.transform.position.y;
		}
	}
	private void OnTriggerExit2D(Collider2D collision) {
		if (collision.CompareTag("Ladder")) {
			ladder_count--;
		}
	}

	// Player stacking management
	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.CompareTag("Player")) {
			player p = collision.gameObject.GetComponent<player>();
			players_touching.Add(p);
			if (p.transform.position.y > transform.position.y + stacking_height_check) {
				p.Movement.platform_velo = new Vector2(rb.velocity.x, 0);
			}
		}
	}

	private void OnCollisionExit2D(Collision2D collision) {
		if (collision.gameObject.CompareTag("Player")) {
			player p = collision.gameObject.GetComponent<player>();
			if (p != null) {
				players_touching.Remove(p);
				p.Movement.platform_velo = new Vector2(0, 0);
			}
		}
	}

	// Remove a single player from the players_touching list
	public void remove_player_touching(player Player) {
		players_touching.Remove(Player);
	}

	// Clear all players from your players_touching list, and yourself from their lists
	public void clear_all_players_touching() {
		while (players_touching.Count > 0) {
			players_touching[0].Movement.remove_player_touching(Player);
			players_touching.RemoveAt(0);
		}
		players_touching.Clear();
	}
}
