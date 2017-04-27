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

    private StateMachine<PlayerStates> fsm;
    private StateMachine<MovementStates> movementStateMachine;
    private CharacterController characterController;

    void Awake()
    {
        movementStateMachine = GetComponent<MovementStateMachine>().StateMachine;
        fsm = StateMachine<PlayerStates>.Initialize(this, startingState);
        characterController = player.GetComponent<CharacterController>();
    }

    void Default_Enter()
    {
        movementStateMachine.ChangeState(MovementStates.Default);
        crosshair.enabled = true;
    }

    void Default_Update()
    {
        if (playerInput.shot)
        {
            crystalMeter.Activate();
        }
    }

    public StateMachine<PlayerStates> FSM
    {
        get { return fsm; }
    }
}
