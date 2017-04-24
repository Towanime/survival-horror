using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

public class GameStateMachine : MonoBehaviour {

    public DecoyManager decoyManager;

    private GameObject player;
    private SunCrystalCircleMeter sunCrystalCircleMeter;

    private StateMachine<GameStates> fsm;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        sunCrystalCircleMeter = player.GetComponentInChildren<SunCrystalCircleMeter>();
        fsm = StateMachine<GameStates>.Initialize(this, GameStates.Running);
    }

    void Running_Update()
    {
        // Missing check if player is in safe zone
        decoyManager.Active = !sunCrystalCircleMeter.IsLit;
    }
}
