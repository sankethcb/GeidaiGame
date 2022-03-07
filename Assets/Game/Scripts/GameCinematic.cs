using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;
public class GameCinematic : MonoBehaviour, IGamePlayable
{
    public bool LockPlayer = true;
    public bool IsPlaying => (TimelineController.state == PlayState.Playing);

    public event System.Action<GameCinematic> QueueCinematic;

    public PlayableDirector TimelineController;

    public UnityEvent OnCinematicComplete;
    void Start()
    {
        GameController.RegisterCinematic(this);
    }

    void OnDestroy()
    {
        GameController.DeregisterCinematic(this);
    }

    public void PlayCinematic()
    {
        QueueCinematic?.Invoke(this);
    }

    public IEnumerator Play()
    {
        TimelineController.Play();

        while (TimelineController.state == PlayState.Playing)
            yield return null;

        OnCinematicComplete?.Invoke();
        TimelineController.gameObject.SetActive(false);
    }
}
