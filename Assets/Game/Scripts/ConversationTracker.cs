using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class ConversationTracker : MonoBehaviour
{
    [Header("References")]
    public Transform LeftTarget;
    public Transform RightTarget;
    public CinemachineVirtualCamera ConversationCam;

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
            return LeftTarget.position;
        }
        else
        {
            mainSpeakerPos.x += m_speakerOffset;
            transform.position = mainSpeakerPos;
            return RightTarget.position;
        }
    }


}
