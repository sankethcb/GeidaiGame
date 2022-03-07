using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.InputSystem;

public interface ISubMenu
{
    bool PauseGame { get; }
    void OpenMenu(InputAction.CallbackContext inputCallback) => OpenMenu();

    void OpenMenu();

    void CloseMenu(InputAction.CallbackContext inputCallback) => CloseMenu();
    void CloseMenu();
}


public class BookMenu : MonoBehaviour
{
    [Header("References")]
    public Button BookButton;
    public Canvas BookCanvas;
    public Image Overlay;
    public GameObject MenuButtons;
    public GameObject FrontPage;
    public BookDataHandler BookDataHandler;
    public PauseHandler PauseHandler;
    public BookPage LeftPage;
    public BookPage RightPage;
    public GameObject Book;

    public bool open = false;
    public int currentPageSet = 0;
    bool remainder = false;
    Tween fader;

    public void OpenBook(InputAction.CallbackContext inputCallback) => OpenBook();

    public void CloseBook(InputAction.CallbackContext inputCallback) => CloseBook();

    public void ToggleBook(InputAction.CallbackContext inputCallback) => ToggleBook();

    void Start()
    {
        if (BookDataHandler.PageData.Count % 2 != 0)
        {
            remainder = true;
        }
    }
    public void OpenBook()
    {
        SetBookState(true);
    }

    public void CloseBook()
    {
        SetBookState(false);
    }

    public void ToggleBook()
    {
        SetBookState(!open);
    }

    void SetBookState(bool state)
    {
        Debug.Log("BookState");

        open = state;
        PauseHandler.TogglePauseState();
        BookButton.gameObject.SetActive(!open);
        BookCanvas.enabled = open;

        if (fader?.IsPlaying() != null)
            fader.Kill();

        if (open)
            fader = Overlay.DOFade(0.6f, 0.2f).SetUpdate(true);
        else
        {
            fader = Overlay.DOFade(0.0f, 0.2f).SetUpdate(true);
            currentPageSet = 0;
            ToggleBookStart(true);
        }

    }

    public void NextPage()
    {
        currentPageSet += 2;

        if (currentPageSet == 2)
            ToggleBookStart(false);

        if (currentPageSet > BookDataHandler.PageData.Count)
        {
            if (remainder)
            {
                RightPage.gameObject.SetActive(false);
                LeftPage.LoadPageData(BookDataHandler.PageData[currentPageSet - 2]);

            }
            else
            {
                currentPageSet = 0;
                ToggleBookStart(true);
            }
            return;
        }

        LoadPageData();
    }

    public void PrevPage()
    {
        currentPageSet -= 2;

        if (currentPageSet == 0)
        {
            ToggleBookStart(true);
            return;
        }
        else if (!RightPage.gameObject.activeSelf)
            RightPage.gameObject.SetActive(true);

        LoadPageData();
    }

    void ToggleBookStart(bool state)
    {

        MenuButtons.SetActive(state);
        FrontPage.SetActive(state);
        LeftPage.gameObject.SetActive(!state);
        RightPage.gameObject.SetActive(!state);
    }

    void LoadPageData()
    {
        Debug.Log(currentPageSet);
        RightPage.LoadPageData(BookDataHandler.PageData[currentPageSet - 1]);
        LeftPage.LoadPageData(BookDataHandler.PageData[currentPageSet - 2]);
    }
}
