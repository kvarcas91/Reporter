using MaterialDesignExtensions.Controls;
using Prism.Ioc;
using Prism.Regions;
using Reporter_V2.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Reporter_V2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MaterialWindow
    {

        readonly IContainerExtension _container;
        readonly IRegionManager _regionManager;
        private IRegion _region;

        SideBar _sidebarView;

        public MainWindow(IContainerExtension container, IRegionManager regionManager)
        {
            InitializeComponent();
            _container = container;
            _regionManager = regionManager;

            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _sidebarView = _container.Resolve<SideBar>();
            _region = _regionManager.Regions["SideBarRegion"];
            _region.Add(_sidebarView);
        }
    }
}
