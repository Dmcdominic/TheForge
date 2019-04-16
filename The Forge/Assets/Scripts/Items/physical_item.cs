using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class physical_item : MonoBehaviour {

	// Public vars
	public item Item;
	public player thrower;
	public bool just_thrown = false;

	[HideInInspector]
	public bool flashing = false;

	// Private vars
	private bool flying = false;

	// Static settings
	public static float expiration_time = 7f;
	public static float flashing_time = 3f;
	public static float flying_velo_threshold = 1.5f;

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
		StartCoroutine(expire_after_delay());
	}

	// Called every frame
	private void Update() {
		// TODO - disappears after a few seconds, and flashes.

		// Update flying value
		flying = rb.velocity.magnitude > flying_velo_threshold;
	}

	// Check for collisions with players
	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.CompareTag("Player")) {
			player Player = collision.gameObject.GetComponent<player>();
			if (Player.team == thrower.team) {
				if (just_thrown) {
					just_thrown = false;
				} else {
					try_pickup(Player);
				}
			} else if (Player.team != thrower.team && !Player.Movement.stunned) {
				if (flying) {
					// Stun them
					Player.Movement.stun(Item);
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

	// Destroy this object after the appropriate delay
	private IEnumerator expire_after_delay() {
		yield return new WaitForSeconds(expiration_time - flashing_time);
		flashing = true;
		yield return new WaitForSeconds(flashing_time);
		destroy_this();
	}
}
