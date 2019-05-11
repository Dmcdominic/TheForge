using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class grindstone : MonoStation {

	// Public fields
	public GameObject completed_indicator;
	public float deltaY;
	public float deltaX;
	float oldX;
	float oldY;
	Vector2 inputdirect;

	// Private vars
	private player current_owner;
	private bool finished_grinding;
	private bool is_playing = false;

	private int index;
	private int player_num;

	private int rotatecount = 0;
	private float MinRotSpd = 0.1f;

	private TextMeshPro player_indicator;


	// Init
	private void Awake() {
		player_indicator = completed_indicator.GetComponentInChildren<TextMeshPro>();

		current_owner = null;
		finished_grinding = false;
		completed_indicator.SetActive(false);
	}

	public override void on_interact(player Player) {
		base.on_interact(Player);
		player_num = Player.index;
		index = Player.index;
		is_playing = true;
	}

	// Called every frame
	protected override void Update() {
		base.Update();
		// Your stuff here

		if (is_playing == true) {
			//index = Player.index;
			float x_input = input.p[index].h_axis;
			float y_input = input.p[index].v_axis;

			finished_grinding = false;
			sound_manager.update_loop(sound_manager.instance.whetstone_loop, true);

			float deltaValuex = x_input - oldX;
			float deltaValuey = y_input - oldY;
			oldX = x_input;
			oldY = y_input;
			inputdirect = new Vector2(0, 1);

			if (Mathf.Abs(deltaValuex) < MinRotSpd && Mathf.Abs(deltaValuey) < MinRotSpd) {
				print("not fast");
				rotatecount = 0;
			}

			if (Mathf.Abs(deltaValuex) >= MinRotSpd || Mathf.Abs(deltaValuey) >= MinRotSpd) {
				print("fast");
				inputdirect = new Vector2(x_input, y_input);
				if (x_input == inputdirect.x && y_input == inputdirect.y) {
					rotatecount += 1;
					print(rotatecount);
				}
				if (rotatecount >= 3 && is_playing) {
					on_done_grinding();
					rotatecount = 0;
					is_playing = false;
				}
			}
		}
	}

	// Called when the product is done grinding
	private void on_done_grinding() {
		complete_items_swap();
		finished_grinding = true;
		completed_indicator.SetActive(true);
		//player_indicator.text = player.get_indicator_string(current_owner.index);
		sound_manager.update_loop(sound_manager.instance.whetstone_loop, false);
	}

	// More specific can_interact
	public override bool can_interact(player Player) {
		return (current_owner == null && base.can_interact(Player)) || (current_owner == Player && finished_grinding);
	}
}