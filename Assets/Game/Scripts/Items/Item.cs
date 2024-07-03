using Cysharp.Threading.Tasks;
using Game.Scripts.Core.DependencyInjection;
using Game.Scripts.Core.Interfaces;
using Game.Scripts.Items;
using Match3System.Core.Models;
using PoolSystem;
using PoolSystem.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Core
{
    public class Item : MonoBehaviour, IItem, IPoolObject<Item>
    {
        [SerializeField] private string _prefabId;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        public IPool<Item> Pool { get; private set; }
        private ItemType itemType;

        public ItemType ItemType => itemType;
        public Transform Transform => transform;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        public string PrefabId => _prefabId;
        public Item Component => this;

        [Inject] private IPoolManager _poolManager;

        private void Start()
        {
            Pool = _poolManager.GetPool<Item>(PrefabId) as IPool<Item>;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SetItem(Sprite sprite, ItemType type)
        {
            itemType = type;
            _spriteRenderer.sprite = sprite;
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public void SetScale(float value)
        {
            transform.localScale = new Vector3(value, value, value);
        }

        public async UniTask ItemClicked(GameBoard board,GridPoint point)
        {
            var matchedList = MatchDetector.GetMatchedItems(board[point], board);

            if (matchedList == null || matchedList.Count < 2)
                return;

            // _audioManager.PlayAudio(Audios.Explode);


            List<UniTask> tasks = new List<UniTask>();

            foreach (var item in matchedList)
            {
              //  StartParticle(item);
                tasks.Add(GridOperations.ClearTileAsync(item));
            }
            await UniTask.WhenAll(tasks);

         /*   if (matchedList.Count >= 5) // Check Can Create Rocket
            {
                var rocketItem = _rocketGenerator.GetItem();
                fillClass.FillOneObject(_gameBoard, point, rocketItem);
            }
         */
        }
    }
}
