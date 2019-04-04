using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class score_display : MonoBehaviour {
	
	public TextMeshPro[] score_TMPs;


	// Regularly update the text displays
	private void Update() {
		for (int p=0; p < player.scores.Length && p < score_TMPs.Length; p++) {
			score_TMPs[p].text = "P" + (p+1).ToString() + ": " + player.scores[p].ToString();
		}
	}
}
