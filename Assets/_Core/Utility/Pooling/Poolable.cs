using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Utilities.Pooling
{
    [DisallowMultipleComponent]
    public class Poolable : MonoBehaviour
    {
        public Pool Pool { get; private set; } = null;
        public bool InPool { get; private set; } = false;
        private IPoolable[] _entities;

        private static HashSet<int> _pooledObjects = new HashSet<int>();

        public static bool IsPooled(GameObject instance) => _pooledObjects.Contains(instance.GetHashCode());

        void Awake()
        {
            _entities = gameObject.GetComponentsInChildren<IPoolable>(true);
        }

        public void AddToPool(Pool pool)
        {
            if (InPool)
                return;

            Pool = pool;
            InPool = true;

            _pooledObjects.Add(gameObject.GetHashCode());
        }

        public void TakenFromPool()
        {
            for (int i = 0; i < _entities.Length; i++)
                _entities[i].OnSpawn();
        }

        public void ReturnToPool()
        {
            for (int i = 0; i < _entities.Length; i++)
                _entities[i].OnDespawn();

            Pool.ReturnToPool(this);
        }

        void OnDestroy()
        {
            if (InPool)
            {
                _pooledObjects.Remove(gameObject.GetHashCode());
            }
        }
    }
}