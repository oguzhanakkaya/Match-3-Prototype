using Match3System.Core.Interfaces;
using Match3System.Core.Models;
using UnityEngine;

namespace Game.Scripts.Core
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }
        [SerializeField] private SceneContext       _sceneContext;
        [SerializeField] private InputSystem        _inputSystem;
        [SerializeField] private LevelController    levelController;

        private bool        isLevelEnded;
        private int         moveCount,destroyItemCount;
        private GridPoint   firstGridPoint;
        private EventBus    _eventBus;
        private GameBoard   _gameBoard;
        public async void Init()
        {
            Instance = this;

            _eventBus = ServiceLocator.Instance.Resolve<EventBus>();
            _eventBus.Subscribe<GameEvents.OnPointerDown>(OnPointerDown);
            _eventBus.Subscribe<GameEvents.OnPointerUp>(OnPointerUp);
            _eventBus.Subscribe<GameEvents.OnLevelLoaded>(OnLevelLoaded);
        }
        private void OnDestroy()
        {
            _eventBus.Unsubscribe<GameEvents.OnPointerDown>(OnPointerDown);
            _eventBus.Unsubscribe<GameEvents.OnPointerUp>(OnPointerUp);
        }
        private void OnLevelLoaded(GameEvents.OnLevelLoaded p)
        {
            moveCount = p.moveCount;
            destroyItemCount = p.destroyItemCount;

            _gameBoard = levelController._gameBoard;

            _eventBus.Fire(new GameEvents.OnMoveCompleted(moveCount));
            _eventBus.Fire(new GameEvents.OnItemDestroyed(destroyItemCount));

            isLevelEnded = false;
        }
        private void DecreaseMoveCount()
        {
            moveCount--;
            _eventBus.Fire(new GameEvents.OnMoveCompleted(moveCount));

            if (moveCount<=0 && !isLevelEnded)
            {
                isLevelEnded = true;
                _eventBus.Fire(new GameEvents.OnLevelFailed());
            }
        }
        public void DecreaseItemDestroyCount()
        {
            destroyItemCount--;
            _eventBus.Fire(new GameEvents.OnItemDestroyed(destroyItemCount));

            if (destroyItemCount <= 0 && !isLevelEnded)
            {
                isLevelEnded = true;
                _eventBus.Fire(new GameEvents.OnLevelCompleted());
            }
        }
        private void OnPointerDown(GameEvents.OnPointerDown pointer)
        {
            if (_gameBoard.IsPointerOnGrid(pointer.point.WorldPosition, out GridPoint point))
                firstGridPoint = point;
        }
        private async void OnPointerUp(GameEvents.OnPointerUp pointer)
        {
            if (_gameBoard.IsPointerOnGrid(pointer.point.WorldPosition, out GridPoint point))
                if (IsSameSlot(point) || !IsDiagonalSlot(point) || !HasItem(point) || isLevelEnded)
                    return;
                
                DecreaseMoveCount();
                await GridOperations.SwapItemsAsync(firstGridPoint, point, levelController._gameBoard, this,levelController);        
        }
        public void StartParticle(IGridNode grid)
        {
           // _particleGenerator.GetItem().StartParticle(_gameData.GetParticleColorFromItemType(0), grid.Item.GetPosition());
        }
        private bool IsSameSlot(GridPoint slotPosition)
        {
            return firstGridPoint.Equals(slotPosition);
        }
        private bool IsDiagonalSlot(GridPoint slotPosition)
        {
            return slotPosition.Equals(firstGridPoint + GridPoint.Up) ||
                   slotPosition.Equals(firstGridPoint + GridPoint.Down) ||
                   slotPosition.Equals(firstGridPoint + GridPoint.Left) ||
                   slotPosition.Equals(firstGridPoint + GridPoint.Right);
        }
        private bool HasItem(GridPoint slotPosition)
        {
            return _gameBoard[slotPosition].Item != null;
        }
        public void DebugWrite(string s)
        {
            Debug.LogError(s);
        }
    }
}
