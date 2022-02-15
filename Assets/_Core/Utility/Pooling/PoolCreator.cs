using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities.Pooling
{
    [DefaultExecutionOrder(-1)]
    public class PoolCreator : MonoBehaviour
    {
        public Pool Pool;

        [SerializeField] GameObject prefab;
        [SerializeField] GameObject[] existingPool;

        void Awake() => Pool = new Pool(prefab, existingPool);
    }
}