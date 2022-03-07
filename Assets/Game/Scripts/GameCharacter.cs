using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public interface IGamePlayable
{
    bool IsPlaying { get; }
    IEnumerator Play();
}



public class GameCharacter : MonoBehaviour, IInteractable, IGamePlayable
{

    public Animator Animator;
    public ConversationData nextConversation;
    public GameEventFlag InteractionEvent;
    public Color CharacterColor = Color.white;
    public ConversationTracker.ConversationPosition ConversationPosition;

    public bool LockPlayer => true;

    public bool IsPlaying => isInPlayerConversation;

    public Vector3 Position => transform.position;

    public event System.Func<GameCharacter, IEnumerator> RequestConversation;
    public event Action OnInteractionComplete;

    bool isInPlayerConversation = false;


    IGamePlayable currentPlayable;
    void Start()
    {
        GameSpeechSystem.RegisterCharacter(this);
        currentPlayable = this;
    }

    void OnDestroy()
    {
        GameSpeechSystem.DeregisterCharacter(this);
    }

    public void SetPlayable(GameObject Playable) => Playable.TryGetComponent<IGamePlayable>(out currentPlayable);

    public IEnumerator Play()
    {
        isInPlayerConversation = true;
        yield return RequestConversation?.Invoke(this);
        isInPlayerConversation = false;
        InteractionEvent?.Raise();
    }

    public IEnumerator DoInteraction()
    {
        yield return currentPlayable.Play().Start();
        InteractionComplete();
    }

    void InteractionComplete()
    {
        Debug.Log("Interaction complete");
        OnInteractionComplete?.Invoke();
    }

    public void CancelInteraction()
    {
        throw new NotImplementedException();
    }

    public void SetTrigger(string trigger) => Animator.SetTrigger(trigger);

    public void SetConversation(ConversationData convo) => nextConversation = convo;

}
