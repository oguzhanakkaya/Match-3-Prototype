using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Scripts.Core.Interfaces
{
    public interface IItem
    {
        string PrefabId { get; }
        bool IsUsableItem { get; }

        Transform Transform { get; }
        SpriteRenderer SpriteRenderer { get; }

        void Show();
        void Hide();
        void SetItem();

        void SetPosition(Vector3 position);
        void SetScale(float value);
        Vector3 GetPosition();
        UniTask Use();
    }
}
