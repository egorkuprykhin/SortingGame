using System.Linq;
using Infrastructure.Services;

namespace Infrastructure.SortingGame
{
    public class SortingGameFinisher : IInitializableService
    {
        private SortingGameField _gameField;
        private GameFinisherService _gameFinisher;
        
        public void Initialize()
        {
            _gameField = ServiceLocator.GetService<SortingGameField>();
            _gameFinisher = ServiceLocator.GetService<GameFinisherService>();
        }

        public void TryFinishGame()
        {
            if (GameIsOver())
                _gameFinisher.FinishGame();
        }

        private bool GameIsOver()
        {
            return _gameField.Groups.All(group => group.IsEmpty() ||
                                                  group.AllElementsAreSame());
        }
    }
}