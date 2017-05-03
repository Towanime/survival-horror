using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LycanAnimationEventListener : MonoBehaviour {

	void OnAnimationFinished ()
    {
        GameObject.FindGameObjectWithTag("GameStateMachine").SendMessage("OnGameOverSequenceEnded");
    }
}
