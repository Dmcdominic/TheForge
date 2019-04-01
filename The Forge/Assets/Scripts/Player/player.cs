using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour {

	public int index;

	[HideInInspector]
	public List<item> items_carried;
	public bool hands_full { get { return items_carried.Count < 2; } }
	[HideInInspector]
	public MonoStation current_station = null;


	// Init
	private void Start() {
		// TESTING
	}
}
