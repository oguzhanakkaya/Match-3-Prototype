using System.Collections;
using System.Collections.Generic;
using Match3System.Core.Interfaces;
using UnityEngine;

public class MatchedItems<TGridSlot> where TGridSlot : IGridNode
{
    public MatchedItems(ItemType itemType, IReadOnlyList<TGridSlot> matchedGridSlot)
    {
        this.itemType = itemType;
        this.matchedGridSlot = matchedGridSlot;
    }

    public ItemType itemType { get; }
    public IReadOnlyList<TGridSlot> matchedGridSlot { get; }
}
