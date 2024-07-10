using Cysharp.Threading.Tasks;
using Game.Scripts.Core.Interfaces;
using PoolSystem;
using UnityEngine;

namespace Game.Scripts.Core
{
    public class ItemBase : MonoBehaviour, IItem, IPoolObject<ItemBase>
    {
        [SerializeField] private string _prefabId;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private int itemType;
        [SerializeField] private bool isUsableItem;
        public IPool<ItemBase> Pool { get; private set; }
        public bool IsUsableItem => isUsableItem;
        public int ItemType => itemType;
        public Transform Transform => transform;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        public string PrefabId => _prefabId;
        public ItemBase Component => this;

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
        public async UniTask Use()
        {

        }
    }
}
