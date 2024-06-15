using Infrastructure.Services;

namespace Infrastructure.SortingGame
{
    public class SortingGameStarter : IInitializableService
    {
        private SortingGameField _gameField;
        private GameStarterService _gameStarter;

        public void Initialize()
        {
            _gameField = ServiceLocator.GetService<SortingGameField>();
            _gameStarter = ServiceLocator.GetService<GameStarterService>();
        }

        public void StartGame()
        {
            _gameField.Clear();
            _gameField.Create();
            
            _gameStarter.StartGame();
        }
    }
}