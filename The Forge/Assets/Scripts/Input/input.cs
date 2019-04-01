using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

/// <summary>
/// Custom input manager for consolidating controller and keyboard inputs
/// </summary>
public static class input {
	public static player[] p = {
		new player(0, XboxController.First, "Horizontal_1", "Vertical_1"),
		new player(1, XboxController.Second, "Horizontal_2", "Vertical_2"),
		new player(2, XboxController.Third, null, null),
		new player(3, XboxController.Fourth, null, null)
	};

	/// <summary>
	/// A player, from an input standpoint
	/// </summary>
	public class player {
		public int index;
		public XboxController controller;
		public string keyboard_H;
		public string keyboard_V;

		public player(int _index, XboxController _controller, string _keyboard_H, string _keyboard_V) {
			index = _index;
			controller = _controller;
			keyboard_H = _keyboard_H;
			keyboard_V = _keyboard_V;
		}

		// Buttons
		bool interact {
			get { return XCI.GetButtonDown(XboxButton.Y, controller); }
		}

		bool hand_tool {
			get { return XCI.GetButtonDown(XboxButton.A, controller); }
		}

		bool tp_up {
			get { return XCI.GetButtonDown(XboxButton.RightBumper, controller); }
		}
		bool tp_down {
			get { return XCI.GetButtonDown(XboxButton.LeftBumper, controller); }
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
