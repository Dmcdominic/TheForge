﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class steel_crate : MonoStation {
	public override void on_interact(player Player) {
		base.on_interact(Player);
		// todo - play minigame
		complete_items_swap();
	}
}
