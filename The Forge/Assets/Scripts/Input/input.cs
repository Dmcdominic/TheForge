using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

/// <summary>
/// Custom input manager for consolidating controller and keyboard inputs
/// </summary>
public static class input {
	public static player[] p = {
		new player(0, XboxController.First, "Horizontal_1", "Vertical_1", KeyCode.E, KeyCode.Q),
		new player(1, XboxController.Second, "Horizontal_2", "Vertical_2", KeyCode.RightShift, KeyCode.Slash),
		new player(2, XboxController.Third, null, null, KeyCode.None, KeyCode.None),
		new player(3, XboxController.Fourth, null, null, KeyCode.None, KeyCode.None)
	};

	/// <summary>
	/// A player, from an input standpoint
	/// </summary>
	public class player {
		public int index;
		private XboxController controller;
		private string keyboard_H;
		private string keyboard_V;
		private KeyCode interact_key;
		private KeyCode hand_tool_key;

		public player(int _index, XboxController _controller, string _keyboard_H, string _keyboard_V, KeyCode _interact_key, KeyCode _hand_tool_key) {
			index = _index;
			controller = _controller;
			keyboard_H = _keyboard_H;
			keyboard_V = _keyboard_V;
			interact_key = _interact_key;
			hand_tool_key = _hand_tool_key;
		}

		// Buttons
		public bool interact {
			get { Input.GetKeyDown(KeyCode.E); return XCI.GetButtonDown(XboxButton.Y, controller) || Input.GetKeyDown(interact_key); }
		}

		bool hand_tool {
			get { return XCI.GetButtonDown(XboxButton.A, controller) || Input.GetKeyDown(hand_tool_key); ; }
		}

		//bool tp_up {
		//	get { return XCI.GetButtonDown(XboxButton.RightBumper, controller); }
		//}
		//bool tp_down {
		//	get { return XCI.GetButtonDown(XboxButton.LeftBumper, controller); }
		//}
		public bool switch_floors {
			get { return XCI.GetButtonDown(XboxButton.LeftBumper, controller) || XCI.GetButtonDown(XboxButton.RightBumper, controller); }
		}

		public bool start {
			get { return XCI.GetButtonDown(XboxButton.Start, controller); }
		}

		// Axes
		public float h_axis {
			get {
				float xbox = XCI.GetAxis(XboxAxis.LeftStickX, controller);
				if (keyboard_H == null) {
					return xbox;
				}
				float keyboard = Input.GetAxis(keyboard_H);
				return (xbox != 0) ? xbox : keyboard;
			}
		}

		public float v_axis {
			get {
				float xbox = XCI.GetAxis(XboxAxis.LeftStickY, controller);
				if (keyboard_V == null) {
					return xbox;
				}
				float keyboard = Input.GetAxis(keyboard_V);
				return (xbox != 0) ? xbox : keyboard;
			}
		}
	}
}
