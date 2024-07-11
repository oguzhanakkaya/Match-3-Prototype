using Game.Scripts.Core;
using Game.Scripts.UI.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameEvents;

namespace Game.Scripts.UI
{
    public class GameView : MonoBehaviour, IGameView
    {
        [SerializeField]private SceneContext    _sceneContext;
        [SerializeField]private TextMeshProUGUI moveCountText, destroyedItemCountText,targetItemText;
        [SerializeField]private GameObject      levelFailedUI, levelCompletedUI;
        [SerializeField]private Button          retryLevelButton, nextLevelButton;

        private LevelController levelController;
        private EventBus        _eventBus;
        public void Init()
        {
            levelController = _sceneContext.Resolve<LevelController>();

            _eventBus = ServiceLocator.Instance.Resolve<EventBus>();
            _eventBus.Subscribe<GameEvents.OnItemDestroyed>(OnItemDestroy);
            _eventBus.Subscribe<GameEvents.OnLevelLoaded>(OnLevelLoaded);
            _eventBus.Subscribe<GameEvents.OnMoveCompleted>(OnMoveComplete);
            _eventBus.Subscribe<GameEvents.OnLevelFailed>(OnLevelFailed);
            _eventBus.Subscribe<GameEvents.OnLevelCompleted>(OnLevelCompleted);

            retryLevelButton.onClick.AddListener(NextLevelButtonPressed);
            nextLevelButton.onClick.AddListener(RetryButtonPressed);
        }
        private void OnDestroy()
        {
            _eventBus.Unsubscribe<GameEvents.OnItemDestroyed>(OnItemDestroy);
            _eventBus.Unsubscribe<GameEvents.OnMoveCompleted>(OnMoveComplete);
            _eventBus.Unsubscribe<GameEvents.OnLevelFailed>(OnLevelFailed);
            _eventBus.Unsubscribe<GameEvents.OnLevelCompleted>(OnLevelCompleted);
            _eventBus.Unsubscribe<GameEvents.OnLevelLoaded>(OnLevelLoaded);
        }
        private void NextLevelButtonPressed()
        {
            levelFailedUI.gameObject.SetActive(false);
              levelController.LoadLevel();
           // SceneManager.LoadScene(0);
        }
        private void RetryButtonPressed()
        {
            levelCompletedUI.gameObject.SetActive(false);
            //SceneManager.LoadScene(0);
              levelController.LoadLevel();
        }
        private void OnLevelFailed()
        {
            levelFailedUI.gameObject.SetActive(true);
        }
        private void OnLevelCompleted()
        {
            levelCompletedUI.gameObject.SetActive(true);
        }
        private void OnMoveComplete(GameEvents.OnMoveCompleted onMoveCompleted)
        {
            moveCountText.text = onMoveCompleted.moveCount.ToString();
        }
        private void OnItemDestroy(GameEvents.OnItemDestroyed onItemDestroyed)
        {
            destroyedItemCountText.text = onItemDestroyed.remainingItemCount.ToString();
        }
        private void OnLevelLoaded(GameEvents.OnLevelLoaded onLevelLoaded) 
        {
            moveCountText.text = onLevelLoaded.moveCount.ToString();
            targetItemText.text = onLevelLoaded.targetDestroyItemCount.ToString();
            destroyedItemCountText.text = "0";
        }
    }
}
