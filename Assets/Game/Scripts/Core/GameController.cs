using System;
using Cysharp.Threading.Tasks;
using Game.Scripts.Core.Interfaces;
using Match3System.Core.Interfaces;
using Match3System.Core.Models;
using UnityEngine;
using TMPro;
using Game.Scripts.UI;
using Lean.Pool;
using PoolSystem.Core;

namespace Game.Scripts.Core
{
    public class GameController : MonoBehaviour, IGameBoardDataProvider<IGridNode>
    {
        [SerializeField] private SceneContext _sceneContext;
        [SerializeField] private InputSystem _inputSystem;
        [SerializeField] private SpriteRenderer gridFrame;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private TextMeshProUGUI moveCountText;

        private bool isLevelEnded;
        private float _tileSize = 1f;
        private int moveCount,destroyItemCount;
        private int _rowCount, _columnCount;
        private Vector3 _originPosition;
        private GridPoint firstGridPoint;
        private GridNode[,] _gameBoardNodes;
        private PoolManager _poolManager;
        private EventBus _eventBus;

        public LevelData levelData;
        public GameBoard _gameBoard;
        public GridFiller _gridFiller;

        public async void Init()
        {
            _eventBus = ServiceLocator.Instance.Resolve<EventBus>();

            _eventBus.Subscribe<GameEvents.OnPointerDown>(OnPointerDown);
            _eventBus.Subscribe<GameEvents.OnPointerUp>(OnPointerUp);

            _poolManager = _sceneContext.Resolve<PoolManager>();

            LoadLevel();
        }
        private void OnDestroy()
        {
            _eventBus.Unsubscribe<GameEvents.OnPointerDown>(OnPointerDown);
            _eventBus.Unsubscribe<GameEvents.OnPointerUp>(OnPointerUp);
        }
        public IGridNode[,] GetGameBoardNodes()
        {
            return _gameBoardNodes;
        }
        private void CreateGridTiles(int[,] data)
        {
            _gameBoardNodes = new GridNode[_rowCount, _columnCount];
            _originPosition = GetOriginPosition(_rowCount, _columnCount);

            CreateGridTiles();
        }
        private void CreateGridTiles()
        {
            for (var rowIndex = 0; rowIndex < _rowCount; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < _columnCount; columnIndex++)
                {
                    var gridTile = GetTile();
                    
                    gridTile.SetWorldPosition(GetWorldPosition(rowIndex, columnIndex));

                    var gridNode = new GridNode(new GridPoint(rowIndex, columnIndex));

                    _gameBoardNodes[rowIndex, columnIndex] = gridNode;
                }
            }
        }
        public Vector3 GetWorldPosition(int rowIndex, int columnIndex)
        {
            return new Vector3(columnIndex, -rowIndex) * _tileSize + _originPosition;
        }
        private Vector3 GetOriginPosition(int rowCount, int columnCount)
        {
            var offsetY = Mathf.Floor(rowCount / 2.0f) * _tileSize;
            var offsetX = Mathf.Floor(columnCount / 2.0f) * _tileSize;

            return new Vector3(-offsetX, offsetY);
        }
        private IGridTile GetTile()
        {
            return LeanPool.Spawn(_poolManager.GetComponentFromID("tile_basic")).GetComponent<IGridTile>();
        }
        private void SetGridFrame()
        {
            gridFrame.size = new Vector2(_columnCount + .5f, _rowCount + .5f);
            gridFrame.transform.position = GetCenterPoint();
        }
        private void SetCamera()
        {
            mainCamera.orthographicSize = ((_rowCount + _columnCount) / 2f) + 1f;
            mainCamera.transform.position = GetCenterPoint() - Vector3.forward * 10f;
        }
        private Vector3 GetCenterPoint()
        {
            var pos1 = GetWorldPosition(0, _columnCount - 1) + GetWorldPosition(_rowCount - 1, _columnCount - 1);
            var pos2 = GetWorldPosition(_rowCount - 1, 0) + GetWorldPosition(_rowCount - 1, _columnCount - 1);

            return new Vector3(pos2.x * .5f, pos1.y * .5f, 0);
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
            {
                firstGridPoint = point;
            }
        }
        private async void OnPointerUp(GameEvents.OnPointerUp pointer)
        {
            if (_gameBoard.IsPointerOnGrid(pointer.point.WorldPosition, out GridPoint point))
            {
                if (IsSameSlot(point) || !IsDiagonalSlot(point) || !HasItem(point) || isLevelEnded)
                    return;
                
                DecreaseMoveCount();
                await GridOperations.SwapItemsAsync(firstGridPoint, point, _gameBoard,this);        
            }
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

        public async void LoadLevel()
        {
            LeanPool.DespawnAll();

            _rowCount = levelData.rowIndex;
            _columnCount = levelData.columnIndex;
            moveCount = levelData.moveCount;
            destroyItemCount = levelData.destroyItemCount;

            CreateGridTiles(null);

            _gameBoard = new GameBoard();
            _gameBoard.SetGridSlots(_gameBoardNodes, GetOriginPosition(_rowCount, _columnCount), _tileSize);

            _gridFiller = new GridFiller(_sceneContext,levelData);
            _gridFiller.GenerateToAllBoard();

            SetGridFrame();
            SetCamera();

            _eventBus.Fire(new GameEvents.OnMoveCompleted(moveCount));
            _eventBus.Fire(new GameEvents.OnItemDestroyed(destroyItemCount));

            isLevelEnded = false;

            await GridOperations.ClearSequence(_gameBoard, this);
        }
    }
}
