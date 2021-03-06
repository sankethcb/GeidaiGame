using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

[CreateAssetMenu(fileName = "ConversationData", menuName = "Geidai/Conversation Data", order = 1)]
public class ConversationData : ScriptableObject
{
    public List<Speech> ConversationText;
}


[System.Serializable]
public class Speech
{
    public bool PlayerSpeech = false;

    [SerializeField, TextArea(3, 20)]
    public string Text = "";

    public bool AnimationTrigger = false;
    public string AnimationParameter;
}