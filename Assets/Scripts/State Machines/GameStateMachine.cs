﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

public class GameStateMachine : MonoBehaviour {

    public DecoyManager decoyManager;
    public LycanStateMachine lycanStateMachine;
    public PlayerStateMachine playerStateMachine;
    public GameObject focusPoint;
    public AnimationCurve cameraRotationAnimationCurve;
    public Camera uiCamera;
    public float timeToRotateToLycan = 0.5f;
    public CanvasGroup foregroundCanvasGroup;
    private float timeBeforeFadeInGameOverScreen = 0.5f;
    private float timeToFadeInGameOverScreen = 0.5f;

    private GameObject player;
    private SunCrystalCircleMeter sunCrystalCircleMeter;
    private Fog fog;
    private Camera playerCamera;
    private bool playerIsInSafeArea;
    private Quaternion initialRotation;
    private Quaternion destinationRotation;
    private float timeStateChanged;
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
        playerCamera.gameObject.SetActive(true);
        uiCamera.enabled = false;
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
        timeStateChanged = Time.time;
    }

    void GameOverSequence_FixedUpdate()
    {
        float time = (Time.time - timeStateChanged) / timeToRotateToLycan;
        Quaternion newRotation = Quaternion.Slerp(initialRotation, GetDestinationRotation(), cameraRotationAnimationCurve.Evaluate(time));
        playerCamera.transform.rotation = newRotation;
    }

    IEnumerator GameOverScreen_Enter()
    {
        foregroundCanvasGroup.alpha = 1;
        playerCamera.transform.rotation = GetDestinationRotation();
        playerCamera.gameObject.SetActive(false);
        uiCamera.enabled = true;
        yield return new WaitForSeconds(timeBeforeFadeInGameOverScreen);
        timeStateChanged = Time.time;
    }

    void GameOverScreen_Update()
    {
        float alpha = Mathf.Min(1, (Time.time - timeStateChanged) / timeToFadeInGameOverScreen);
        foregroundCanvasGroup.alpha = 1 - alpha;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            fsm.ChangeState(GameStates.Running);
        }
    }

    private Quaternion GetDestinationRotation()
    {
        return Quaternion.LookRotation(focusPoint.transform.position - playerCamera.transform.position);
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
