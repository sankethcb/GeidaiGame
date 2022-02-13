using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Core.Input
{
    [CreateAssetMenu(fileName = "InputActionData", menuName = "Core/Input/Input Action Data", order = 1)]
    public class InputActionData : ScriptableObject
    {
        [SerializeField] InputActionReference m_inputReference;
        [SerializeField] List<InputAction> inputActions;

        public InputAction InputAction() => m_inputReference.action;

        public InputAction InputAction(int playerIndex = 0) => inputActions[playerIndex];

        public void AddInputAction(InputAction inputAction, int playerIndex = 0)
        {
            if (inputActions.Count <= playerIndex)
            {
                if (inputActions.Count == playerIndex)
                    inputActions.Add(inputAction);
                else
                {
                    while (inputActions.Count < playerIndex)
                    {
                        inputActions.Add(new UnityEngine.InputSystem.InputAction());
                    }
                    inputActions.Add(inputAction);
                }
            }
            else
                inputActions[playerIndex] = inputAction;
        }
    }
}
