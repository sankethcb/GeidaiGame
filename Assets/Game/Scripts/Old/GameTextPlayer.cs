using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Utilities;
using DG.Tweening;

namespace GameText
{
    public class GameTextPlayer : MonoBehaviour
    {
        [Header("References")]
        public Canvas GameTextPlayerCanvas;
        public RectTransform GameTextPlayerPanel;
        public TextMeshProUGUI GameText;
        public TextMeshProUGUI GameTextOwner;
        public UnityEngine.UI.Image Portrait;
        public UnityEngine.UI.Button ExitButton;
        public UnityEngine.UI.Button SkipButton;
        public AudioSource AudioSource;

        [Header("Settings")]
        [Range(0, 1)] public float CharacterPrintTime = 0.1f;
        public bool AllowFastForward = true;

        [Header("Events")]
        public UnityEvent OnPlayerStart;
        public UnityEvent OnNextGameText;
        public UnityEvent OnNextTextEntry;
        public UnityEvent OnPlayerEnd;

        public static UnityEvent OnNewGameText_Global => s_gameTextPlayer?.OnPlayerStart;
        public static UnityEvent OnNextTextEntry_Global => s_gameTextPlayer?.OnNextTextEntry;
        public static UnityEvent OnNextGameText_Global => s_gameTextPlayer?.OnNextGameText;
        public static UnityEvent OnPlayerEnd_Global => s_gameTextPlayer?.OnPlayerEnd;

        [Header("Variables")]
        [SerializeField] [ReadOnly] GameTextData currentGameText;
        [SerializeField] [ReadOnly] GameTextData quedGameText;

        static GameTextPlayer s_gameTextPlayer;

        Sequence m_animatorSequence;
        Sequence m_textSequence;
        Tween m_playerAnimator;

        int m_currentEntry;
        int m_characterCount;

        void Awake()
        {
            if (s_gameTextPlayer != null)
                Destroy(this);

            s_gameTextPlayer = this;
            GameTextPlayerCanvas.enabled = false;
        }

        void OnDestroy()
        {
            if (s_gameTextPlayer == this)
                s_gameTextPlayer = null;
        }

        public void PlayerEntry()
        {
            if (m_playerAnimator != null)
                m_playerAnimator.Kill(true);

            GameTextPlayerCanvas.enabled = true;
            GameText.text = "";


            if (GameTextPlayerPanel.anchoredPosition.y != -500)
                GameTextPlayerPanel.anchoredPosition = Vector2.up * -500;

            m_playerAnimator = GameTextPlayerPanel.DOAnchorPosY(0, 0.5f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                OnPlayerStart?.Invoke();
                NextTextEntry();
                if (AllowFastForward) SkipButton.gameObject.SetActive(true);
                m_playerAnimator = null;
            });

        }

        public void PlayerExit(UnityEngine.InputSystem.InputAction.CallbackContext inputCallback) => PlayerExit();

        public void PlayerExit()
        {
            ExitButton.gameObject.SetActive(false);

            if (m_playerAnimator != null)
                m_playerAnimator.Kill(true);

            OnPlayerEnd?.Invoke();
            m_playerAnimator = GameTextPlayerPanel.DOAnchorPosY(-500, 0.5f)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                GameTextPlayerCanvas.enabled = false;
                m_playerAnimator = null;
            });
        }

        public static bool QueGameText_Global(GameTextData gameText, bool playAutomatically = true)
        {
            return s_gameTextPlayer.QueGameText(gameText, playAutomatically);
        }
        public bool QueGameText(GameTextData gameText, bool playAutomatically = true)
        {
            if (quedGameText != null)
                return false;

            quedGameText = gameText;

            if (currentGameText == null && playAutomatically)
            {
                NextGameText();
            }

            return true;
        }

        public bool NextGameText()
        {
            currentGameText?.Played();

            if (currentGameText?.NextGameText == null)
            {
                currentGameText = null;

                if (quedGameText == null)
                    return false;

                currentGameText = quedGameText;
                quedGameText = null;
            }
            else
            {
                currentGameText = currentGameText.NextGameText;
            }

            m_currentEntry = -1;

            currentGameText.Started();

            if (currentGameText.Portrait != null)
            {
                Portrait.sprite = currentGameText.Portrait;
                if (!Portrait.transform.parent.gameObject.activeSelf) Portrait.transform.parent.gameObject.SetActive(true);
            }

            else
                Portrait.transform.parent.gameObject.SetActive(false);

            GameTextOwner.text = currentGameText.TextOwner;

            if (m_playerAnimator != null)
                m_playerAnimator.Kill(true);

            if (GameTextPlayerCanvas.enabled)
                NextTextEntry();
            else
                PlayerEntry();

            OnNextGameText?.Invoke();
            return true;
        }

        public bool NextTextEntry()
        {
            m_currentEntry++;

            if (m_currentEntry == currentGameText.TextEntries.Count)
            {
                return NextGameText();
            }

            GameText.maxVisibleCharacters = 9999;
            GameText.text = "";
            GameText.text = currentGameText.TextEntries[m_currentEntry].value;
            GameText.ForceMeshUpdate();

            m_characterCount = GameText.text.Length;
            GameText.maxVisibleCharacters = 0;
            m_textSequence =
                DOTween.Sequence()
                .OnStart(() => AudioSource.Play())
                .AppendInterval(CharacterPrintTime)
                .AppendCallback(() =>
                {
                    GameText.maxVisibleCharacters++;
                })
                .SetLoops(m_characterCount)
                .OnComplete(() =>
                {
                    GameText.maxVisibleCharacters = m_characterCount;
                    AudioSource.Stop();
                });

            OnNextTextEntry?.Invoke();
            return true;
        }

        public void SkipToNext(UnityEngine.InputSystem.InputAction.CallbackContext inputCallback)
        {
            if (SkipButton.gameObject.activeSelf) SkipToNext();
        }

        public void SkipToNext()
        {
            if (ExitButton.gameObject.activeInHierarchy)
                return;

            if (!m_textSequence.active)
            {
                if (!NextTextEntry())
                {
                    ExitButton.gameObject.SetActive(true);
                    SkipButton.gameObject.SetActive(false);
                }
                return;
            }

            m_textSequence.Kill(true);
        }

    }
}