using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

public class PlayerStateMachine : MonoBehaviour {
    
    public PlayerInput playerInput;
    public Animator weaponAnimator;
    public AnimationCurve dashingSpeedCurve;
    public GameObject player;
    public float dashingDuration;
    public float dashingMaxSpeed;
    public int dashCost;
    public Crosshair crosshair;
    public Collider playerCollider;
    public PlayerStates startingState = PlayerStates.Booting;
    public SunCrystalCircleMeter crystalMeter;

    private StateMachine<PlayerStates> fsm;
    private StateMachine<MovementStates> movementStateMachine;
    private float stateEnterTime;
    private Vector3 dashDirection;
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
        playerCollider.enabled = true;
    }

    void Default_Update()
    {
        if (playerInput.shot)
        {
            crystalMeter.Activate();
        }
    }

    private void UpdateSynergyInput()
    {
     
    }

    void Dashing_Update()
    {
        float t = (Time.time - stateEnterTime) / dashingDuration;
        float speedDelta = dashingSpeedCurve.Evaluate((Time.time - stateEnterTime) / dashingDuration);
        float currentSpeed = Mathf.Lerp(0, dashingMaxSpeed, speedDelta);

        Vector3 distanceToMove = dashDirection * currentSpeed * Time.deltaTime;
        Vector3 nextPosition = distanceToMove + player.transform.position;
        characterController.Move(distanceToMove);

        if (t >= 1)
        {
            fsm.ChangeState(PlayerStates.Default);
        }
    }

    public PlayerStates GetCurrentState()
    {
        return fsm.State;
    }
}
