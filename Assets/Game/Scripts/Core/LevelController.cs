using Game.Scripts.Core.Interfaces;
using Game.Scripts.Core;
using Lean.Pool;
using UnityEngine;
using PoolSystem.Core;
using Match3System.Core.Models;
using Match3System.Core.Interfaces;
using System.Linq;
using UnityEngine.Playables;

public class LevelController : MonoBehaviour
{
    [SerializeField] private SceneContext   _sceneContext;
    [SerializeField] private SpriteRenderer _gridFrame;
    [SerializeField] private Camera         _mainCamera;
    [SerializeField] private GameController _gameController;

    private float       _tileSize = 1f;
    private int         _rowCount, _columnCount;
    private Vector3     _originPosition;
    private GridNode[,] _gameBoardNodes;
    private PoolManager _poolManager;
    private EventBus    _eventBus;

    public Transform        _boardTransform;
    public LevelData        _levelData;
    public GameData         _gameData;
    public GameBoard        _gameBoard;
    public GridFiller       _gridFiller;
    public GridOperations   _gridOperarations;

    public async void Init()
    {
        _eventBus = ServiceLocator.Instance.Resolve<EventBus>();
        _poolManager = _sceneContext.Resolve<PoolManager>();

        LoadLevel();
    }
    public async void LoadLevel()
    {
        LeanPool.DespawnAll();

        _rowCount = _levelData.rowIndex;
        _columnCount = _levelData.columnIndex;

        CreateGridTiles(null);

        _gameBoard = new GameBoard();
        _gameBoard.SetGridSlots(_gameBoardNodes, GetOriginPosition(_rowCount, _columnCount), _tileSize);

        _gridOperarations = new GridOperations(_sceneContext);

        _gridFiller = new GridFiller(_sceneContext, _levelData);
        _gridFiller.GenerateToAllBoard();

        SetGridFrame();
        SetCamera();

        _eventBus.Fire(new GameEvents.OnLevelLoaded(_levelData.moveCount, _levelData.destroyItemCount));

        await _gridOperarations.ClearSequence();
    }
    public IGridNode[,] GetGameBoardNodes()
    {
        return _gameBoardNodes;
    }
    public Vector3 GetWorldPosition(int rowIndex, int columnIndex)
    {
        return new Vector3(columnIndex, -rowIndex) * _tileSize + _originPosition;
    }
    public void StartParticle(IGridNode grid)
    {
       _poolManager.GetParticle().StartParticle(_gameData.GetParticleColorFromItemType(grid.Item.PrefabId), grid.Item.GetPosition());
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
    private void SetGridFrame()
    {
        _gridFrame.size = new Vector2(_columnCount + .5f, _rowCount + .5f);
        _gridFrame.transform.position = GetCenterPoint();
    }
    private void SetCamera()
    {
        _mainCamera.orthographicSize = ((_rowCount + _columnCount) / 2f) + 1f;
        _mainCamera.transform.position = GetCenterPoint() - Vector3.forward * 10f;
    }

    private Vector3 GetOriginPosition(int rowCount, int columnCount)
    {
        var offsetY = Mathf.Floor(rowCount / 2.0f) * _tileSize;
        var offsetX = Mathf.Floor(columnCount / 2.0f) * _tileSize;

        return new Vector3(-offsetX, offsetY);
    }
    private IGridTile GetTile()
    {
        return LeanPool.Spawn(_poolManager.GetComponentFromID("tile_basic"),_boardTransform).GetComponent<IGridTile>();
    }
    private Vector3 GetCenterPoint()
    {
        var pos1 = GetWorldPosition(0, _columnCount - 1) + GetWorldPosition(_rowCount - 1, _columnCount - 1);
        var pos2 = GetWorldPosition(_rowCount - 1, 0) + GetWorldPosition(_rowCount - 1, _columnCount - 1);

        return new Vector3(pos2.x * .5f, pos1.y * .5f, 0);
    }
}
