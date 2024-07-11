using Cysharp.Threading.Tasks;
using Match3System.Core.Models;
using System;
using UnityEngine;

namespace Game.Scripts.Core
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private SceneContext       _sceneContext;
        [SerializeField] private InputSystem        _inputSystem;
        [SerializeField] private LevelController    levelController;

        private bool        isLevelEnded;
        private int         _moveCount;
        private int         _destroyItemCount;
        private int         _targetDestroyItemCount;
        private GridPoint   firstGridPoint;
        private EventBus    _eventBus;
        private GameBoard   _gameBoard;

        public bool isSequenceEnded;
        public async void Init()
        {
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
            _moveCount = p.moveCount;
            _destroyItemCount = 0;
            _targetDestroyItemCount = p.targetDestroyItemCount;

            _gameBoard = levelController._gameBoard;

            isLevelEnded = false;
        }
        private void DecreaseMoveCount()
        {
            _moveCount--;
            _eventBus.Fire(new GameEvents.OnMoveCompleted(_moveCount));

            if (_moveCount <= 0 && !isLevelEnded)
            {
                isLevelEnded = true;
                _eventBus.Fire(new GameEvents.OnLevelFailed());
            }
        }
        public async void DecreaseItemDestroyCount()
        {
            _destroyItemCount++;
            _eventBus.Fire(new GameEvents.OnItemDestroyed(_destroyItemCount));

            if (_destroyItemCount >= _targetDestroyItemCount && !isLevelEnded)
            {
                isLevelEnded = true;
                await FireLevelFailedEvent();

            }
        }
        private async UniTask FireLevelFailedEvent()
        {
            await UniTask.WaitUntil(() => isSequenceEnded == true);
            await UniTask.Delay(TimeSpan.FromSeconds(.5f));
            _eventBus.Fire(new GameEvents.OnLevelCompleted());

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
                await levelController._gridOperarations.SwapItemsAsync(firstGridPoint, point);        
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
    }
}
