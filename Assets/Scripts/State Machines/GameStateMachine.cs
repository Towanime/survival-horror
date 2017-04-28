using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

public class GameStateMachine : MonoBehaviour {

    public DecoyManager decoyManager;
    public LycanStateMachine lycanStateMachine;
    public PlayerStateMachine playerStateMachine;

    private GameObject player;
    private SunCrystalCircleMeter sunCrystalCircleMeter;
    private Fog fog;
    private bool playerIsInSafeArea;

    private StateMachine<GameStates> fsm;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        fog = player.GetComponentInChildren<Fog>();
        sunCrystalCircleMeter = player.GetComponentInChildren<SunCrystalCircleMeter>();
        fsm = StateMachine<GameStates>.Initialize(this, GameStates.Running);
    }

    void Running_Enter()
    {
        playerStateMachine.FSM.ChangeState(PlayerStates.Default);
        if (playerIsInSafeArea)
        {
            lycanStateMachine.FSM.ChangeState(LycanStates.WaitingForRespawn);
        }
    }

    void Running_Update()
    {
        decoyManager.Active = !playerIsInSafeArea && !sunCrystalCircleMeter.IsLit;
    }

    void GameOver_Enter()
    {
        playerStateMachine.FSM.ChangeState(PlayerStates.Inactive);
        lycanStateMachine.FSM.ChangeState(LycanStates.Inactive);
    }

    public void OnPlayerEnterSafeArea()
    {
        playerIsInSafeArea = true;
        fog.Disable();
        lycanStateMachine.FSM.ChangeState(LycanStates.Inactive);
    }

    public void OnPlayerExitSafeArea()
    {
        playerIsInSafeArea = false;
        fog.Enable();
        lycanStateMachine.FSM.ChangeState(LycanStates.WaitingForRespawn);
    }
}
