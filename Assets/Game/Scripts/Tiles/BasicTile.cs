using System;
using Game.Scripts.Core.DependencyInjection;
using Game.Scripts.Core.DependencyInjection.Interfaces;
using Game.Scripts.Core.Interfaces;
using PoolSystem;
using UnityEngine;

namespace Game.Scripts.Tiles
{
    [RequireComponent(typeof(AutoInjector))]
    public class BasicTile : MonoBehaviour, IGridTile, IPoolObject<BasicTile>, IHasAutoInjector
    {
        [SerializeField] private string _prefabId;

        public string PrefabId => _prefabId;
        public BasicTile Component => this;
        public IPool<BasicTile> Pool { get; private set; }

        [Inject] private IPoolManager _poolManager;

        public void OnFieldValuesInjected()
        {
            Pool = _poolManager.GetPool<BasicTile>(_prefabId);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        
        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }

        public void SetWorldPosition(Vector3 worldPosition)
        {
            transform.position = worldPosition;
        }
    }
}
