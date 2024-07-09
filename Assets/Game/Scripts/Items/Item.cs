using Cysharp.Threading.Tasks;
using Game.Scripts.Core.DependencyInjection;
using Game.Scripts.Core.Interfaces;
using Match3System.Core.Models;
using PoolSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Core
{
    public class Item : MonoBehaviour, IItem, IPoolObject<Item>
    {
        [SerializeField] private string _prefabId;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        public IPool<Item> Pool { get; private set; }
        [SerializeField]private int itemType;
        private bool usable;
        public bool Usable => usable;
        public int ItemType => itemType;
        public Transform Transform => transform;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        public string PrefabId => _prefabId;
        public Item Component => this;

        private void Start()
        {
         //   Pool = _poolManager.GetPool<Item>(PrefabId) as IPool<Item>;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SetItem(Sprite sprite, int type)
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
        public virtual void Use()
        {

        }
    }
}
