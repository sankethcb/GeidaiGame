using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameText
{
    [CreateAssetMenu(fileName = "GameTextData", menuName = "CKTC/Game Text Data", order = 1)]
    public class GameTextData : ScriptableObject
    {
        public string TextOwner;
        public Sprite Portrait;
        public GameTextData NextGameText;
        public List<Utilities.TextReference> TextEntries;

        public event System.Action OnGameTextStarted;
        public event System.Action OnGameTextPlayed;

        public void AddToGameTextPlayerQue()
        {
            GameTextPlayer.QueGameText_Global(this);
        }


        public void Started() => OnGameTextStarted?.Invoke();
        public void Played() => OnGameTextPlayed?.Invoke();

    }
}