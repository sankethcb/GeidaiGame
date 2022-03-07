using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;
using Core.Input;
using Core.FSM;
using DG.Tweening;

public class Player2D : MonoBehaviour
{
    [Header("References")]
    [SerializeField] StateMachineHandler stateMachineHandler;
    public Movement2D movement2D;
    [SerializeField] Interaction2D interaction2D;
    [SerializeField] Animator2D animator2D;
    [SerializeField] Cinemachine.CinemachineVirtualCamera playerCamera;

    [Header("Variables")]
    [SerializeField][ReadOnly] StateMachine<PlayerStates> stateMachine;

    protected static readonly int MOVEMENT_HASH = Animator.StringToHash("MOVEMENT");
    protected static readonly int TALK_HASH = Animator.StringToHash("TALK");
    protected static readonly int FLIP_HASH = Animator.StringToHash("FLIP");


    Transition idleToMoving;
    Transition movingToIdle;
    Transition toLocked;
    Transition lockedToIdle;


    public StateMachine<PlayerStates> StateMachine => stateMachine;
    public Cinemachine.CinemachineVirtualCamera PlayerCamera => playerCamera;
    public Animator Animator => animator2D.Animator;

    public enum PlayerStates
    {
        IDLE,
        MOVING,
        LOCKED
    }

    void Awake()
    {
        stateMachine = stateMachineHandler.Initialize<PlayerStates>();
        InitalizeStateMachine();
    }

    void InitalizeStateMachine()
    {
        stateMachine.GetState(PlayerStates.LOCKED).SetActions(
        onEnter: () =>
        {
            StopPlayer();
            animator2D.SetInt("MOVEMENT", 0);
            interaction2D.enabled = false;
        },
        onExit: () =>
        {
            interaction2D.enabled = true;
        }
        );


        stateMachine.GetState(PlayerStates.MOVING).SetActions(
        onEnter: () =>
        {

            if (movement2D.Direction.x != 0 || movement2D.Direction.y != 0)
            {
                if (movement2D.Direction.x > 0)
                    animator2D.SetBool("FLIP", true);
                else if (movement2D.Direction.x < 0)
                    animator2D.SetBool("FLIP", false);
            }
            animator2D.SetInt("MOVEMENT", 1);
        });
        stateMachine.GetState(PlayerStates.IDLE).SetActions(
        onEnter: () =>
        {
            animator2D.SetInt("MOVEMENT", 0);
        });



        idleToMoving = stateMachine.AddTransition(
            from: PlayerStates.IDLE,
            to: PlayerStates.MOVING);

        movingToIdle = stateMachine.AddTransition(
            from: PlayerStates.MOVING,
            to: PlayerStates.IDLE);

        lockedToIdle = stateMachine.AddTransition(
            from: PlayerStates.LOCKED,
            to: PlayerStates.IDLE);

        toLocked = stateMachine.AddGlobalTransition(new Transition(stateMachine.GetState(PlayerStates.LOCKED)));

        stateMachine.GetState(PlayerStates.MOVING).SetActions(
            onPhysicsUpdate: movement2D.Move,
            onEnter: () =>
        {
            animator2D.SetInt("MOVEMENT", 1);
        });


        stateMachine.Enter(PlayerStates.IDLE);
    }

    public virtual void MovementInput(InputAction.CallbackContext inputCallback)
    {
        MovePlayer(inputCallback.ReadValue<Vector2>());
    }

    public virtual void MovePlayer(Vector2 direction)
    {
        if (stateMachine.activeState == stateMachine.GetState(PlayerStates.LOCKED))
            return;

        movement2D.SetMovementTarget(direction);

        if (movement2D.Direction.x != 0 || movement2D.Direction.y != 0)
        {
            if (movement2D.Direction.x > 0)
                animator2D.SetBool("FLIP", true);
            else if (movement2D.Direction.x < 0)
                animator2D.SetBool("FLIP", false);
        }

        if (movement2D.Direction.x != 0 || movement2D.Direction.y != 0)
        {
            if (stateMachine.activeState == stateMachine.GetState(PlayerStates.IDLE))
            {
                AudioHook.PlaySFX(SFX.WALKING);
                idleToMoving.Trigger?.Invoke(true);
            }
        }
    }

    public IEnumerator MovePlayerTo(Vector3 position)
    {
        Debug.Log("Move Player started");

        AudioHook.PlaySFX(SFX.WALKING);
        while ((transform.position - position).sqrMagnitude > 0.1f)
        {
            movement2D.SetMovementTarget(position - transform.position);
            animator2D.SetInt("MOVEMENT", 1);

            if (movement2D.Direction.x != 0 || movement2D.Direction.y != 0)
            {
                SetSpriteDirection(movement2D.Direction.x > 0);
            }

            movement2D.Move();
            yield return null;
        }
        StopPlayer();
        animator2D.SetInt("MOVEMENT", 0);
        transform.position = position;
    }

    public void SetSpriteDirection(bool flip) => animator2D.SetBool("FLIP", flip);

    public virtual void StopPlayer(InputAction.CallbackContext inputCallback)
    {
        StopPlayer();
    }

    public virtual void StopPlayer()
    {
        AudioHook.StopSFX(SFX.WALKING);
        movement2D.Stop();

        movingToIdle.Trigger?.Invoke(true);
    }

    public void Lock() => toLocked.Trigger?.Invoke(true);
    public void Unlock() => lockedToIdle.Trigger?.Invoke(true);


    public void AttemptInteraction(InputAction.CallbackContext inputCallback) => AttemptInteraction();
    public void AttemptInteraction()
    {
        if (interaction2D.Interactable == null || interaction2D.Interacting)
            return;
        interaction2D.OnInteractStart += InteractionBehaviour;
        interaction2D.OnInteractComplete += PostInteraction;
        interaction2D.InteractWith();
    }

    void InteractionBehaviour(bool lockPlayer)
    {
        interaction2D.OnInteractStart -= InteractionBehaviour;
        if (lockPlayer)
        {
            Lock();
        }
    }

    void PostInteraction()
    {
        interaction2D.OnInteractComplete -= PostInteraction;
        if (stateMachine.activeState == stateMachine.GetState(PlayerStates.LOCKED))
            Unlock();
    }

}
