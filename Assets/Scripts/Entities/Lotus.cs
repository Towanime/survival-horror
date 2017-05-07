using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lotus : MonoBehaviour {

    private GameObject gameStateMachine;

    void Start()
    {
        gameStateMachine = GameObject.FindGameObjectWithTag("GameStateMachine");
    }

    void Pickup ()
    {
        gameStateMachine.SendMessage("OnLotusPickup");
        Destroy(transform.parent.gameObject);
	}
}
