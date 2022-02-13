using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace Core.Input
{
    public class InputEventSystem : MonoBehaviour
    {
        [SerializeField] int playerIndex = 0;

        [CollectionLabel("inputActionData")]
        [SerializeField] InputReciever[] inputRecievers;

        public int PlayerIndex => playerIndex;
        public void SetPlayerIndex(int playerIndex) => this.playerIndex = playerIndex;

        void OnEnable()
        {
            for (int i = 0; i < inputRecievers.Length; i++)
                inputRecievers[i].RegisterInput(playerIndex);
        }

        void OnDisable()
        {
            for (int i = 0; i < inputRecievers.Length; i++)
                inputRecievers[i].DeregisterInput();
        }

    }
}
