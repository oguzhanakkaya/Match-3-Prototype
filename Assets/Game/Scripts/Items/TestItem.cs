using System.Collections;
using Cysharp.Threading.Tasks;
using Game.Scripts.Core.DependencyInjection;
using Plugins.PoolSystem.Extensions;
using PoolSystem;
using PoolSystem.Core;
using UnityEngine;

namespace Game.Scripts.Items
{
    public class TestItem : MonoBehaviour, IPoolObject<TestItem>, ISpawnCallbackReceiver
    {
        [SerializeField] private string _prefabId;

        public string PrefabId => _prefabId;
        public TestItem Component => this;
        public IPool<TestItem> Pool { get; private set; }

        [Inject] private IPoolManager _poolManager;

        private void Start()
        {
            Pool = _poolManager.GetPool<TestItem>(_prefabId);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void OnSpawned()
        {
            // RecycleAsync(1000);
        }

        private async void RecycleAsync(int delayInMilliseconds)
        {
            await UniTask.Delay(delayInMilliseconds);
            
            this.Recycle();
        }
    }
}
