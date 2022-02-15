using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using TMPro;
using Utilities;
using Cinemachine;


public class GameSpeechSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] ConversationTracker convoTracker;
    [SerializeField] List<SpeechBubble> speechBubblePool;
    [SerializeField] Canvas speechCanvas;

    [Header("Settings")]
    public bool AllowSkip = true;

    [Header("Variables")]
    [SerializeField] [ReadOnly] ConversationData currentConversation;
    [SerializeField] [ReadOnly] Player2D player;
    [SerializeField] [ReadOnly] bool inProgress = false;

    //[Header("Events")]
    public event System.Action OnSpeechStart;
    public event System.Action OnSpeechEnd;

    GameCharacter currentCharacter;

    static GameSpeechSystem s_gameSpeechSystem;

    void Awake()
    {
        if (s_gameSpeechSystem != null)
            Destroy(gameObject);

        s_gameSpeechSystem = this;
        Debug.Log(s_gameSpeechSystem);

        player = FindObjectOfType<Player2D>();
        if (player == null)
            Destroy(gameObject);
    }

    public static void RegisterCharacter(GameCharacter gameCharacter)
    {

        gameCharacter.RequestConversation += s_gameSpeechSystem.PrepareConversation;
    }

    public static void DeregisterCharacter(GameCharacter gameCharacter)
    {
        gameCharacter.RequestConversation -= s_gameSpeechSystem.PrepareConversation;
    }

    void OnDestroy()
    {
        if (s_gameSpeechSystem == this)
            s_gameSpeechSystem = null;
    }

    public IEnumerator PrepareConversation(GameCharacter gameCharacter)
    {
        currentCharacter = gameCharacter;

        if (inProgress)
            yield break;
        else
            inProgress = true;

        player.Lock();
        Vector2 newPlayerpos = convoTracker.SetConversation(gameCharacter.transform.position, player.transform.position);
        player.transform.position = newPlayerpos;

        yield return StartConversation();
    }


    //TODO
    public IEnumerator SetupPlayer()
    {
        yield return null;
    }


    public IEnumerator StartConversation()
    {

        yield return new WaitForSeconds(3);
        EndConversation();
    }

    public IEnumerator NextSentence()
    {
        SpeechBubble current;
        yield return null;

        EndConversation();
    }

    public void EndConversation()
    {
        inProgress = false;
    }
}
