using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

public class GameStateMachine : MonoBehaviour {

    public DecoyManager decoyManager;
    public LycanStateMachine lycanStateMachine;
    public PlayerStateMachine playerStateMachine;
    public GameObject lycan;
    public AnimationCurve cameraRotationAnimationCurve;
    public float timeToRotateToLycan = 0.5f;

    private GameObject player;
    private SunCrystalCircleMeter sunCrystalCircleMeter;
    private Fog fog;
    private Camera playerCamera;
    private bool playerIsInSafeArea;
    private Quaternion initialRotation;
    private Quaternion destinationRotation;
    private float timeWhenGameOverSequenceStarted;

    private StateMachine<GameStates> fsm;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        fog = player.GetComponentInChildren<Fog>();
        playerCamera = player.GetComponentInChildren<Camera>();
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

    void GameOverSequence_Enter()
    {
        decoyManager.Active = false;
        playerStateMachine.FSM.ChangeState(PlayerStates.Inactive);
        initialRotation = playerCamera.transform.rotation;
        destinationRotation = Quaternion.LookRotation(lycan.transform.position - playerCamera.transform.position);
        timeWhenGameOverSequenceStarted = Time.time;
    }

    void GameOverSequence_FixedUpdate()
    {
        float time = (Time.time - timeWhenGameOverSequenceStarted) / timeToRotateToLycan;
        Quaternion newRotation = Quaternion.Slerp(initialRotation, destinationRotation, cameraRotationAnimationCurve.Evaluate(time));
        playerCamera.transform.rotation = newRotation;
    }

    void GameOverScreen_Enter()
    {
        //lycanStateMachine.FSM.ChangeState(LycanStates.Inactive);
    }

    void OnPlayerEnterSafeArea()
    {
        playerIsInSafeArea = true;
        fog.Disable();
        lycanStateMachine.FSM.ChangeState(LycanStates.Inactive);
    }

    void OnPlayerExitSafeArea()
    {
        playerIsInSafeArea = false;
        fog.Enable();
        lycanStateMachine.FSM.ChangeState(LycanStates.WaitingForRespawn);
    }

    void OnGameOverSequenceStarted()
    {
        fsm.ChangeState(GameStates.GameOverSequence);
    }

    void OnGameOverSequenceEnded()
    {
        fsm.ChangeState(GameStates.GameOverScreen);
    }
}
