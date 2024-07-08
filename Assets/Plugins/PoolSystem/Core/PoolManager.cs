using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

namespace PoolSystem.Core
{
    public class PoolManager : MonoBehaviour
    {
        [SerializeField] private List<PoolData> _poolDataList;

        public Component GetComponentFromID(string id)
        {
            return _poolDataList.First(i => i.PrefabId == id).Prefab;
        }
    }
}
       