using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using Game.Scripts.Core.Interfaces;
using Match3System.Core.Interfaces;
using Match3System.Core.Models;
using UnityEngine;

public static class MatchDetector 
{
     private static readonly GridPoint[] pointDirections= new[] { GridPoint.Left,
                                                                  GridPoint.Right,
                                                                  GridPoint.Up,
                                                                  GridPoint.Down};

 //   private static readonly List<ItemType> nonIdMatchableItems = new List<ItemType> { ItemType.Baloon };

    public static IReadOnlyList<IGridNode> GetMatchedItems(IGridNode node,GameBoard _gameBoard)
     {
        var match=GetMatches(_gameBoard);

        foreach (var item in match)
        {
            foreach (var item2 in item.matchedGridSlot)
            {
                if (item2 == node)
                    return item.matchedGridSlot;
            }
        }
        return null;
     }

     public static List<MatchedItems<IGridNode>> GetMatches(GameBoard _gameBoard)
     {
        List<MatchedItems<IGridNode>> matchedItems = new List<MatchedItems<IGridNode>>();

        for (var rowIndex = 0; rowIndex < _gameBoard.RowCount; rowIndex++)
        {
            for (var columnIndex = 0; columnIndex < _gameBoard.ColumnCount; columnIndex++)
            {
                if (IsItemAlreadyMatch(_gameBoard[new GridPoint(rowIndex, columnIndex)].Item, matchedItems))
                    continue;

                var match= GetMatch(new GridPoint(rowIndex, columnIndex),_gameBoard);

                if (match.matchedGridSlot.Count > 1)
                    matchedItems.Add(match);
            }
        }
        return matchedItems;
     }

    private static MatchedItems<IGridNode> GetMatch(GridPoint point,GameBoard _gameBoard)
    {
        List<IGridNode> matchedGridSlot = new List<IGridNode>();
        List<IGridNode> checkedItems = new List<IGridNode>();

        IGridNode node = _gameBoard[new GridPoint(point.RowIndex, point.ColumnIndex)];

        CheckMatch(node.Item.ItemType,point,_gameBoard,matchedGridSlot,checkedItems);

        return new MatchedItems<IGridNode>(node.Item.ItemType, matchedGridSlot);
    }
    private static void CheckMatch(ItemType itemType,GridPoint point,GameBoard _gameBoard, List<IGridNode> matchedGridSlot,List<IGridNode> checkedItems)
    {
        if (!(point.RowIndex >= 0 &&
               point.RowIndex < _gameBoard.RowCount &&
               point.ColumnIndex >= 0 &&
               point.ColumnIndex < _gameBoard.ColumnCount))
            return;

        IGridNode gridNode = _gameBoard[point];

        bool checkItemId = gridNode.Item.ItemType == itemType; /*|| nonIdMatchableItems.Contains(gridNode.Item.ItemType);*/

        if (checkedItems.Contains(gridNode) ||
            matchedGridSlot.Contains(gridNode) ||
            !checkItemId )
            return;

        checkedItems.Add(gridNode);
        matchedGridSlot.Add(gridNode);

       /* if (gridNode.Item.ItemType == ItemType.Baloon)
            return;
       */

        foreach (var item in pointDirections)
        {
            var newPoint = item + point;
            CheckMatch(gridNode.Item.ItemType, newPoint, _gameBoard,matchedGridSlot,checkedItems);
        }
    }


    private static bool IsItemAlreadyMatch(IItem newItem, List<MatchedItems<IGridNode>> matchedItemsList)
    {
        return matchedItemsList.Any(item => item.matchedGridSlot.Any(node => node.Item == newItem));
    }
}
