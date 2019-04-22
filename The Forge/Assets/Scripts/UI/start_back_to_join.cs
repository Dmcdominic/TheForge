using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class start_back_to_join : MonoBehaviour {

	public SpriteRenderer back;
	public SpriteRenderer start;

	// Start is called before the first frame update
	void Start() {
		back.color = teams.lighter_colors[0];
		start.color = teams.lighter_colors[1];
	}
	
}
