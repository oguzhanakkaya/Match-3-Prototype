using Match3System.Core.Interfaces;
using Match3System.Core.Models;
using System.Collections.Generic;
using System.Linq;

public static class MatchSolver
{
    private static List<MatchedItems<IGridNode>> matchedItemsList = new List<MatchedItems<IGridNode>>();
    public static List<MatchedItems<IGridNode>> GetMatches(GameBoard _gameBoard, List<LineDetectors> lines)
    {
        matchedItemsList.Clear();

        foreach (var item in lines)
            CheckMatches(_gameBoard,item.lineDirections);

        return matchedItemsList;
    }
    private static void CheckMatches(GameBoard _gameBoard, GridPoint[] _lineDirections)
    {
        for (var rowIndex = 0; rowIndex < _gameBoard.RowCount; rowIndex++)
        {
            for (var columnIndex = 0; columnIndex < _gameBoard.ColumnCount; columnIndex++)
            {
                GridPoint point = new GridPoint(rowIndex, columnIndex);

                if (!CheckAllPointsOnGrid(point, _gameBoard, _lineDirections))
                    continue;

                if (CheckMatch(point, _gameBoard, _lineDirections))
                {
                    List<GridPoint> matchedGridPoint = new List<GridPoint>();

                    matchedGridPoint.Add(point);

                    foreach (var item in _lineDirections)
                        matchedGridPoint.Add(point + item);

                    AddMatchToList(new MatchedItems<IGridNode>(_gameBoard[point].Item.ItemType, matchedGridPoint));
                }
            }
        }
    }
    private static void AddMatchToList(MatchedItems<IGridNode> newMatch)
    {
        foreach (var child in matchedItemsList)
        {
            if (CheckMatchedItemOnPreviousMatch(child,newMatch))
            {
                AddItemsToPreviousMatchList(child, newMatch);
                return;
            }
        }
        matchedItemsList.Add(newMatch);
    }
    private static void AddItemsToPreviousMatchList(MatchedItems<IGridNode> currentListItem, MatchedItems<IGridNode> newMatch)
    {
        foreach (var item in newMatch.itemsList)
            if (!currentListItem.itemsList.Any(x => x.Equals(item)))
                currentListItem.itemsList.Add(item);  
    }
    private static bool CheckMatchedItemOnPreviousMatch(MatchedItems<IGridNode> currentListItem, MatchedItems<IGridNode> newMatch)
    {
        foreach (var item in newMatch.itemsList)
            if (currentListItem.itemsList.Any(x => x.Equals(item)))    
                return true;
        return false;
    }
    private static bool IsPointOnGrid(GridPoint point,GameBoard _gameBoard)
    {
        return point.RowIndex >= 0 &&
               point.RowIndex < _gameBoard.RowCount &&
               point.ColumnIndex >= 0 &&
               point.ColumnIndex < _gameBoard.ColumnCount;
            
    }
    private static bool CheckAllPointsOnGrid(GridPoint point,GameBoard _gameBoard, GridPoint[] _lineDirections)
    {
        foreach (var item in _lineDirections)
        {
            if (!IsPointOnGrid(point + item, _gameBoard) || _gameBoard[point+item].Item==null || _gameBoard[point].Item == null)
                return false;
        }
        return true;
    }
    private static bool CheckMatch(GridPoint point, GameBoard _gameBoard, GridPoint[] _lineDirections)
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
