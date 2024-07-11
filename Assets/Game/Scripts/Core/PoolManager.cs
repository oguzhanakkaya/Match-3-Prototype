using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Lean.Pool;


namespace PoolSystem.Core
{
    public class PoolManager : MonoBehaviour
    {
        public List<PoolData> _poolDataList;

        public ParticleObject _particleObject;

        public Component GetComponentFromID(string id)
        {
            return _poolDataList.First(i => i.PrefabId == id).Prefab;
        }
        public ParticleObject GetParticle()
        {
            return LeanPool.Spawn(_particleObject);
        }
    }
}
       