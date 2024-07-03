using Cysharp.Threading.Tasks;
using Match3System.Core.Models;
using UnityEngine;

namespace Game.Scripts.Core.Interfaces
{
    public interface IItem
    {
        ItemType ItemType { get; }

        Transform Transform { get; }
        SpriteRenderer SpriteRenderer { get; }

        void Show();
        void Hide();

        void SetItem(Sprite sprite,ItemType itemType);
        void SetPosition(Vector3 position);
        Vector3 GetPosition();
        void SetScale(float value);

        UniTask ItemClicked(GameBoard board, GridPoint point);
    }
}
