using UnityEngine;

namespace Game.Scripts.Core.Interfaces
{
    public interface IGridTile
    {
        void SetActive(bool value);
        void SetWorldPosition(Vector3 worldPosition);
    }
}
