using Prism.Ioc;
using Prism.Regions;
using System.Windows;
using System.Windows.Controls;

namespace Reporter_V2.Views
{
    /// <summary>
    /// Interaction logic for SideBar.xaml
    /// </summary>
    public partial class SideBar : UserControl
    {
        readonly IContainerExtension _container;
        readonly IRegionManager _regionManager;
        IRegion _region;
        Dashboard _dashboard;

        public SideBar(IContainerExtension container, IRegionManager regionManager)
        {
            InitializeComponent();

            _container = container;
            _regionManager = regionManager;

            Loaded += SideBar_Loaded;
        }

        private void SideBar_Loaded(object sender, RoutedEventArgs e)
        {
            _dashboard = _container.Resolve<Dashboard>();
            _region = _regionManager.Regions["ContentRegion"];
            _region.Add(_dashboard);
        }
    }
}
