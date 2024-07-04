using Game.Scripts.Core.Interfaces;
using Match3System.Core.Interfaces;
using Match3System.Core.Models;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public static class MatchSolver
{
    private static readonly GridPoint[]  _lineDirections = new[] { GridPoint.Left, GridPoint.Right };

    private static List<MatchedItems<IGridNode>> matchedItems = new List<MatchedItems<IGridNode>>();



    public static List<MatchedItems<IGridNode>> GetMatches(GameBoard _gameBoard)
    {
        matchedItems.Clear();

        for (var rowIndex = 0; rowIndex < _gameBoard.RowCount; rowIndex++)
        {
            for (var columnIndex = 0; columnIndex < _gameBoard.ColumnCount; columnIndex++)
            {
                GridPoint point=new GridPoint(rowIndex, columnIndex);

                if (!CheckAllPointsOnGrid(point, _gameBoard))
                    continue;

                if (CheckMatch(point,_gameBoard))
                {
                     List<GridPoint> matchedGridPoint=new List<GridPoint>();

                    matchedGridPoint.Add(point);

                    foreach (var item in _lineDirections)
                    {
                        matchedGridPoint.Add(point+item);
                    }

                    matchedItems.Add(new MatchedItems<IGridNode>(_gameBoard[point].Item.ItemType, matchedGridPoint));

                }
            }
        }
        return matchedItems;
    }
    private static bool IsPointOnGrid(GridPoint point,GameBoard _gameBoard)
    {
        return point.RowIndex >= 0 &&
               point.RowIndex < _gameBoard.RowCount &&
               point.ColumnIndex >= 0 &&
               point.ColumnIndex < _gameBoard.ColumnCount;
            
    }
    private static bool CheckAllPointsOnGrid(GridPoint point,GameBoard _gameBoard)
    {
        foreach (var item in _lineDirections)
        {
            if (!IsPointOnGrid(point + item, _gameBoard))
                return false;
        }
        return true;
    }
    private static bool CheckMatch(GridPoint point, GameBoard _gameBoard)
    {
        foreach (var item in _lineDirections)
        {
            if (_gameBoard[point].Item.ItemType != 
                _gameBoard[point+item].Item.ItemType)
                return false;
        }
        return true;
    }
}
