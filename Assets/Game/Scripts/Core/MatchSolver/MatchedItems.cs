using System.Collections.Generic;
using Match3System.Core.Interfaces;
using Match3System.Core.Models;

public class MatchedItems<TGridSlot> where TGridSlot : IGridNode
{
    public MatchedItems(ItemType itemType, List<GridPoint> matchedGridPoint)
    {
        this.itemType = itemType;
        this.matchedItems = matchedGridPoint;
    }

    public ItemType itemType { get; }
    public List<GridPoint> matchedItems { get; set; }
}
