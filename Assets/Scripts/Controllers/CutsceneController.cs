using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneController : MonoBehaviour {

	public bool cutsceneActive;
	public bool fadeInActiveBlack;
	public bool fadeOutActiveBlack;
	public bool fadeInActiveWhite;
	public bool fadeOutActiveWhite;

	public Color fadeColor;
	public float fadeTime;
	private float currentTime;

	public Image fadeImage;
	public GameObject cutsceneCam;
	public GameObject cameraPos1;
	public GameObject cameraPos2;
	public GameObject playerController;
	public GameObject sunLotusParticles;
	public GameObject pedestalParticles;
	public GameObject sunLotus;
	public GameObject fadeOverlay;
	public Light monumentLight;


	// Use this for initialization
	void Start () {
		sunLotus.GetComponent<Animator> ().speed = 0;
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
		if (fadeColor.a != 0 && cutsceneActive == true && fadeInActiveBlack == false && fadeInActiveWhite == false && fadeOutActiveWhite == false) {
			fadeOutActiveBlack = true;
		}

		if (fadeOutActiveBlack == true) {
			fadeColor = fadeImage.color;
			fadeColor.a = Mathf.Lerp(1, 0, currentTime/(fadeTime-currentTime));
			currentTime += Time.deltaTime;
			fadeImage.color = fadeColor;

			if (fadeColor.a <= 0) {
				fadeOutActiveBlack = false;
				LotusBloom ();
				currentTime = 0;
			}
		}
	}

	public void WhiteFadeInOut() {

		fadeImage.color = Color.white;

		if (fadeColor.a == 0 && cutsceneActive == true) {
			fadeInActiveWhite = true;
		}

		if (fadeInActiveWhite == true) {
			fadeColor = fadeImage.color;
			fadeColor.a = Mathf.Lerp (0, 1, currentTime / (fadeTime - currentTime));
			currentTime += Time.deltaTime;
			fadeImage.color = fadeColor;

			if (fadeColor.a >= 100) {
				fadeInActiveWhite = false;
				fadeOutActiveWhite = true;
				currentTime = 0;
			}
		}

		if (fadeOutActiveWhite == true) {
			fadeColor = fadeImage.color;
			fadeColor.a = Mathf.Lerp (1, 0, currentTime / (fadeTime - currentTime));
			currentTime += Time.deltaTime;
			fadeImage.color = fadeColor;

			if (fadeColor.a <= 0) {
				fadeOutActiveWhite = false;
				currentTime = 0;
			}
		}
	}

	void LotusBloom() {
		sunLotus.GetComponent<Animator> ().speed = 1;
		sunLotus.GetComponent<Animator> ().Play ("Unfold");
	}
}
