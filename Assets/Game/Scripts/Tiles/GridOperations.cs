using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Scripts.Core;
using Game.Scripts.Core.Interfaces;
using Match3System.Core.Interfaces;
using Match3System.Core.Models;
using UnityEngine;
using UnityEngine.Playables;

public static class GridOperations
{
    public static void ClearMatchedItem(List<MatchedItems<IGridNode>> matchedItemsList,GameBoard _gameBoard)
    {
        foreach (var item in matchedItemsList)
        {
            foreach (var item2 in item.matchedItems)
            {
                ClearTile(_gameBoard[item2]);
            }
        }
    }
    private static async UniTask ClearTileAsync(IGridNode grid)
    {
        grid.Item.Hide();
        grid.Clear();
    }
    private static void ClearTile(IGridNode grid)
    {
        grid.Item.Hide();
        grid.Clear();
    }
}
