using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.InputSystem;

public class WritingMinigame : MonoBehaviour, IMinigame, ISubMenu, IGamePlayable
{

    [SerializeField] List<GestureRecognizer> gestureRecognizers;
    [SerializeField] Canvas gestureCanvas;
    [SerializeField] Canvas copyCanvas;
    [SerializeField] Camera gestureCamera;
    [SerializeField] GameObject hintObject;
    [SerializeField] TMPro.TextMeshProUGUI hintText;
    [SerializeField] List<string> correctAnswer;

    public GameEventFlag MinigameFlag;
    public GameObject Book;


    public bool PauseGame => throw new NotImplementedException();

    public bool IsPlaying => gestureCanvas.gameObject.activeSelf;

    public event Action OnGameStart;
    public event Action OnGameComplete;

    public int letterIndex = 0;


    void Start()
    {


    }
    public void StartMinigame()
    {
        Book.SetActive(false);
        letterIndex = 0;
        hintText.text = "We need to write down the gestures in the right order!";

        foreach (GestureRecognizer gR in gestureRecognizers)
            gR.OnRecognize += ProcessAnswer;

        gestureCanvas.gameObject.SetActive(true);

        OnGameStart?.Invoke();

    }

    public void ExitMinigame()
    {
        Book.SetActive(true);
        foreach (GestureRecognizer gR in gestureRecognizers)
            gR.OnRecognize -= ProcessAnswer;


        gestureCanvas.gameObject.SetActive(false);

        OnGameComplete?.Invoke();
    }

    Sequence textTimer;
    public bool ProcessAnswer(GestureRecognizer gestureRecog, string id, float value)
    {

        if (correctAnswer[gestureRecognizers.IndexOf(gestureRecog)] == id && value > 0.75f)
        {
            UpdateText("Great!");

            letterIndex++;
            gestureRecog.enabled = false;

            if (letterIndex == correctAnswer.Count)
            {
                UpdateText("I think that should all be correct!");

                gestureCamera.gameObject.SetActive(true);
                gestureCamera.transform.position = Camera.main.transform.position;
                gestureCanvas.worldCamera = gestureCamera;
                copyCanvas.enabled = true;

                MinigameFlag.Raise();
                DOTween.Sequence().AppendInterval(3).OnComplete(() => CloseMenu());
            }

            return true;
        }
        else if (correctAnswer[gestureRecognizers.IndexOf(gestureRecog)] != id && value > 0.75f)
        {
            UpdateText("I don't think that gesture goes there!");
        }
        else
        {
            UpdateText("Hmm that doesn't look right...!");
        }

        return false;
    }

    void UpdateText(string text)
    {
        if (textTimer!= null && textTimer.IsPlaying())
        {
            textTimer.Kill();
            textTimer = DOTween.Sequence().Append(hintText.DOFade(0, 0.5f).OnComplete(() => hintText.text = text)).Pause();
        }
        else
        {
            textTimer = DOTween.Sequence().Append(hintText.DOFade(0, 0f)).Pause();
            hintText.text = text;
        }

        textTimer
        .Append(hintText.DOFade(1, 0.5f))
        .AppendInterval(3)
        .Append(hintText.DOFade(0, 0.5f))
        .Play();
    }

    public void OpenMenu()
    {
        StartMinigame();
    }
    public void OpenMenu(InputAction.CallbackContext inputCallback) => OpenMenu();
    public void CloseMenu(InputAction.CallbackContext inputCallback) => CloseMenu();

    public void CloseMenu()
    {
        gestureCamera.gameObject.SetActive(false);
        gestureCanvas.worldCamera = Camera.main;
        copyCanvas.enabled = false;
        ExitMinigame();
    }


    public IEnumerator Play()
    {
        OpenMenu();
        while (IsPlaying)
            yield return null;
    }
}
