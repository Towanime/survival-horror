using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotusParticleTrigger : MonoBehaviour {

	public CutsceneController cutsceneController;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LotusParticleTriggerz() {
		cutsceneController.sunLotusParticles.SetActive (true);
	}
}
