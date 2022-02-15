using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class ConversationTracker : MonoBehaviour
{
    public enum ConversationPosition
    {
        PLAYER_LEFT,
        PLAYER_RIGHT
    }


    [Header("References")]
    public Transform LeftTarget;
    public Transform RightTarget;
    public Transform SpeechTarget;
    public CinemachineVirtualCamera ConversationCam;
    public ConversationPosition Position;
    float m_speakerOffset;
    void Awake()
    {
        m_speakerOffset = RightTarget.position.x - transform.position.x;
    }


    public Vector2 SetConversation(Vector2 mainSpeakerPos, Vector2 subSpeakerPos)
    {
        if (mainSpeakerPos.x > subSpeakerPos.x)
        {
            mainSpeakerPos.x -= m_speakerOffset;
            transform.position = mainSpeakerPos;
            Position = ConversationPosition.PLAYER_LEFT;
            return LeftTarget.position;
        }
        else
        {
            mainSpeakerPos.x += m_speakerOffset;
            transform.position = mainSpeakerPos;
            Position = ConversationPosition.PLAYER_RIGHT;
            return RightTarget.position;
        }
    }


}
