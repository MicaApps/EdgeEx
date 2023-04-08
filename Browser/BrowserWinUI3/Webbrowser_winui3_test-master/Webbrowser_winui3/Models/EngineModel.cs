using ABI.System;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Webbrowser_winui3.Models
{
    public class EngineModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
       
       
        private string _Title = "";
        public string Title
        {
            set
            {
                _Title = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
            }
            get
            {
                return _Title;
            }
        }
        private string _Url = "";
        public string Url
        {
            set
            {
                _Url = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Url)));
            }
            get
            {
                return _Url;
            }
        }
       
    }
}
