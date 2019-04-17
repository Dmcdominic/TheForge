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
	// Calls on_interact() if any of the touching players are trying to interact, and are able to.
	// If you want to use this in a parent class, make sure to call the base method.
	protected virtual void Update() {
		if (occupied()) {
			indicator.SetActive(false);
			return;
		}
		
		bool someone_can_interact = false;
		item craftable = null;
		foreach (player p in players_touching) {
			bool p_can_interact = can_interact(p);
			someone_can_interact = someone_can_interact || p_can_interact;
			item new_craftable = get_craftable_item(p);
			craftable = new_craftable != null ? new_craftable : craftable;
			if (input.p[p.index].interact && p_can_interact && !p.Movement.stunned) {
				on_interact(p);
				break;
			}
		}
		
		// Don't display recipes for crates
		if (craftable != null && craftable.is_base_item) {
			craftable = null;
		}

		indicator.SetActive(someone_can_interact);
		set_recipe_indicator(craftable);
	}

	// Recipe indicator management
	private recipe_indicator_control _RIC;
	private recipe_indicator_control RIC {
		get {
			if (_RIC == null) {
				_RIC = indicator.GetComponent<recipe_indicator_control>();
			}
			return _RIC;
		}
	}

	private void set_recipe_indicator(item product) {
		RIC.display_recipe_for(product);
	}

	protected virtual item get_craftable_item(player Player) {
		return null;
	}

	// Handles the player interaction indicator.
	// If you want to use this in a parent class, make sure to call the base method.
	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.CompareTag("Player")) {
			player p = collision.gameObject.GetComponent<player>();
			players_touching.Add(p);
		}
	}
	
	private void OnTriggerExit2D(Collider2D collision) {
		if (collision.CompareTag("Player")) {
			player p = collision.gameObject.GetComponent<player>();
			if (p != null) {
				players_touching.Remove(p);
			}
		}
	}

}
