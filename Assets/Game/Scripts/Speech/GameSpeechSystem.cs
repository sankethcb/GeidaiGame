using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using TMPro;
using Utilities;
using Cinemachine;
using UnityEngine.InputSystem;
using Core.Input;
public class GameSpeechSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] ConversationTracker convoTracker;
    [SerializeField] List<SpeechBubble> speechBubblePool;
    [SerializeField] Canvas speechCanvas;
    [SerializeField] InputEventSystem inputEventSystem;

    [Header("Settings")]
    public bool Timed = true;
    public bool AllowSkip = false;
    public float SpeechTimer = 5;
    public int MaxSpeechHistory = 5;
    public float SpeechMoveAmt = 0.5f;

    [Header("Variables")]
    [SerializeField][ReadOnly] ConversationData currentConversation;
    [SerializeField][ReadOnly] Player2D player;
    [SerializeField][ReadOnly] bool inProgress = false;

    //[Header("Events")]
    public event System.Action OnSpeechStart;
    public event System.Action OnSpeechEnd;

    GameCharacter m_currentCharacter;
    CinemachineBrain m_camBrain;
    SpeechBubble m_currentSpeech;

    Sequence m_speechMaster;
    Sequence m_speechTimer;
    Sequence m_speechMover;
    int m_speechCount = 0;
    public bool m_animPlaying = false;

    List<SpeechBubble> m_speechHistory = new List<SpeechBubble>();

    static GameSpeechSystem s_gameSpeechSystem;

    void Awake()
    {
        if (s_gameSpeechSystem != null)
            Destroy(gameObject);

        inputEventSystem.enabled = false;
        s_gameSpeechSystem = this;

        player = FindObjectOfType<Player2D>();
        if (player == null)
            Destroy(gameObject);
    }

    void Start()
    {
        m_camBrain = Camera.main.GetComponent<CinemachineBrain>();
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
        m_currentCharacter = gameCharacter;
        convoTracker.ConversationCam.gameObject.SetActive(true);

        if (inProgress)
            yield break;
        else
            inProgress = true;

        Vector3 newPlayerpos = convoTracker.SetConversation(gameCharacter.transform.position, gameCharacter.ConversationPosition);

        yield return player.MovePlayerTo(newPlayerpos);
        player.SetSpriteDirection(gameCharacter.ConversationPosition == ConversationTracker.ConversationPosition.PLAYER_LEFT);
        convoTracker.ConversationCam.Priority += 2;

        yield return null;
        while (m_camBrain.IsBlending)
            yield return null;

        yield return StartConversation();
    }


    public IEnumerator StartConversation()
    {
        currentConversation = m_currentCharacter.nextConversation;
        m_speechCount = -1;
        inputEventSystem.enabled = true;
        m_currentSpeech = null;
        //m_currentSpeech = speechBubblePool[0];
        NextSpeech();

        // if (Timed)
        //m_speechMaster = GetSpeechTimer();


        while (inProgress)
            yield return null;
    }

    public void GoToNextSpeech(InputAction.CallbackContext inputCallback) => GoToNextSpeech();

    public void GoToNextSpeech()
    {
        if (m_animPlaying)
            return;

        if (m_speechMaster != null && m_speechMaster.IsPlaying())
        {
            if (AllowSkip)
                m_speechMaster.Kill(true);
            else
                return;
        }

        NextSpeech();
    }


    Vector3 m_moveVector;
    public void NextSpeech()
    {

        player.Animator.SetBool("TALK", false);
        m_currentCharacter.Animator.SetBool("TALK", false);

        if (m_speechTimer != null)
            m_speechTimer?.Kill();

        m_speechCount++;
        if (m_speechCount == currentConversation.ConversationText.Count)
        {
            EndConversation().Start();
            return;
        }


        //Moving speech bubbles
        m_moveVector = Vector3.up * SpeechMoveAmt;
        if (m_currentSpeech != null)
        {
            speechBubblePool?.RemoveAt(0);
            m_speechHistory.Add(m_currentSpeech);
        }


        if (m_speechHistory.Count >= MaxSpeechHistory)
        {
            m_speechHistory[0].gameObject.SetActive(false);
            speechBubblePool.Add(m_speechHistory[0]);
            m_speechHistory.RemoveAt(0);
        }

        m_speechMover = DOTween.Sequence().Pause();
        foreach (SpeechBubble bubble in m_speechHistory)
        {
            m_speechMover.Join(bubble.transform.DOBlendableMoveBy(m_moveVector, 0.2f).SetEase(Ease.InOutExpo));
        }
        //


        if (currentConversation.ConversationText[m_speechCount].AnimationTrigger)
        {
            m_animPlaying = true;

            if (currentConversation.ConversationText[m_speechCount].PlayerSpeech)
            {
                player.Animator.SetTrigger(currentConversation.ConversationText[m_speechCount].AnimationParameter);
                player.Animator.TrackCurrentAnimation().OnComplete(() => m_animPlaying = false).Play();
            }
            else
            {
                m_currentCharacter.Animator.SetTrigger(currentConversation.ConversationText[m_speechCount].AnimationParameter);
                m_currentCharacter.Animator.TrackCurrentAnimation().OnComplete(() => m_animPlaying = false).Play();
            }

        }
        else
        {
            if (currentConversation.ConversationText[m_speechCount].PlayerSpeech)
            {
                player.Animator.SetBool("TALK", true);
            }
            else
            {
                m_currentCharacter.Animator.SetBool("TALK", true);
            }
        }

        if (!string.IsNullOrEmpty(currentConversation.ConversationText[m_speechCount].Text))
        {
            m_speechMover.Play().OnComplete(() =>
            {
                m_currentSpeech = speechBubblePool[0];
                ActivateSpeechBubble();
            });
        }


        m_speechMaster = DOTween.Sequence().Append(m_speechMover);
        if (Timed)
            m_speechMaster.Append(GetSpeechTimer());

    }

    Sequence GetSpeechTimer()
    {
        if (m_speechTimer != null)
            m_speechTimer.Kill();

        m_speechTimer = DOTween.Sequence().AppendInterval(SpeechTimer).AppendCallback(() => NextSpeech());
        return m_speechTimer;
    }

    public void ActivateSpeechBubble()
    {
        m_currentSpeech.TextObject.text = currentConversation.ConversationText[m_speechCount].Text;
        if ((convoTracker.Position == ConversationTracker.ConversationPosition.PLAYER_LEFT && currentConversation.ConversationText[m_speechCount].PlayerSpeech) ||
        (convoTracker.Position == ConversationTracker.ConversationPosition.PLAYER_RIGHT && !currentConversation.ConversationText[m_speechCount].PlayerSpeech))
        {
            m_currentSpeech.SetLeft();
        }
        else if ((convoTracker.Position == ConversationTracker.ConversationPosition.PLAYER_RIGHT && currentConversation.ConversationText[m_speechCount].PlayerSpeech) ||
        (convoTracker.Position == ConversationTracker.ConversationPosition.PLAYER_LEFT && !currentConversation.ConversationText[m_speechCount].PlayerSpeech))
        {
            m_currentSpeech.SetRight();
        }
        m_currentSpeech.transform.position = convoTracker.SpeechTarget.position;

        AudioHook.StopSFX(SFX.BUBBLE);
        AudioHook.PlaySFX(SFX.BUBBLE);

        if (!currentConversation.ConversationText[m_speechCount].PlayerSpeech)
            m_currentSpeech.Sprite.color = m_currentCharacter.CharacterColor;
        else
            m_currentSpeech.Sprite.color = Color.white;

        m_currentSpeech.gameObject.SetActive(true);

    }

    public IEnumerator EndConversation()
    {
        m_currentSpeech.gameObject.SetActive(false);
        foreach (SpeechBubble bubble in m_speechHistory)
        {
            if (!speechBubblePool.Contains(bubble))
            {
                speechBubblePool.Add(bubble);
            }
        }

        foreach (SpeechBubble bubble in speechBubblePool)
        {
            bubble.gameObject.SetActive(false);
        }


        m_speechHistory.Clear();
        m_animPlaying = false;
        inputEventSystem.enabled = false;
        convoTracker.ConversationCam.Priority -= 2;
        yield return null;
        while (m_camBrain.IsBlending)
            yield return null;

        convoTracker.ConversationCam.gameObject.SetActive(false);
        inProgress = false;
    }
}

public static class AnimatorExtensions
{
    public static Sequence TrackCurrentAnimation(this Animator animator, int layer = 0)
    {
        Sequence tracker = DOTween.Sequence().Pause();
        if (animator.IsInTransition(layer))
        {
            Debug.Log(animator.GetAnimatorTransitionInfo(layer).userNameHash);
            tracker.AppendInterval(animator.GetAnimatorTransitionInfo(layer).duration);

        }

        tracker.AppendInterval(animator.GetCurrentAnimatorStateInfo(layer).length);

        return tracker;
    }
}
