using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trash_can : MonoInteractable {
	public override bool can_interact(player Player) {
		return Player.carrying_items;
	}

	public override bool occupied() {
		return false;
	}

	public override void on_interact(player Player) {
		//Player.items_carried.Clear();
		Player.items_carried.RemoveAt(Player.items_carried.Count - 1);
	}
}
