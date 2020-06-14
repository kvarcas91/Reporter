using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporter_V2.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "This is Title";
        public string Title
        {
            get => _title;
            set { SetProperty(ref _title, value); }
        }
    }
}
