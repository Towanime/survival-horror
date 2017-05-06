using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monument : MonoBehaviour {

    public float sfxFadeInSpeed = 1;
    public float sfxFadeOutSpeed = 1;

    void OnTriggerEnter(Collider other)
    {
        GameObject.FindGameObjectWithTag("GameStateMachine").SendMessage("OnPlayerEnterSafeArea");
    }

    void OnTriggerExit(Collider other)
    {
        GameObject.FindGameObjectWithTag("GameStateMachine").SendMessage("OnPlayerExitSafeArea");
    }
}
