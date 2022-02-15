using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;
using Core.Input;
using Core.FSM;

public class Player2D : MonoBehaviour
{
    [Header("References")]
    [SerializeField] StateMachineHandler stateMachineHandler;
    [SerializeField] Movement2D movement2D;
    [SerializeField] Interaction2D interaction2D;


    [Header("Variables")]
    [SerializeField] [ReadOnly] StateMachine<PlayerStates> stateMachine;


    Transition idleToMoving;
    Transition movingToIdle;
    Transition toLocked;
    Transition lockedToIdle;


    public StateMachine<PlayerStates> StateMachine => stateMachine;

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
        onEnter: () => { StopPlayer(); });


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
            onPhysicsUpdate: movement2D.Move
        );

        stateMachine.Enter(PlayerStates.IDLE);
    }

    public virtual void MovementInput(InputAction.CallbackContext inputCallback)
    {
        MovementInput(inputCallback.ReadValue<Vector2>());
    }

    public virtual void MovementInput(Vector2 direction)
    {
        if (stateMachine.activeState == stateMachine.GetState(PlayerStates.LOCKED))
            return;

        movement2D.SetMovementTarget(direction);

        if (movement2D.Direction.x != 0 || movement2D.Direction.y != 0) idleToMoving.Trigger?.Invoke(true);
    }

    public virtual void StopPlayer(InputAction.CallbackContext inputCallback)
    {
        StopPlayer();
    }

    public virtual void StopPlayer()
    {
        movement2D.Stop();

        movingToIdle.Trigger?.Invoke(true);
    }

    public void Lock() => toLocked.Trigger?.Invoke(true);
    public void Unlock() => lockedToIdle.Trigger?.Invoke(true);


    public void AttemptInteraction(InputAction.CallbackContext inputCallback) => AttemptInteraction();
    public void AttemptInteraction()
    {
        if (interaction2D.Interactable == null)
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
        if(stateMachine.activeState == stateMachine.GetState(PlayerStates.LOCKED))
            Unlock();
    }

}
