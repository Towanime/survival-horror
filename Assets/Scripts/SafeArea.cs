using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour {

    public float sfxFadeInSpeed = 1;
    public float sfxFadeOutSpeed = 1;

    void OnTriggerEnter(Collider other)
    {
        SoundManager.Instance.FadeOut(SoundId.FOREST_AMBIENT, sfxFadeOutSpeed);
        GameObject.FindGameObjectWithTag("GameStateMachine").SendMessage("OnPlayerEnterSafeArea");
    }

    void OnTriggerExit(Collider other)
    {
        SoundManager.Instance.FadeIn(SoundId.FOREST_AMBIENT, sfxFadeInSpeed);
        GameObject.FindGameObjectWithTag("GameStateMachine").SendMessage("OnPlayerExitSafeArea");
    }
}
