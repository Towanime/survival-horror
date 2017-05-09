using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanicButton : MonoBehaviour {
    public GameObject player;
    public KeyCode panicKey;
    
    // Update is called once per frame
	void Update () {
        if (player.activeInHierarchy && Input.GetKey(panicKey))
        {
            GameObject.FindGameObjectWithTag("GameStateMachine").SendMessage("OnGameOverSequenceEnded");
        }
    }
}
