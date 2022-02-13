using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities.Pooling
{
    [DefaultExecutionOrder(-1)]
    public class PoolCreator : MonoBehaviour
    {
        [SerializeField] GameObject prefab;
        [SerializeField] GameObject[] existingPool;

        void Awake() => new Pool(prefab, existingPool);
    }
}