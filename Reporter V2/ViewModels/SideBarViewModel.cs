using DataProcessor;
using Domain.Models;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace Reporter_V2.ViewModels
{
    public class SideBarViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private ObservableCollection<RosterData> _rosterData;
        private ObservableCollection<string[]> _errors;
        private string[] _headers;
        private string _rosterFileName = string.Empty;

        public DelegateCommand<string> NavigateCommand { get; private set; }
        public DelegateCommand ImportRoster { get; private set; }

        public SideBarViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommand = new DelegateCommand<string>(Navigate);
            ImportRoster = new DelegateCommand(Import);
        }

        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
            {
                var parameters = new NavigationParameters
                {
                    { "roster", _rosterData },
                    { "rosterHeaders", _headers },
                    { "rosterErrors", _errors },
                    { "rosterFileName", _rosterFileName }
                };
                _regionManager.RequestNavigate("ContentRegion", navigatePath, parameters);
            }
        }

        private void Import()
        {
          
            var dialog = new OpenFileDialog
            {
                DefaultExt = ".csv"
            };

            dialog.Filter = "CSV Files|*.csv";

            bool? result = dialog.ShowDialog();

            if (result != null && !(bool)result) return;

            var path = dialog.FileName;
            if (string.IsNullOrEmpty(path)) return;

            _rosterFileName = Path.GetFileName(path);
           (var roster, var errors, var headers) = ExtractData.GetData<RosterData>(path);
            _rosterData = new ObservableCollection<RosterData>(roster);
            _errors = new ObservableCollection<string[]>(errors);
            _headers = headers;

            Navigate("Dashboard");
        }


    }
}
