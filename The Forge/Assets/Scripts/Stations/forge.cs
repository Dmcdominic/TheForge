using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class forge : MonoStation {
	public override void on_interact(player Player) {
		base.on_interact(Player);
		StartCoroutine(brief_forge_audio());
		// todo - play minigame
		complete_items_swap();
	}

	private IEnumerator brief_forge_audio() {
		sound_manager.instance.update_loop(sound_manager.instance.furnace_loop, true);
		yield return new WaitForSeconds(1f);
		sound_manager.instance.update_loop(sound_manager.instance.furnace_loop, false);
	}
}
