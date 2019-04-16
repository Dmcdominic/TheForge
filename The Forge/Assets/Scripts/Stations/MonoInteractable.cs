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
	protected virtual void Update() {
		if (occupied()) {
			indicator.SetActive(false);
			return;
		}
		
		bool someone_can_interact = false;
		foreach (player p in players_touching) {
			bool p_can_interact = can_interact(p);
			someone_can_interact = someone_can_interact || p_can_interact;
			if (input.p[p.index].interact && p_can_interact && !p.Movement.stunned) {
				on_interact(p);
				break;
			}
		}
		
		indicator.SetActive(someone_can_interact);
	}

	// Handles the player interaction indicator.
	// If you want to use this in a parent class, make sure to call the base method.
	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.CompareTag("Player")) {
			player p = collision.gameObject.GetComponent<player>();
			players_touching.Add(p);
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
