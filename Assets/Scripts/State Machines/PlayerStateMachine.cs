using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

public class PlayerStateMachine : MonoBehaviour {
    
    public PlayerInput playerInput;
    public GameObject player;
    public Crosshair crosshair;
    public PlayerStates startingState = PlayerStates.Default;
    public SunCrystalCircleMeter crystalMeter;
    public ActionInView actionActivator;
    public Animator armAnimator;

    private StateMachine<PlayerStates> fsm;
    private StateMachine<MovementStates> movementStateMachine;
    private CharacterController characterController;
    private bool lotusEnabled = true;

    private bool initialized;

    void Awake()
    {
        if (!initialized) Init();
    }

    void Init()
    {
        movementStateMachine = GetComponent<MovementStateMachine>().StateMachine;
        fsm = StateMachine<PlayerStates>.Initialize(this, startingState);
        characterController = player.GetComponent<CharacterController>();
        initialized = true;
    }

    void Inactive_Enter()
    {
        movementStateMachine.ChangeState(MovementStates.Disabled);
        crosshair.enabled = false;
    }

    void Default_Enter()
    {
        movementStateMachine.ChangeState(MovementStates.Default);
        crosshair.enabled = true;
    }

    void Default_Update()
    {
        if (playerInput.action)
        {
            actionActivator.Activate();
        }
        if (playerInput.crystal && lotusEnabled)
        {
            crystalMeter.Activate();
        }
    }

    public void EnableLotus()
    {
        if (!lotusEnabled)
        {
            armAnimator.gameObject.SetActive(true);
            armAnimator.SetTrigger("lotusActive");
            lotusEnabled = true;
        }
    }

    public void DisableLotus()
    {
        armAnimator.gameObject.SetActive(false);
        lotusEnabled = false;
    }

    public StateMachine<PlayerStates> FSM
    {
        get {
            if (!initialized)
            {
                Init();
            }
            return fsm; }
    }
}
