using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class camera_experiment : MonoBehaviour {

	private PixelPerfectCamera ppCam;

	private float ppu_vs_rrX;
	private float ppu_vs_rrY;


	// Init
	private void Awake() {
		ppCam = GetComponent<PixelPerfectCamera>();
		print("0: " + ppCam.assetsPPU);
		ppu_vs_rrX = (float)ppCam.assetsPPU / ppCam.refResolutionX;
		ppu_vs_rrY = (float)ppCam.assetsPPU / ppCam.refResolutionY;
		print("1: " + ppu_vs_rrX);
		print("2: " + ppu_vs_rrY);
	}

	// Update is called once per frame
	void Update() {

		if (ppCam.assetsPPU > 1) {
			//ppCam.assetsPPU -= Random.Range(0, 3) == 0 ? 1 : 0;
			//ppCam.refResolutionX = Mathf.RoundToInt(ppCam.assetsPPU / ppu_vs_rrX);
			//ppCam.refResolutionY = Mathf.RoundToInt(ppCam.assetsPPU / ppu_vs_rrY);
			print("unrounded: " + ppCam.assetsPPU / ppu_vs_rrX);
			print("rounded: " + Mathf.RoundToInt(ppCam.assetsPPU / ppu_vs_rrX));
		}
	}
}
