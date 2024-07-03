using System;

namespace Game.Scripts.Core.Interfaces
{
    public interface IItemGenerator : IDisposable
    {
        void CreateItems(int capacity);
        
        IItem GetItem(LevelData levelData);
        IItem GetSpecificItem(ItemType type);
    }
}
