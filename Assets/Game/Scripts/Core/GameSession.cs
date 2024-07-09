using System;
using Game.Scripts.Core.Interfaces;
using Game.Scripts.UI.Interfaces;

namespace Game.Scripts.Core
{
    public class GameSession : IDisposable
    {
        private IContext _context;
        private IGameView _gameView;
        public GameSession(IContext context)
        {
            _context = context;
            _gameView = context.Resolve<IGameView>();
        }
        
        public void Init()
        {
        }
        public void Dispose()
        {
        }
    }
}
