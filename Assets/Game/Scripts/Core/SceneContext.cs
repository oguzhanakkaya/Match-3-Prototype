using Game.Scripts.Core.Interfaces;
using Game.Scripts.Items;
using Game.Scripts.UI;
using Game.Scripts.UI.Interfaces;
using Match3System.Core.Interfaces;
using PoolSystem;
using PoolSystem.Core;
using UnityEngine;

namespace Game.Scripts.Core
{
    public class SceneContext : BaseContext
    {
        [SerializeField] private GameView _gameView;
        [SerializeField] private InputSystem _inputSystem;
        [SerializeField] private PoolManager _poolManager;
        [SerializeField] private GameController _gameController;
        [SerializeField] private GameData gameData;

        public override void Initialize()
        {
            base.Initialize();
            
            Register<IGameView>(_gameView);
            Register<IInputSystem>(_inputSystem);
          //  Register<AudioManager>(_audioManager);

            InitPooling();
            
            CreateGenerator();
            CreateParticleGenerator();
            CreateRocketGenerator();
            
            Register(GetGameSession());
            Register(_gameController);
            Register<IGameBoardDataProvider<IGridNode>>(_gameController);
            
            _gameController.Init();
        }

        private void InitPooling()
        {
            _poolManager.Init();
            Register<IPoolManager>(_poolManager);
        }

        private GameSession GetGameSession()
        {
            return new GameSession(this);
        }
        public GameController GetGameController()
        {
            return _gameController;
        }
        public GameData GetGameData()
        {
            return gameData;
        }

        private void CreateGenerator()
        {
            var itemPool = _poolManager.GetPool<Item>("item_basic");
            var nodeItemGenerator = new NodeItemGenerator(itemPool);
            nodeItemGenerator.SetGameData(gameData);
            Register<NodeItemGenerator>(nodeItemGenerator);
        }
        private void CreateRocketGenerator()
        {
          /*  var itemPool = _poolManager.GetPool<RocketItem>("item_rocket");
            var rocketItemGenerator = new RocketItemGenerator(itemPool);
            Register<RocketItemGenerator>(rocketItemGenerator);*/
        }
        private void CreateParticleGenerator()
        {
            var itemPool = _poolManager.GetPool<ParticleObject>("particle_object");
            var particleGenerator = new ParticleGenerator(itemPool);
            Register<ParticleGenerator>(particleGenerator);
        }
    }
}
