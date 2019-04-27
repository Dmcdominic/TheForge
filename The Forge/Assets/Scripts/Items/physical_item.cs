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
	private bool picked_up = false;

	// Static settings
	public static readonly float expiration_time = 7f;
	public static readonly float flashing_time = 3f;
	public static readonly float flying_velo_threshold = 1.5f;
	public static int limit_per_item = 4;

	// Component references
	private SpriteRenderer sr;
	private Rigidbody2D rb;


	// Init
	void Awake() {
		sr = GetComponentInChildren<SpriteRenderer>();
		rb = GetComponent<Rigidbody2D>();
	}
	private void Start() {
		add_new_physical(this);
		sr.sprite = Item.icon;
		StartCoroutine(set_just_thrown_delayed(false, 0.1f));
		StartCoroutine(expire_after_delay());
	}

	// Called every frame
	private void Update() {
		// Update flying value
		flying = rb.velocity.magnitude > flying_velo_threshold;
	}

	// Check for collisions with players
	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.CompareTag("Player")) {
			player Player = collision.gameObject.GetComponent<player>();
			if (Player.team == thrower.team) {
				if (just_thrown && Player == thrower) {
					just_thrown = false;
				} else {
					try_pickup(Player);
				}
			} else if (Player.team != thrower.team && !Player.Movement.stunned) {
				if (flying || just_thrown) {
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
		if (!Player.hands_full && !picked_up) {
			Player.items_carried.Add(Item);
			picked_up = true;
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

	// ======== Physical item limit management ==========
	private static Dictionary<item, Queue<physical_item>> _all_physicals;
	public static Dictionary<item, Queue<physical_item>> all_physicals {
		get {
			if (_all_physicals == null) {
				init_all_physicals();
			}
			return _all_physicals;
		}
	}

	public static void init_all_physicals() {
		_all_physicals = new Dictionary<item, Queue<physical_item>>();
		foreach (item Item in items_oracle.all) {
			_all_physicals.Add(Item, new Queue<physical_item>());
		}
	}

	private static void add_new_physical(physical_item physical_Item) {
		item Item = physical_Item.Item;
		Queue<physical_item> queue = all_physicals[Item];
		queue.Enqueue(physical_Item);

		while (queue.Peek() == null) {
			queue.Dequeue();
		}

		while (queue.Count > limit_per_item) {
			queue.Dequeue().destroy_this();
		}
	}
}
