using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTriggers : MonoBehaviour {

	public CutsceneController cutsceneController;
	public AudioSource energyBallAudio;
	// Use this for initialization
	void Start () {
		energyBallAudio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LotusParticleTrigger() {
		cutsceneController.sunLotusParticles.SetActive (true);
	}

	public void CameraSwitchPos2() {
		cutsceneController.cutsceneCam.transform.position = cutsceneController.cameraPos2.transform.position;
		cutsceneController.cutsceneCam.transform.rotation = cutsceneController.cameraPos2.transform.rotation;

	}

	public void CameraSwitchPos3() {
		cutsceneController.WhiteFadeInOut ();
		energyBallAudio.Play ();
	}
}
