using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerups_display : MonoBehaviour {

	// Public fields
	public int team;

	public Transform red_spritemask;
	public Transform diamond_spritemask;
	public Transform green_spritemask;

	public float red_min_y;
	public float diamond_min_y;
	public float green_min_y;

	public float red_max_y;
	public float diamond_max_y;
	public float green_max_y;


	// Called once per frame
	void Update() {
		float t = powerups_controller.get_duration(team, powerups.speed) / powerups_controller.powerup_duration;
		float new_y = Mathf.Lerp(red_min_y, red_max_y, t);
		position_util.set_pos_y(red_spritemask, new_y, true);

		t = powerups_controller.get_duration(team, powerups.quick_craft) / powerups_controller.powerup_duration;
		new_y = Mathf.Lerp(diamond_min_y, diamond_max_y, t);
		position_util.set_pos_y(diamond_spritemask, new_y, true);

		t = powerups_controller.get_duration(team, powerups.jump) / powerups_controller.powerup_duration;
		new_y = Mathf.Lerp(green_min_y, green_max_y, t);
		position_util.set_pos_y(green_spritemask, new_y, true);
	}
	
}
