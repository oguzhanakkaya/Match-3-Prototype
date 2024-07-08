using Game.Scripts.Core.Interfaces;
using PoolSystem;
using UnityEngine;
using Random = System.Random;

namespace Game.Scripts.Core
{
    public class NodeItemGenerator : ItemGenerator<Item>, INodeItemGenerator
    {
        private readonly Random _random;

        GameData gameData;

        protected override IItem ConfigureItem<T1>(Item item,LevelData levelData)
        {
            var index = _random.Next(0, levelData.levelItems.Count);
            item.SetItem(GetLevelItemSprite(levelData, index),0);

            return item;
        }

        protected override IItem ConfigureItem<T1>(Item item,ItemType itemType)
        {
             item.SetItem(gameData.GetSprite(itemType),0);

             return item;
          //  return null;
        }

        public void SetGameData(GameData gameData)
        {
            this.gameData = gameData;
        }

        private Sprite GetLevelItemSprite(LevelData levelData,int i)
        {
            return null;
          //  return gameData.GetSprite(levelData.levelItems[i]);
        }

        public NodeItemGenerator(IPool<Item> pool) : base(pool)
        {
            _random = new Random();
        }
    }
}
