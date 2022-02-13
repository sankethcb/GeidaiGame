using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Core.Input
{
    [DefaultExecutionOrder(-10)]
    [RequireComponent(typeof(PlayerInput))]
    public class InputHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] PlayerInput playerInput;

        [SerializeField] PointerData pointerData;
        [Header("Settings")]
        [SerializeField] List<InputActionData> inputActionDataSet;

        public static int PlayerCount { get; private set; }

        void Awake()
        {
            if (!playerInput) playerInput = GetComponent<PlayerInput>();

            playerInput.onActionTriggered += ProcessInputEvent;
            playerInput.onControlsChanged += CurrentDeviceSwitched;

            InputSystem.onDeviceChange += DeviceChanged;

            PlayerCount++;

            RegisterPlayerInput(playerInput.actions, playerInput.playerIndex);


            DontDestroyOnLoad(gameObject);
        }

        void OnDestroy()
        {
            PlayerCount--;
            playerInput.onActionTriggered -= ProcessInputEvent;
        }


        void OnEnable()
        {
            playerInput.ActivateInput();
        }

        void OnDisable()
        {
            playerInput.DeactivateInput();
        }

        public void RegisterPlayerInput(InputActionAsset playerAsset, int playerIndex = 0)
        {
            for (int i = 0; i < inputActionDataSet.Count; i++)
            {
                inputActionDataSet[i].AddInputAction(playerAsset.FindAction(inputActionDataSet[i].InputAction().name), playerIndex);
            }

            Debug.Log("Player " + playerIndex + " input registered!");
        }

        public void RegisterPlayerInput()
        {
            foreach (InputActionData inputActionData in inputActionDataSet)
            {

            }
        }

        void ProcessInputEvent(InputAction.CallbackContext inputCallback)
        {

        }

        void Update()
        {
            pointerData.TrackPointer(Pointer.current.position.ReadValue());
            pointerData.TrackDelta(Pointer.current.delta.ReadValue());

        }

        void DeviceChanged(InputDevice device, InputDeviceChange deviceChange)
        {
            switch (deviceChange)
            {
                case InputDeviceChange.Added:
                    break;
                case InputDeviceChange.Removed:
                    break;
                case InputDeviceChange.Reconnected:
                    break;
                case InputDeviceChange.Disconnected:
                    break;
            }
        }

        void CurrentDeviceSwitched(PlayerInput playerInput)
        {
            Debug.Log("Player " + (playerInput.playerIndex) + " controls switched to" + playerInput.currentControlScheme.ToString());
        }


        public static void SaveBindingsToPrefs(InputActionAsset actionsAsset, string key)
        {
            PlayerPrefs.SetString(key, actionsAsset.ToJson());
        }

        public static void LoadBindingsFromPrefs(InputActionAsset actionsAsset, string key)
        {
            actionsAsset.LoadFromJson(PlayerPrefs.GetString(key));
        }
    }
}