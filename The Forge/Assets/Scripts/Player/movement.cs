using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(player), typeof(Animator))]
public class movement : MonoBehaviour {
	// Public fields
	public float speed;
	public float vertical_height = 40f;

	public GameObject dwarf_visuals;
	public GameObject carried_items;

	[HideInInspector]
	public bool movement_enabled;

	// Private vars
	private int index;
	private bool teledown = false;

	private player Player;
	private Animator animator;


	// Init
	private void Awake() {
		Player = GetComponent<player>();
		animator = GetComponent<Animator>();
		index = Player.index;

		movement_enabled = true;
	}

	// Update is called once per frame
	void Update() {
		if (!movement_enabled) {
			animator.SetBool("running", false);
			animator.SetBool("carrying", Player.carrying_items);
			return;
		}

		float move_hor = input.p[index].h_axis * speed * Time.deltaTime;
		float move_ver_input = input.p[index].v_axis;

		bool tp_up = move_ver_input > 0.7f;
		bool tp_down = move_ver_input < -0.7f;
		float move_ver = 0f;

		//if (input.p[index].switch_floors && teledown) {
		if (tp_up && teledown) {
			move_ver = vertical_height;
			teledown = false;
		//} else if (input.p[index].switch_floors && !teledown) {
		} else if (tp_down && !teledown) {
			move_ver = -vertical_height;
			teledown = true;
		}

		Vector2 movement = new Vector2(move_hor, move_ver);
		transform.Translate(movement);

		// Update animations
		animator.SetBool("running", Mathf.Abs(move_hor) > 0);
		animator.SetBool("carrying", Player.carrying_items);

		// Update visual orientation
		if (move_hor > 0) {
			set_rot_y(dwarf_visuals.transform, 180f);
			set_rot_y(carried_items.transform, 180f);
		} else if (move_hor < 0) {
			set_rot_y(dwarf_visuals.transform, 0);
			set_rot_y(carried_items.transform, 0);
		}
	}

	// Rotate an object to a certain y-rotation value
	private void set_rot_y(Transform trans, float val) {
		trans.rotation = Quaternion.Euler(new Vector3(trans.rotation.x, val, trans.rotation.z));
	}
}
