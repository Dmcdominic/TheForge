﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crate : MonoStation {

	public SpriteRenderer item_contained_SR;


	// Init
	private void Awake() {
		item_contained_SR.sprite = station_info.products[0].icon;
	}

	public override void on_interact(player Player) {
		base.on_interact(Player);
		complete_items_swap();
	}
}
