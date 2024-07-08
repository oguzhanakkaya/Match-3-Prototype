using System;
using System.Diagnostics;
using Game.Scripts.Core.Interfaces;
using Match3System.Core.Interfaces;
using Match3System.Core.Models;

namespace Game.Scripts.Core
{
    public class GridNode : IGridNode
    {
        public GridPoint GridPoint { get; }
        
        public GridNode(GridPoint gridPoint)
        {
            GridPoint = gridPoint;
        }

        public bool HasItem => Item != null;
        public IItem Item { get; private set; }
        
        public void SetItem(IItem item)
        {
            Item = item;
        }

        public void Clear()
        {
            Item = null;
        }
    }
}
