using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

[CreateAssetMenu(fileName = "SpeechData", menuName = "Geidai/Speech Data", order = 2)]
public class SpeechData : ScriptableObject
{
    public bool PlayerSpeech = false;
    public TextReference Text;
}
