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
    public TextReference Text;
}