using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace PoolSystem.Core
{
    [Serializable]
    public struct PoolData
    {
        public string PrefabId;
        public Component Prefab;
    }
}