using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities.Pooling
{
    public interface IPoolable
    {
        void OnSpawn();
        void OnDespawn();
    }

    public sealed class Pool
    {
        private int _poolSize = 0;
        private Queue<Poolable> _entities = null;
        private GameObject _prefab = null;
        private Poolable _entity = null;

        private static Dictionary<int, Pool> _pools = new Dictionary<int, Pool>();

        public Pool(Poolable entity, int size)
        {
            _prefab = entity.gameObject;
            _entities = new Queue<Poolable>(size);

            _pools.Add(_prefab.GetHashCode(), this);

            FillPool(size);
        }

        public Pool(GameObject prefab, int size)
        {
            _prefab = prefab;
            _entities = new Queue<Poolable>(size);

            _pools.Add(_prefab.GetHashCode(), this);

            FillPool(size);
        }

        public Pool(GameObject prefab, GameObject[] exisiting)
        {
            _prefab = prefab;
            _entities = new Queue<Poolable>(exisiting.Length);

            _pools.Add(_prefab.GetHashCode(), this);

            FillPool(exisiting);
        }

        public static Pool Find(GameObject prefab)
        {
            Pool pool;
            _pools.TryGetValue(prefab.GetHashCode(), out pool);

            if (pool == null)
                pool = new Pool(prefab, 0);

            return pool;
        }

        public void Release()
        {
            if (_pools.ContainsKey(_prefab.GetHashCode()))
                _pools.Remove(_prefab.GetHashCode());

            _entities.Clear();
            _poolSize = 0;
        }

        public void FillPool(int size)
        {
            for (int i = 0; i < size; i++)
            {
                _entity = GameObject.Instantiate(_prefab).AddComponent<Poolable>();
                _entity.gameObject.SetActive(false);

                _entity.AddToPool(this);
                _entities.Enqueue(_entity);
            }

            _poolSize += size;
        }

        public void FillPool(GameObject[] exisiting)
        {
            for (int i = 0; i < exisiting.Length; i++)
            {
                _entity = exisiting[i].AddComponent<Poolable>();

                _entity.gameObject.SetActive(false);

                _entity.AddToPool(this);
                _entities.Enqueue(_entity);
            }

            _poolSize += exisiting.Length;
        }

        private Poolable TakeFromPool()
        {
            Poolable entity;

            if (_poolSize == 0)
            {
                entity = GameObject.Instantiate(_prefab).AddComponent<Poolable>();
                entity.AddToPool(this);
                entity.gameObject.SetActive(true);

                return entity;
            }

            entity = _entities.Dequeue();

            if (entity == null)
            {
                entity = GameObject.Instantiate(_prefab).AddComponent<Poolable>();
                entity.AddToPool(this);

                _poolSize++;
            }

            entity.gameObject.SetActive(true);
            _poolSize--;

            return entity;
        }

        public void ReturnToPool(Poolable entity)
        {
            if (entity.Pool != this)
                return;

            _entities.Enqueue(entity);
            _poolSize++;

            entity.gameObject.SetActive(false);
            entity.gameObject.transform.SetParent(null, false);
        }

        public Poolable GrabEntity()
        {
            _entity = TakeFromPool();

            _entity.TakenFromPool();
            return _entity;
        }

        public Poolable GrabEntity(Transform parent, bool spawnInWorldSpace)
        {
            _entity = TakeFromPool();

            _entity.gameObject.transform.SetParent(parent, spawnInWorldSpace);

            _entity.TakenFromPool();
            return _entity;
        }

        public Poolable GrabEntity(Vector3 position, Quaternion rotation)
        {
            _entity = TakeFromPool();

            _entity.gameObject.transform.SetPositionAndRotation(position, rotation);

            _entity.TakenFromPool();
            return _entity;
        }

        public Poolable GrabEntity(Vector3 position, Quaternion rotation, Transform parent, bool spawnInWorldSpace)
        {
            _entity = TakeFromPool();
            Transform entityTransform = _entity.gameObject.transform;

            entityTransform.SetParent(parent, spawnInWorldSpace);
            entityTransform.SetPositionAndRotation(position, rotation);

            _entity.TakenFromPool();
            return _entity;
        }

        public T GrabEntity<T>() where T : Component =>
            GrabEntity().gameObject.GetComponent<T>();

        public T GrabEntity<T>(Transform parent, bool spawnInWorldSpace) where T : Component =>
            GrabEntity(parent, spawnInWorldSpace).gameObject.GetComponent<T>();

        public T GrabEntity<T>(Vector3 position, Quaternion rotation) where T : Component =>
            GrabEntity(position, rotation).gameObject.GetComponent<T>();

        public T GrabEntity<T>(Vector3 position, Quaternion rotation, Transform parent, bool spawnInWorldSpace) where T : Component =>
            GrabEntity(position, rotation, parent, spawnInWorldSpace).gameObject.GetComponent<T>();
    }

    public static class PoolExtensions
    {
        private static Pool _cachedPool;
        private static Poolable _cachedPoolable;
        private static bool _cachedBool;
        public static void FillPool(this GameObject prefab, int size)
        {
            _cachedPool = Pool.Find(prefab);
            _cachedPool.FillPool(size);
        }

        public static Poolable Spawn(this GameObject prefab)
        {
            _cachedPool = Pool.Find(prefab);
            _cachedPoolable = _cachedPool.GrabEntity();

            return _cachedPoolable;
        }

        public static void Despawn(this GameObject instance)
        {
            _cachedBool = Poolable.IsPooled(instance);

            if (_cachedBool)
            {
                instance.GetComponent<Poolable>().ReturnToPool();
                return;
            }

            GameObject.Destroy(instance);
        }

        public static void Despawn(this Poolable entity)
        {
            if (entity.InPool)
            {
                entity.ReturnToPool();
                return;
            }

            GameObject.Destroy(entity.gameObject);
        }
    }
}
