using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temple : MonoBehaviour {

    public float templeSfxFadeSpeed = 0.25f;

    void OnTriggerEnter(Collider other)
    {
        SoundManager.Instance.FadeIn(SoundId.TEMPLE_AMBIENT, templeSfxFadeSpeed).Loop();
        GameObject.FindGameObjectWithTag("GameStateMachine").SendMessage("OnPlayerEnterSafeArea");
    }

    void OnTriggerExit(Collider other)
    {
        SoundManager.Instance.FadeOut(SoundId.TEMPLE_AMBIENT, templeSfxFadeSpeed, true);
        GameObject.FindGameObjectWithTag("GameStateMachine").SendMessage("OnPlayerExitSafeArea");
    }
}
