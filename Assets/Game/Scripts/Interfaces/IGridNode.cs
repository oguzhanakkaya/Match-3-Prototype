using Game.Scripts.Core.Interfaces;

namespace Match3System.Core.Interfaces
{
    public interface IGridNode
    {
        bool HasItem { get; }

        IItem Item { get; }


        void SetItem(IItem item);
        void Clear();
    }
}
