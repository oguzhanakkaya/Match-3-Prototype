using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Scripts.Core.Interfaces;
using Game.Scripts.Core.Models;
using Game.Scripts.Tiles;
using Match3System.Core.Interfaces;
using Match3System.Core.Models;
using PoolSystem;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using TMPro;
using static UnityEditor.Progress;
using System.Drawing;

namespace Game.Scripts.Core
{
    public class GameController : MonoBehaviour, IGameBoardDataProvider<IGridNode>
    {
        [SerializeField] private SceneContext _sceneContext;
        [SerializeField] private InputSystem _inputSystem;
        [SerializeField] private SpriteRenderer gridFrame;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private TextMeshProUGUI moveCountText;


        private float _tileSize = 1f;

        private int moveCount;

        private int _rowCount, _columnCount;

        private Vector3 _originPosition;

        private GridNode[,] _gameBoardNodes;

        private IPoolManager _poolManager;
        private NodeItemGenerator _itemGenerator;
        private ParticleGenerator _particleGenerator;
        private IPool<BasicTile> _tilePool;

        public GameBoard _gameBoard;
        private GameData _gameData;

        public FillClass fillClass;

        public LevelData levelData;

        private GridPoint firstGridPoint;
        private bool canDrag;

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
            SceneContext _sceneContext1 = _sceneContext;
            _poolManager = _sceneContext1.Resolve<IPoolManager>();
            _itemGenerator = _sceneContext1.Resolve<NodeItemGenerator>();
            _particleGenerator = _sceneContext1.Resolve<ParticleGenerator>();
            _tilePool = _poolManager.GetPool<BasicTile>("tile_basic");
            _gameData = _sceneContext1.GetGameData();

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
            _gameBoard.SetGridSlots(_gameBoardNodes, GetOriginPosition(rowCount,columnCount),_tileSize);

            fillClass = new FillClass(_sceneContext1);
            fillClass.FillInstantly(_gameBoard,levelData);

            SetGridFrame();
            SetCamera();

            moveCount = levelData.moveCount;
            SetMoveCountText();

            await GridOperations.ClearSequence(_gameBoard,this);
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
        private void SetMoveCountText()
        {
            moveCountText.text = moveCount.ToString();
        }

        private void DecreaseMoveCount()
        {
            moveCount--;
            SetMoveCountText();
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
                if (IsSameSlot(point) || !IsDiagonalSlot(point) || !HasItem(point))
                    return;
                
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



    }
}
