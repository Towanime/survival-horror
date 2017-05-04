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
	public bool cameraPanStick;

	public Color fadeColor;
	public float fadeTime;
	private float currentTime;
	public Image fadeImage;

	public GameObject cutsceneCam;
	public GameObject cameraPos1;
	public GameObject cameraPos2;
	public GameObject cameraPos3;
	public GameObject camera3Pan;
	public GameObject playerController;
	public GameObject sunLotusParticles;
	public GameObject pedestalParticles;
	public GameObject safeZoneParticles;
	public GameObject sunLotus;
	public GameObject fadeOverlay;
	public GameObject monumentTop;
	public GameObject monumentSpike1;
	public GameObject monumentSpike2;
	public GameObject monumentSpike3;
	public GameObject monumentSpike4;
	public GameObject monumentSpike5;
	public GameObject monumentSpike6;
	public GameObject monumentPillar;
	public GameObject monumentDarkPillar;

	public Light monumentLight;

	public Material shinyTop;
	public Material shinySpike;
	public Material shinyPillar;


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

        // Check white fade
        UpdateFadeTimeline();

		if (cameraPanStick == true) {
			camera3Pan.SetActive (true);
			cutsceneCam.transform.position = cameraPos3.transform.position;
			cutsceneCam.transform.rotation = cameraPos3.transform.rotation;
		}
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

	public void WhiteFadeInOut()
    {
        // this is only called once so use it to initialize the fade
        // change color to white but also no alpha
        Color whiteNoAlpha = Color.white;
        whiteNoAlpha.a = 0;
        fadeImage.color = whiteNoAlpha;
        currentTime = 0;
        // start the fade chain
        fadeInActiveWhite = true;
    }

    private void UpdateFadeTimeline()
    {
        // check fade in!
        if (cutsceneActive == true && fadeInActiveWhite == true)
        {
            // begin or continue fading until it's done
            fadeInActiveWhite = FadeAlpha(0, 1, 0.2f); // alpha 0 to 1 (completely opaque)
            // fadeInActiveWhite it will go false when the fade is complete, breaking out of this if forevah*
            // if it is false then it can begin the fade out
            if (fadeInActiveWhite == false) // extra points if you get this #yu-gi-oh chain effect
            {
                fadeOutActiveWhite = true; // starts next item in the chain
            }
        }

        // check fade out
        if (cutsceneActive == true && fadeOutActiveWhite == true)
        {
			safeZoneParticles.SetActive (true);
			cameraPanStick = true;
			MonumentColorShift ();
            // begin or continue fading until it's done
            fadeOutActiveWhite = FadeAlpha(1, 0, 5f); // alpha 1 to 0
            // fadeOutActiveWhite it will go false when the fade is complete
            // if it is false then it can begin the next step ???
			if (fadeOutActiveWhite == false && camera3Pan.GetComponent<Transform>().rotation.x <= -20) // extra points if you get this #yu-gi-oh chain effect
            {
				// fadeInActiveBlack true here?
            }
        }
    }

    /// <summary>
    /// Fades alpha between the provided values at a specific rate (in seconds).
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="time"></param>
    /// <returns>It will return false when the fading is complete.</returns>
    public bool FadeAlpha(float from, float to, float time)
    {
        // how much time has passed?
        float traversedTime = currentTime / time;
        // get color value
        fadeColor = fadeImage.color;
        // lerp alpha
        fadeColor.a = Mathf.Lerp(from, to, traversedTime);
        currentTime += Time.deltaTime;
        fadeImage.color = fadeColor;

        // 1 means the lerp is complete
        if (traversedTime >= 1)
        {
            currentTime = 0;
            return false;
        }
        else
        {
            return true;
        }
    }

	void MonumentColorShift() {
		monumentTop.GetComponent<Renderer> ().material = shinyTop;
		monumentSpike1.GetComponent<Renderer> ().material = shinySpike;
		monumentSpike2.GetComponent<Renderer> ().material = shinySpike;
		monumentSpike3.GetComponent<Renderer> ().material = shinySpike;
		monumentSpike4.GetComponent<Renderer> ().material = shinySpike;
		monumentSpike5.GetComponent<Renderer> ().material = shinySpike;
		monumentSpike6.GetComponent<Renderer> ().material = shinySpike;
		monumentPillar.GetComponent<Renderer> ().material = shinyPillar;
		monumentDarkPillar.SetActive (false);

	}

    void LotusBloom() {
		sunLotus.GetComponent<Animator> ().speed = 1;
		sunLotus.GetComponent<Animator> ().Play ("Unfold");
	}
}
