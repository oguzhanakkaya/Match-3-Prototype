using System;
using Cysharp.Threading.Tasks;
using Game.Scripts.Core.Interfaces;
using Game.Scripts.Core.Models;
using Game.Scripts.Tiles;
using Match3System.Core.Interfaces;
using Match3System.Core.Models;
using PoolSystem;
using UnityEngine;
using TMPro;
using Game.Scripts.UI;

namespace Game.Scripts.Core
{
    public class GameController : MonoBehaviour, IGameBoardDataProvider<IGridNode>
    {
        [SerializeField] private SceneContext _sceneContext;
        [SerializeField] private InputSystem _inputSystem;
        [SerializeField] private SpriteRenderer gridFrame;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private TextMeshProUGUI moveCountText;
        [SerializeField] private GameView _gameView;

        private bool isLevelEnded;
        private float _tileSize = 1f;
        private int moveCount,destroyItemCount;
        private int _rowCount, _columnCount;
        private Vector3 _originPosition;
        private GridNode[,] _gameBoardNodes;
        private IPoolManager _poolManager;
        private NodeItemGenerator _itemGenerator;
        private ParticleGenerator _particleGenerator;
        private IPool<BasicTile> _tilePool;
        private GridPoint firstGridPoint;
        private GameData _gameData;

        public FillClass fillClass;
        public LevelData levelData;
        public GameBoard _gameBoard;

        public event EventHandler<int> MoveComplete;
        public event EventHandler<int> ItemDestroy;
        public event EventHandler<bool> OnLevelFailed;
        public event EventHandler<bool> OnLevelCompleted;

        private void Awake()
        {
            _inputSystem.PointerUp += OnPointerUp;
            _inputSystem.PointerDown += OnPointerDown;
        }
        private void OnDestroy()
        {
            _inputSystem.PointerUp -= OnPointerUp;
            _inputSystem.PointerDown -= OnPointerDown;
        }
        public async void Init()
        {
            _poolManager = _sceneContext.Resolve<IPoolManager>();
            _itemGenerator = _sceneContext.Resolve<NodeItemGenerator>();
            _particleGenerator = _sceneContext.Resolve<ParticleGenerator>();
            _gameData = _sceneContext.GetGameData();

            _tilePool = _poolManager.GetPool<BasicTile>("tile_basic");

            _gameView.Init();

            LoadLevel();
        }
        public IGridNode[,] GetGameBoardNodes(int level)
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
            return _tilePool.Spawn();
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
            MoveComplete?.Invoke(this,moveCount);

            if (moveCount<=0 && !isLevelEnded)
            {
                isLevelEnded = true;
                OnLevelFailed?.Invoke(this,true);
            }
        }
        public void DecreaseItemDestroyCount()
        {
            destroyItemCount--;
            ItemDestroy?.Invoke(this, destroyItemCount);

            if (destroyItemCount <= 0 && !isLevelEnded)
            {
                isLevelEnded = true;
                OnLevelCompleted?.Invoke(this, true);
            }
        }
        private async void OnPointerDown(object sender, PointerEventArgs e)
        {
            if (_gameBoard.IsPointerOnGrid(e.WorldPosition, out GridPoint point))
            {
                firstGridPoint = point;
            }
        }
        private async void OnPointerUp(object sender, PointerEventArgs e)
        {
            if (_gameBoard.IsPointerOnGrid(e.WorldPosition, out GridPoint point))
            {
                if (IsSameSlot(point) || !IsDiagonalSlot(point) || !HasItem(point) || isLevelEnded)
                    return;
                
                DecreaseMoveCount();
                await GridOperations.SwapItemsAsync(firstGridPoint, point, _gameBoard,this);        
            }
        }
        public void StartParticle(IGridNode grid)
        {
            _particleGenerator.GetItem().StartParticle(_gameData.GetParticleColorFromItemType(grid.Item.ItemType), grid.Item.GetPosition());
        }
        public async UniTask FillSequence()
        {
            await ItemFallDown.FallDown(_gameBoard, this, .1f);
            await fillClass.Fill(_gameBoard, 1, levelData);
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
            _tilePool.RecycleAll();
            _poolManager.GetPool<Item>("item_basic").RecycleAll();

            _rowCount = levelData.rowIndex;
            _columnCount = levelData.columnIndex;

            CreateGridTiles(null);

            var gameBoardData = GetGameBoardNodes(0);
            var rowCount = gameBoardData.GetLength(0);
            var columnCount = gameBoardData.GetLength(1);
            var itemsPoolCapacity = rowCount * columnCount + Mathf.Max(rowCount, columnCount) * 2;

            _itemGenerator.CreateItems(itemsPoolCapacity);
            _particleGenerator.CreateItems(itemsPoolCapacity);

            _gameBoard = new GameBoard();
            _gameBoard.SetGridSlots(_gameBoardNodes, GetOriginPosition(rowCount, columnCount), _tileSize);

            fillClass = new FillClass(_sceneContext);
            fillClass.FillInstantly(_gameBoard, levelData);

            SetGridFrame();
            SetCamera();

            moveCount = levelData.moveCount;
            destroyItemCount = levelData.destroyItemCount;

            MoveComplete?.Invoke(this, moveCount);
            ItemDestroy?.Invoke(this, destroyItemCount);

            isLevelEnded = false;

            await GridOperations.ClearSequence(_gameBoard, this);
        }
    }
}
