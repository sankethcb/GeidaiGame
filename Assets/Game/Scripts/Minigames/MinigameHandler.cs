using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public interface IMinigame
{
    void StartMinigame();

    void ExitMinigame();

    event System.Action OnGameStart;
    event System.Action OnGameComplete;
}



public class MinigameHandler : MonoBehaviour
{
    [Header("References")]

    [InterfaceReference(typeof(IMinigame))]
    [SerializeField] List<Object> MinigameList;

    [Header("Variables")]

    [ReadOnly]
    [InterfaceReference(typeof(IMinigame))]
    [SerializeField] Object currentMinigame;
    public IMinigame CurrentMinigame => currentMinigame as IMinigame;


    public event System.Action OnMinigameStart;
    public event System.Action OnMinigameComplete;


    public void StartMinigame(IMinigame minigame)
    {
        currentMinigame = minigame as Object;
        minigame.OnGameComplete += MinigameComplete;
        OnMinigameStart?.Invoke();
        minigame.StartMinigame();
    }




    public void MinigameComplete()
    {
        CurrentMinigame.OnGameComplete -= MinigameComplete;
        OnMinigameComplete?.Invoke();
    }
}
