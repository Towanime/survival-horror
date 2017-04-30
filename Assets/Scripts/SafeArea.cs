using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        GameObject.FindGameObjectWithTag("GameStateMachine").SendMessage("OnPlayerEnterSafeArea");
    }

    void OnTriggerExit(Collider other)
    {
        GameObject.FindGameObjectWithTag("GameStateMachine").SendMessage("OnPlayerExitSafeArea");
    }
}
