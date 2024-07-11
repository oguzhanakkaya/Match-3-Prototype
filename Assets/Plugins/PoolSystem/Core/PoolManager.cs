using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Lean.Pool;


namespace PoolSystem.Core
{
    public class PoolManager : MonoBehaviour
    {
        public List<PoolData> _poolDataList;

        public Component GetComponentFromID(string id)
        {
            return _poolDataList.First(i => i.PrefabId == id).Prefab;
        }
        public Component GetParticle()
        {
            var obj=_poolDataList.First(x=>x.PrefabId== "particle_object").Prefab;
            LeanPool.Spawn(obj);

            return null;
         //   return (ParticleObject)obj;
        }
    }
}
       