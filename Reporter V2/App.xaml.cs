using Prism.Ioc;
using Prism.Unity;
using System.Windows;
using Prism.Modularity;

namespace Reporter_V2
{

    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<Shell>();
        }
    }
}
