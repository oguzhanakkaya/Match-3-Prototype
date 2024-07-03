using System;
using System.Collections.Generic;
using UnityEngine;

namespace PoolSystem.Core
{
    public class PoolManager : MonoBehaviour, IPoolManager
    {
        [SerializeField] private List<PoolData> _poolDataList;

        private readonly Dictionary<string, object> _poolDictionary = new();

        public void Init()
        {
            foreach (var poolData in _poolDataList)
            {
                var objectType = poolData.Prefab.GetType();
                var constructedType = typeof(Pool<>).MakeGenericType(objectType);
                var pool = Activator.CreateInstance(constructedType) as IPoolBase;
                if (pool == null) throw new ArgumentException($"Could not create instance of pool!, KEY: [{poolData.PrefabId}]");
                var parent = new GameObject($"Pool_[{poolData.PrefabId}]");
                parent.transform.SetParent(transform);
                pool.SetPrefab(poolData.Prefab);
                pool.SetParent(parent.transform);
                _poolDictionary[poolData.PrefabId] = pool;
            }
        }

        public IPool<T> GetPool<T>(string prefabId) where T : Component, IPoolObject<T> 
        {
            if (_poolDictionary.TryGetValue(prefabId, out var pool))
            {
                return Convert.ChangeType(pool, typeof(Pool<T>)) as IPool<T>;
            }

            Debug.LogError($"There is not a valid pool with PrefabId: {prefabId}!");
            return null;
        }

        public void ConvertType<TCurrent, TTarget>(TCurrent current) where TCurrent : IPoolBase where TTarget : IPoolBase
        {
            var newTypedObject = Convert.ChangeType(current, typeof(TTarget));

            _poolDictionary[current.PrefabId] = newTypedObject;
        }
    }
}
