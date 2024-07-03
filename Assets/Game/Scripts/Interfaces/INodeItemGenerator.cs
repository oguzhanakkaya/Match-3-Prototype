using UnityEngine;

namespace Game.Scripts.Core.Interfaces
{
    public interface INodeItemGenerator : IItemGenerator
    {
        void SetGameData(GameData gameData);
    }
}