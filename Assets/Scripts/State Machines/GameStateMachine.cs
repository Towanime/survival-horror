using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

public class GameStateMachine : MonoBehaviour {

    public PlayerInput playerInput;
    public DecoyManager decoyManager;
    public LycanStateMachine lycanStateMachine;
    public PlayerStateMachine playerStateMachine;
    public GameObject focusPoint;
    public AnimationCurve cameraRotationAnimationCurve;
    public Camera uiCamera;
    public float timeToRotateToLycan = 0.5f;
    public GameObject gameOverScreenCanvas;
    public CanvasGroup foregroundCanvasGroup;
    public float timeBeforeShowingGameOverScreen = 0.05f;
    public float timeBeforeFadeInGameOverScreen = 0.5f;
    public float timeToFadeInGameOverScreen = 0.5f;
    public Canvas uiCanvas;
    public Diary diary;
    public bool lotusEnabled;

    private GameObject player;
    private SunCrystalCircleMeter sunCrystalCircleMeter;
    private Fog fog;
    private Camera playerCamera;
    private bool playerIsInSafeArea;
    private Quaternion initialRotation;
    private Quaternion destinationRotation;
    private float timeStateChanged;
    private bool initialized;
    private bool playingGameOverSfx;
    private StateMachine<GameStates> fsm;

    void Start()
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
        if (!lotusEnabled)
        {
            playerStateMachine.DisableLotus();
        }
    }

    void Running_Enter()
    {
        uiCanvas.enabled = true;
        playingGameOverSfx = false;
        SoundManager.Instance.FadeOut(SoundId.GAME_OVER, true);
        playerCamera.gameObject.SetActive(true);
        uiCamera.gameObject.SetActive(false);
        gameOverScreenCanvas.SetActive(false);
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

    void ReadingBook_Enter()
    {
        playerStateMachine.FSM.ChangeState(PlayerStates.Inactive);
        uiCanvas.enabled = false;
    }

    void ReadingBook_Update()
    {
        if (playerInput.action)
        {
            StartCoroutine(diary.Close());
        }
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
        yield return new WaitForSeconds(timeBeforeShowingGameOverScreen);
        gameOverScreenCanvas.SetActive(true);
        foregroundCanvasGroup.alpha = 1;
        playerCamera.transform.rotation = GetDestinationRotation();
        playerCamera.gameObject.SetActive(false);
        uiCamera.gameObject.SetActive(true);
        yield return new WaitForSeconds(timeBeforeFadeInGameOverScreen);
        timeStateChanged = Time.time;
    }

    void GameOverScreen_Update()
    {
        float alpha = 1 - Mathf.Min(1, (Time.time - timeStateChanged) / timeToFadeInGameOverScreen);
        foregroundCanvasGroup.alpha = alpha;
        if (alpha <= 0)
        {
            if (!playingGameOverSfx)
            {
                SoundManager.Instance.FadeIn(SoundId.GAME_OVER);
                playingGameOverSfx = true;
            }
            if (playerInput.action)
            {
                fsm.ChangeState(GameStates.Running);
            }
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

    void OnBookOpened()
    {
        fsm.ChangeState(GameStates.ReadingBook);
    }

    void OnBookClosed()
    {
        fsm.ChangeState(GameStates.Running);
    }

    void OnLotusPickup()
    {
        playerStateMachine.EnableLotus();
    }
}
