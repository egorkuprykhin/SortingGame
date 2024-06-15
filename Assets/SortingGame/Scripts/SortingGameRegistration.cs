using Infrastructure.Core;

namespace Infrastructure.SortingGame
{
    public class SortingGameRegistration : RegistrationBase
    {
        protected override void RegisterServices(IRegistrar registrar)
        {
            registrar.Register<SortingGameStarter>();
            registrar.Register<SortingGameFinisher>();
        }
    }
}