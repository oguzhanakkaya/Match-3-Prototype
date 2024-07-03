using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Core.Interfaces;
using PoolSystem;
using UnityEngine;

public class ParticleGenerator : MonoBehaviour
{
    protected IPool<ParticleObject> _pool { get; }

    public ParticleGenerator(IPool<ParticleObject> pool)
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

    public ParticleObject GetItem()
    {
        return _pool.Spawn();
    }
}
