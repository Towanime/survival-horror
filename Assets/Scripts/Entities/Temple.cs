using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temple : MonoBehaviour {

    public float templeSfxFadeSpeed = 0.25f;
    public float forestSfxFadeSpeed = 3f;

    void OnTriggerEnter(Collider other)
    {
        SoundManager.Instance.FadeIn(SoundId.TEMPLE_AMBIENT, templeSfxFadeSpeed);
        SoundManager.Instance.FadeOut(SoundId.FOREST_AMBIENT, forestSfxFadeSpeed);
        GameObject.FindGameObjectWithTag("GameStateMachine").SendMessage("OnPlayerEnterSafeArea");
    }

    void OnTriggerExit(Collider other)
    {
        SoundManager.Instance.FadeOut(SoundId.TEMPLE_AMBIENT, templeSfxFadeSpeed);
        SoundManager.Instance.FadeIn(SoundId.FOREST_AMBIENT, forestSfxFadeSpeed);
        GameObject.FindGameObjectWithTag("GameStateMachine").SendMessage("OnPlayerExitSafeArea");
    }
}
