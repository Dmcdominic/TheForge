using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class MonoInteractable : MonoBehaviour {

	public GameObject indicator;

	// Should return true iff this station is occupied
	public abstract bool occupied();

	// Should return true iff the player can interact with this object
	public abstract bool can_interact(player Player);

	// Handle the behavior of this object when the player interacts with it
	public abstract void on_interact(player Player);


	// INDICATOR MANAGEMENT
	private List<player> players_touching = new List<player>();
	protected void reset_touching() {
		players_touching = new List<player>();
	}

	// Verifies that the indicator should be displayed.
	// Calls on_interact() if any of the touching players are 
	private void Update() {
		indicator.SetActive(!occupied() && players_touching.Count > 0);

		if (occupied()) {
			return;
		}

		player to_try_remove = null;
		foreach (player p in players_touching) {
			if (input.p[p.index].interact && can_interact(p)) {
				on_interact(p);
				to_try_remove = p;
				break;
			}
		}
		if (to_try_remove && (occupied() || !can_interact(to_try_remove))) {
			players_touching.Remove(to_try_remove);
		}
	}

	// Handles the player interaction indicator.
	// If you want to use this in a parent class, make sure to call the base method.
	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.CompareTag("Player")) {
			player p = collision.gameObject.GetComponent<player>();
			if (p != null && can_interact(p)) {
				players_touching.Add(p);
			}
		}
	}

	//private void OnCollisionExit2D(Collision2D collision) {
	private void OnTriggerExit2D(Collider2D collision) {
		if (collision.CompareTag("Player")) {
			player p = collision.gameObject.GetComponent<player>();
			if (p != null) {
				players_touching.Remove(p);
			}
		}
	}

}
