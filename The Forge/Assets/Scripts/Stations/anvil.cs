using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class anvil : MonoStation {
	public override void on_interact(player Player) {
		base.on_interact(Player);
		sound_manager.instance.play_one_shot(sound_manager.instance.anvil_hit);
		// todo - play minigame
		complete_items_swap();
	}
}
