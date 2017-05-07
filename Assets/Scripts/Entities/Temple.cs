using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temple : MonoBehaviour {

    public GameObject gameStateMachine;
    public float templeSfxFadeSpeed = 0.25f;

    void Start()
    {
        gameStateMachine = GameObject.FindGameObjectWithTag("GameStateMachine");
    }

    void OnTriggerEnter(Collider other)
    {
        SoundManager.Instance.FadeIn(SoundId.TEMPLE_AMBIENT, templeSfxFadeSpeed).Loop();
        if (gameStateMachine != null)
        {
            gameStateMachine.SendMessage("OnPlayerEnterSafeArea");
        }
    }

    void OnTriggerExit(Collider other)
    {
        SoundManager.Instance.FadeOut(SoundId.TEMPLE_AMBIENT, templeSfxFadeSpeed, true);
        if (gameStateMachine != null)
        {
            gameStateMachine.SendMessage("OnPlayerExitSafeArea");
        }
    }
}
