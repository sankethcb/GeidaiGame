using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace Core
{
    public class PauseHandler : MonoBehaviour
    {
        [Header("References")]
        public TimeController TimeController;

        [Header("Variables")]
        [SerializeField] [Utilities.ReadOnly] bool m_paused = false;

        [Header("Events")]
        public UnityEvent OnPause;
        public UnityEvent OnUnpause;

        public void TogglePauseState(InputAction.CallbackContext inputCallback) => TogglePauseState();
        public void TogglePauseState()
        {
            m_paused = !m_paused;

            if (m_paused)
            {
                PauseGame();

            }
            else
            {
                UnpauseGame();
            }
        }

        void PauseGame()
        {
            TimeController.FreezeTime();
            AudioListener.pause = true;
            OnPause?.Invoke();
        }

        void UnpauseGame()
        {
            TimeController.ResumeFrozenTime();
            AudioListener.pause = false;
            OnUnpause?.Invoke();
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void QuitGame(int exitCode = 0)
        {
            Application.Quit(exitCode);
        }
    }
}