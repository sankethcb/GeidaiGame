using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct FLAGS
{

}

public enum QUESTLIST
{
    DEFAULT
}

public class GameController : MonoBehaviour
{
    [Header("References")]
    public QuestTracker QuestTracker;
    public Player2D Player;
    public MinigameHandler MinigameHandler;

    public GameCharacter Octopus;
    public GameCharacter Elephant;

    [Header("Variables")]

    [Header("Flags")]
    [SerializeField] FLAGS flags;

    public static FLAGS FLAGS;

    static GameController s_gameController;

    void Awake()
    {
        FLAGS = flags;

        if (s_gameController != null)
            Destroy(gameObject);

        s_gameController = this;
    }

    void OnDestroy()
    {
        if (s_gameController == this)
            s_gameController = null;
    }

    public static void RegisterCinematic(GameCinematic cinematic)
    {
        cinematic.QueueCinematic += s_gameController.QueueCinematic;
    }

    public static void DeregisterCinematic(GameCinematic cinematic)
    {
        cinematic.QueueCinematic -= s_gameController.QueueCinematic;
    }

    public void QueueCinematic(GameCinematic cinematic) => StartCoroutine(TrackCinematic(cinematic));

    public IEnumerator TrackCinematic(GameCinematic cinematic)
    {
        Debug.Log("Cinematic tracking");

        if (cinematic.LockPlayer)
            Player.Lock();

        yield return cinematic.Play();

        if (cinematic.LockPlayer)
            Player.Unlock();
    }

}



