using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour {

    public bool playSfx;
    public float sfxFadeSpeed = 0.25f;
    public SoundId sfx;
    private GameObject gameStateMachine;

    void Start()
    {
        gameStateMachine = GameObject.FindGameObjectWithTag("GameStateMachine");
    }

    void OnTriggerEnter(Collider other)
    {
        if (playSfx)
        {
            SoundManager.Instance.FadeIn(sfx, sfxFadeSpeed).Loop();
        }
        if (gameStateMachine != null)
        {
            gameStateMachine.SendMessage("OnPlayerEnterSafeArea");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (playSfx)
        {
            SoundManager.Instance.FadeOut(sfx, sfxFadeSpeed, true);
        }
        if (gameStateMachine != null)
        {
            gameStateMachine.SendMessage("OnPlayerExitSafeArea");
        }
    }
}
