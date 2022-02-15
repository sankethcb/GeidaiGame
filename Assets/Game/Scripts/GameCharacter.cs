using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class GameCharacter : MonoBehaviour, IInteractable
{
    public ConversationData nextConversation;

    public bool LockPlayer => true;

    public event System.Func<GameCharacter, IEnumerator> RequestConversation;
    public event Action OnInteractionComplete;

    void Start()
    {
        GameSpeechSystem.RegisterCharacter(this);
    }

    void OnDestroy()
    {
        GameSpeechSystem.DeregisterCharacter(this);
    }

    public IEnumerator StartConversation()
    {
        yield return RequestConversation?.Invoke(this);

        Debug.Log("Interaction Complete");
        OnInteractionComplete?.Invoke();
    }

    public void DoInteraction()
    {
        StartConversation().Start();
    }

    public void CancelInteraction()
    {
        throw new NotImplementedException();
    }
}
