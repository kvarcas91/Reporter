using DataProcessor;
using Domain.Models;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Reporter_V2.ViewModels
{
    public class DashboardViewModel : BindableBase, INavigationAware
    {
        private string _rosterInformation = string.Empty;
        private string _dataInformation = string.Empty;
        private bool _isRosterImported = false;
        private bool _isRosterWithErrors = true;
        private bool _isSnackBarActive = false;
        private bool _isDataLoading = false;
        private bool _isDataImported = false;
        private bool _isDataWithErrors = true;

        private string _snackBarMessage = string.Empty;
        private string _errorMessage = "Roster is not imported";
        private string _dataErrorMessage = "Data is not imported";

        private ObservableCollection<ReportData> _reportData;
        private ObservableCollection<string[]> _reportDataErrors;

        private ObservableCollection<RosterData> _roster;
        private ObservableCollection<string[]> _rosterErrors;

        public string RosterInformation
        {
            get => _rosterInformation;
            set { SetProperty(ref _rosterInformation, value); }
        }

        public string DataInformation
        {
            get => _dataInformation;
            set { SetProperty(ref _dataInformation, value); }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set { SetProperty(ref _errorMessage, value); }
        }

        public string DataErrorMessage
        {
            get => _dataErrorMessage;
            set { SetProperty(ref _dataErrorMessage, value); }
        }

        public string SnackBarMessage
        {
            get => _snackBarMessage;
            set { SetProperty(ref _snackBarMessage, value); }
        }

        public bool IsRosterImported
        {
            get => _isRosterImported;
            set { SetProperty(ref _isRosterImported, value); }
        }

        public bool IsDataLoading
        {
            get => _isDataLoading;
            set { SetProperty(ref _isDataLoading, value); }
        }

        public bool IsDataImported
        {
            get => _isDataImported;
            set { SetProperty(ref _isDataImported, value); }
        }

        public bool IsSnackBarActive
        {
            get => _isSnackBarActive;
            set { SetProperty(ref _isSnackBarActive, value); }
        }

        public bool IsRosterWithErrors
        {
            get => _isRosterWithErrors;
            set { SetProperty(ref _isRosterWithErrors, value); }
        }

        public bool IsDataWithErrors
        {
            get => _isDataWithErrors;
            set { SetProperty(ref _isDataWithErrors, value); }
        }

        public DelegateCommand CopyDataCommand { get; private set; }
        public DelegateCommand ImportDataCommand { get; private set; }
        public DelegateCommand ExportDataCommand { get; private set; }

        public DashboardViewModel()
        {
            CopyDataCommand = new DelegateCommand(Copy);
            ImportDataCommand = new DelegateCommand(Import);
            ExportDataCommand = new DelegateCommand(Export);
        }

        private void Import ()
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

            IsDataLoading = true;
            Task.Run(() =>
            {
                (var reportData, var reportErrors, var reportHeaders) = ExtractData.GetData<ReportData>(path);
                _reportData = new ObservableCollection<ReportData>(reportData);
                _reportDataErrors = new ObservableCollection<string[]>(reportErrors);
                IsDataLoading = false;
                IsDataImported = true;
                DataInformation = $"{Path.GetFileName(path)}\n{_reportData.Count} columns";
                IsDataWithErrors = _reportDataErrors.Count > 0 ? true : false;
            });
          
        }

        private void MessageSetter (string message)
        {
            SnackBarMessage = message;
        }

        private void Export ()
        {
            if (!IsRosterImported || !IsDataImported)
            {
                ShowSnackBar("Roster or data is not imported");
                return;
            }

            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "CSV Files|*.csv",
                Title = "Save a dashboard file"
            };



            if (dialog.ShowDialog() == true)
            {
                IsDataLoading = true;
                var fileName = dialog.FileName;
                var response = WriteData.Write(fileName, MessageSetter, _reportData, _roster);
                IsDataLoading = false;
                SnackBarMessage = string.Empty;
            }

        }

        private void Copy ()
        {
            IsDataLoading = true;
            if (!IsRosterImported) return;
            var dataString = new StringBuilder();
            foreach (var item in _roster)
            {
                dataString.Append(item);
            }
            var data = new DataObject(DataFormats.UnicodeText, dataString.ToString(), true);
            Clipboard.SetDataObject(data);
            ShowSnackBar("copied to clipboard");
        }

        private Task ShowSnackBar (string message)
        {
            IsSnackBarActive = true;
            SnackBarMessage = message;
            return Task.Run(() =>
            {
                Task.Delay(3000).Wait();
                IsSnackBarActive = false;
                SnackBarMessage = string.Empty;
            }); 
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            var test = navigationContext.Parameters["roster"] as ObservableCollection<RosterData>;
            if (test != null)
            {
                return true;
            }
            else return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
           
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _roster = navigationContext.Parameters["roster"] as ObservableCollection<RosterData>;
            if (_roster != null)
            {
                _rosterErrors = navigationContext.Parameters["rosterErrors"] as ObservableCollection<string[]>;
                RosterInformation = $"{navigationContext.Parameters["rosterFileName"] as string}\n{_roster.Count} columns";
                IsRosterImported = true;

                if (_rosterErrors.Count > 0)
                {
                    IsRosterWithErrors = false;
                    try
                    {
                        var errorMessage = _rosterErrors[0][0];
                        ErrorMessage = string.IsNullOrEmpty(_rosterErrors[0][0]) ? "Roster has an unknown error" : _rosterErrors[0][0];
                    }
                    catch
                    {
                        ErrorMessage = "Roster has an unknown error";
                    }
                }
                else
                {
                    ErrorMessage = string.Empty;
                    IsRosterWithErrors = false;
                }
            }
        }
    }
}
