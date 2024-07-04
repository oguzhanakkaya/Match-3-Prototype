using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PoolSystem.Core
{
    public class Pool<T> : IPool<T> where T : Component, IPoolObject<T>
    {
        public string PrefabId => Prefab.PrefabId;
        public T Prefab { get; private set; }
        public Transform Parent { get; private set; }
        private HashSet<T> _activeItems;
        private Queue<T> _inactiveItems;

        public Pool()
        {
            _activeItems = new HashSet<T>();
            _inactiveItems = new Queue<T>();
        }

        public Pool(T prefab, Transform parent)
        {
            Prefab = prefab;
            Parent = parent;
            _activeItems = new HashSet<T>();
            _inactiveItems = new Queue<T>();
        }

        public T CreateItem()
        {
            var item = UnityEngine.Object.Instantiate(Prefab, Parent);
            item.TryGetComponent(out ICreationCallbackReceiver creationCallbackReceiver);
            creationCallbackReceiver?.OnCreated();
            item.Hide();
            _inactiveItems.Enqueue(item);
            return item;
        }
        public T Spawn()
        {
            if (_inactiveItems.TryDequeue(out var item))
            {
                _activeItems.Add(item);
                item.Show();
                item.TryGetComponent(out ISpawnCallbackReceiver spawnCallbackReceiver);
                spawnCallbackReceiver?.OnSpawned();
                return item;
            }
            else
            {
                item = CreateItem();
                _inactiveItems.Dequeue();
                _activeItems.Add(item);
                item.TryGetComponent(out ISpawnCallbackReceiver spawnCallbackReceiver);
                spawnCallbackReceiver?.OnSpawned();
                item.Show();
                return item;
            }
        }
        public void Recycle(T item)
        {
            item.Hide();
            item.transform.SetParent(Parent, false);
            _activeItems.Remove(item);
            _inactiveItems.Enqueue(item);
            item.TryGetComponent(out IRecycleCallbackReceiver recycleCallbackReceiver);
            recycleCallbackReceiver?.OnRecycled();
        }
        public void SetParent(Transform parent)
        {
            Parent = parent;
        }
        public void SetPrefab<T1>(T1 prefab) where T1 : Component
        {
            if (prefab is T p)
            {
                Prefab = p;
            }
        }
        public void RecycleAll()
        {
            while (_activeItems.Count > 0)
            {
                var item = _activeItems.First();
                Recycle(item);
            }
        }
    }
}