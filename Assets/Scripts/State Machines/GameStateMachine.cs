using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

public class GameStateMachine : MonoBehaviour {

    public DecoyManager decoyManager;
    public LycanStateMachine lycanStateMachine;
    public PlayerStateMachine playerStateMachine;
    public GameObject focusPoint;
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
    private bool initialized;
    private StateMachine<GameStates> fsm;

    void Awake()
    {
        if (!initialized) Init();
    }

    void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        fog = player.GetComponentInChildren<Fog>();
        playerCamera = player.GetComponentInChildren<Camera>();
        sunCrystalCircleMeter = player.GetComponentInChildren<SunCrystalCircleMeter>();
        fsm = StateMachine<GameStates>.Initialize(this, GameStates.Running);
        initialized = true;
    }

    void Running_Enter()
    {
        playerStateMachine.FSM.ChangeState(PlayerStates.Default);
        if (playerIsInSafeArea)
        {
            lycanStateMachine.FSM.ChangeState(LycanStates.Inactive);
        } else
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
        timeWhenGameOverSequenceStarted = Time.time;
    }

    void GameOverSequence_FixedUpdate()
    {
        destinationRotation = Quaternion.LookRotation(focusPoint.transform.position - playerCamera.transform.position);
        float time = (Time.time - timeWhenGameOverSequenceStarted) / timeToRotateToLycan;
        Quaternion newRotation = Quaternion.Slerp(initialRotation, destinationRotation, cameraRotationAnimationCurve.Evaluate(time));
        playerCamera.transform.rotation = newRotation;
    }

    void GameOverScreen_Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            fsm.ChangeState(GameStates.Running);
        }
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
