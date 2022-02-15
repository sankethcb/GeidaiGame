using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "GameSystems", menuName = "Core/Game/Game Systems", order = 1)]
    public class GameSystems : ScriptableObject
    {
        [SerializeField]  GameObject[] systems;

        public GameObject[] GetSystems => systems;
    }
}
