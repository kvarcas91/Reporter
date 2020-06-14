using Prism.Ioc;
using Prism.Modularity;
using Reporter_V2.Views;

namespace Reporter_V2
{
    public class Shell : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Dashboard>();
           
        }
    }
}
