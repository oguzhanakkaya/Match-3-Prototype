using UnityEngine;

namespace PoolSystem
{
    public interface IPoolManager
    {
        IPool<T> GetPool<T>(string prefabId) where T : Component, IPoolObject<T>;
    }
    public interface IPoolBase
    {
        string PrefabId { get; }
        void SetParent(Transform parent);
        void SetPrefab<T>(T prefab) where T : Component;
        void RecycleAll();
    }
    public interface IPool<T> : IPoolBase where T : Component
    {
        T Prefab { get; }
        Transform Parent { get; }
        T CreateItem();
        T Spawn();
        void Recycle(T item);
    }
    public interface IPoolObject<T> where T : Component
    {
        string PrefabId { get; }
        T Component { get; }
        IPool<T> Pool { get; }
        void Show();
        void Hide();
    }
}
