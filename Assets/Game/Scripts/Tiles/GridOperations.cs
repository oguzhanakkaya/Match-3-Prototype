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
    public static async UniTask ClearTileAsync(IGridNode grid)
    {
        grid.Item.Hide();
        grid.Clear();
    }
    public static void ClearTile(IGridNode grid)
    {
        grid.Item.Hide();
        grid.Clear();
    }
}
