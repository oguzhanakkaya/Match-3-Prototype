using Game.Scripts.Core;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Data/LevelData")]
public class LevelData : ScriptableObject
{
    public int rowIndex, columnIndex;
    public int moveCount;
    public int destroyItemCount;
    public List<ItemBase> levelItems;
    public List<bool> spawners=new List<bool>();
    private void OnValidate()
    {
        int count = spawners.Count;

        if (columnIndex > count)
            for (int i = 0; i < columnIndex - count; i++)
                spawners.Add(true);
        else
            for (int i = 0; i < count - columnIndex; i++)
                spawners.RemoveAt(spawners.Count - 1);

    }
}