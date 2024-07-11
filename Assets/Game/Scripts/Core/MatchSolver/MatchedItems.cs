using System.Collections.Generic;
using Match3System.Core.Interfaces;
using Match3System.Core.Models;

public class MatchedItems<TGridSlot> where TGridSlot : IGridNode
{
    public MatchedItems(string itemType, List<GridPoint> matchedGridPoint)
    {
        this.itemType = itemType;
        this.itemsList = matchedGridPoint;
    }

    public string itemType { get; }
    public List<GridPoint> itemsList { get; set; }
}
