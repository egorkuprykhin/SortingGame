using Infrastructure.ButtonActions;
using Infrastructure.Services;

namespace Infrastructure.SortingGame
{
    public class StartSortingGameGameButtonAction : ButtonAction
    {
        private ScreensService _screensService;
        private SortingGameStarter _gameStarter;

        protected override void Initialize()
        {
            _screensService = ServiceLocator.GetService<ScreensService>();
            _gameStarter = ServiceLocator.GetService<SortingGameStarter>();
        }

        public override void Action()
        {
            _screensService.HideCurrentScreen();
            _gameStarter.StartGame();
        }
    }
}