using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class physical_item : MonoBehaviour {

	// Public vars
	public item Item;
	public player thrower;
	public bool just_thrown = false;

	// Static settings
	public static int expiration_time = 6;
	public static float flying_velo_threshold = 1f;

	// Component references
	private SpriteRenderer sr;
	private Rigidbody2D rb;


	// Init
	void Awake() {
		sr = GetComponentInChildren<SpriteRenderer>();
		rb = GetComponent<Rigidbody2D>();
	}
	private void Start() {
		sr.sprite = Item.icon;
		StartCoroutine(set_just_thrown_delayed(false, 0.3f));
	}

	// Called every frame
	private void Update() {
		// TODO - disappears after a few seconds, and flashes.
	}

	// Check for collisions with players
	private void OnCollisionEnter2D(Collision2D collision) {
		// Check if this is flying through the air
		bool flying = rb.velocity.magnitude > flying_velo_threshold;

		if (collision.gameObject.CompareTag("Player")) {
			player Player = collision.gameObject.GetComponent<player>();
			if (Player == thrower) {
				if (just_thrown) {
					just_thrown = false;
				} else {
					try_pickup(Player);
				}
			} else if (Player != thrower) {
				if (flying) {
					// Stun them
					Player.GetComponent<movement>().stun(Item);
				} else {
					try_pickup(Player);
				}
			}
		}
	}

	// A player should pick this item up, if possible
	private void try_pickup(player Player) {
		if (!Player.hands_full) {
			Player.items_carried.Add(Item);
			destroy_this();
		}
	}

	// Destroy this object
	private void destroy_this() {
		Destroy(gameObject);
	}

	// Set just_thrown after a short delay
	private IEnumerator set_just_thrown_delayed(bool val, float delay) {
		yield return new WaitForSeconds(delay);
		just_thrown = val;
	}
}
