using Game.Scripts.Core.Interfaces;
using PoolSystem;
using PoolSystem.Core;
using UnityEngine;

namespace Game.Scripts.Core
{
    public abstract class ItemGenerator<T> : IItemGenerator where T : Component, IPoolObject<T>, IItem
    {
        protected IPool<T> _pool { get; }
        
        public ItemGenerator(IPool<T> pool)
        {
            _pool = pool;
        }
        public void CreateItems(int capacity)
        {
            for (int i = 0; i < capacity; i++)
            {
                _pool.CreateItem();
            }
        }
        
        public void Dispose()
        {
        }

        public IItem GetItem(LevelData levelData)
        {
            return ConfigureItem<T>(_pool.Spawn(),levelData);
        }
        public IItem GetSpecificItem(ItemType type)
        {
            return ConfigureItem<T>(_pool.Spawn(),type);
        }
        protected abstract IItem ConfigureItem<T1>(T item, LevelData levelData) where T1 : Component, T, IPoolObject<T>, IItem;
        protected abstract IItem ConfigureItem<T1>(T item,ItemType itemType) where T1 : Component, T, IPoolObject<T>, IItem;
    }
}
