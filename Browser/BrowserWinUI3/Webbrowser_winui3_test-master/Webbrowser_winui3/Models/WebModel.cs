using ABI.System;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
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
    public class WebModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private ImageSource _tabItemIcon=null;
        public ImageSource tabItemIcon
        {
            set
            {
                _tabItemIcon = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(tabItemIcon)));
            }
            get
            {
                return _tabItemIcon;
            }
        }
        private TabViewItem _tabItem;
        public TabViewItem tabItem
        {
            set
            {

                _tabItem = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(tabItem)));
            }
            get
            {
                return _tabItem;
            }
        }
       
        private string _Title = "";
        public string Title
        {
            set
            {
                _Title = value;
                if(tabItem!=null)
                {
                    tabItem.Header = value;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
            }
            get
            {
                return _Title;
            }
        }
        public string SimpleTitle
        {
            set
            {
            }
            get
            {
                return _Title.Length>=7? _Title.Substring(0,7)+"...": _Title;
            }
        }
        private string _Url = "";
        public string Url
        {
            set
            {
                _Url = value;
                try
                {
                    if (tabItem != null)
                    {
                        tabItem.Header = value;
                        tabItem.IconSource = new ImageIconSource() { ImageSource = tabItemIcon };
                    }
                }
                catch { }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Url)));
            }
            get
            {
                return _Url;
            }
        }
        private string _Date = "";
        public string Date
        {
            set
            {
                _Date = value;
                try
                {
                    if (tabItem != null)
                    {
                        tabItem.Header = value;
                        tabItem.IconSource = new ImageIconSource() { ImageSource = tabItemIcon };
                    }
                }
                catch { }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Date)));
            }
            get
            {
                return _Date;
            }
        }
    }
}
