using Game.Scripts.Core;
using Game.Scripts.Core.Interfaces;
using Game.Scripts.Core.Models;
using Game.Scripts.Items;
using Game.Scripts.UI.Interfaces;
using PoolSystem;
using PoolSystem.Core;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class GameView : MonoBehaviour, IGameView
    {
        [SerializeField]private SceneContext _sceneContext;
        [SerializeField]private TextMeshProUGUI moveCountText, destroyedItemCountText;
        [SerializeField]private GameObject levelFailedUI, levelCompletedUI;
        [SerializeField]private Button retryLevelButton, nextLevelButton;

        private GameController gameController;
        public void Init()
        {
            gameController = _sceneContext.Resolve<GameController>();

            gameController.ItemDestroy += ItemDestroy;
            gameController.MoveComplete += MoveComplete;
            gameController.OnLevelCompleted += OnLevelCompleted;
            gameController.OnLevelFailed += OnLevelFailed;

            retryLevelButton.onClick.AddListener(NextLevelButtonPressed);
            nextLevelButton.onClick.AddListener(RetryButtonPressed);
        }
        private void NextLevelButtonPressed()
        {
            levelFailedUI.gameObject.SetActive(false);
            gameController.LoadLevel();
        }
        private void RetryButtonPressed()
        {
            levelCompletedUI.gameObject.SetActive(false);
            gameController.LoadLevel();
        }
        private void OnLevelFailed(object sender, bool e)
        {
            levelFailedUI.gameObject.SetActive(true);
        }
        private void OnLevelCompleted(object sender, bool e)
        {
            levelCompletedUI.gameObject.SetActive(true);
        }
        private void MoveComplete(object sender, int e)
        {
            moveCountText.text = e.ToString();
        }
        private void ItemDestroy(object sender, int e)
        {
            destroyedItemCountText.text = e.ToString();
        }
    }
}
