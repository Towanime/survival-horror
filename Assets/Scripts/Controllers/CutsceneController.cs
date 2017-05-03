using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneController : MonoBehaviour {

	public bool cutsceneActive;
	public bool fadeActive;

	public Color fadeColor;
	public float fadeTime;
	private float currentTime;

	public Image fadeImage;
	public GameObject cutsceneCam;
	public GameObject playerController;
	public GameObject sunLotusParticles;
	public GameObject pedestalParticles;
	public GameObject sunLotus;
	public GameObject fadeOverlay;
	public Light monumentLight;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		if (cutsceneActive == true) {
			playerController.SetActive(false);
			cutsceneCam.SetActive (true);
			pedestalParticles.SetActive(false);
			monumentLight.enabled = false;
			sunLotus.SetActive (true);
		}

		BlackFadeOut();
	}

	void BlackFadeOut() {
		if (fadeColor.a != 0 && cutsceneActive == true) {
			fadeActive = true;
		}

		if (fadeActive == true) {
			fadeColor = fadeImage.color;
			fadeColor.a = Mathf.Lerp(1, 0, currentTime/(fadeTime-currentTime));
			currentTime += Time.deltaTime;
			fadeImage.color = fadeColor;

			if (fadeColor.a <= 0) {
				fadeActive = false;
				LotusBloom ();
			}
		}
	}
	void LotusBloom() {
		sunLotus.GetComponent<Animator> ().Play ("Unfold");
	}
}
