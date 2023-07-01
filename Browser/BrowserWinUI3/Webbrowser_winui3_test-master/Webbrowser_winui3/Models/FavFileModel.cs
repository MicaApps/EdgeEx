using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webbrowser_winui3.Models
{
    internal class FavFileModel: ModelBase
    {
        private string? _favName;
        private Guid? _id;
        private string? _url;
        private ObservableCollection<FavFileModel> _favChildren;
        private FavFileModel _parent;

        public string? FavName
        {
            get { return _favName; }
            set
            {
                _favName = value;
                OnPropertyChanged();
            }
        }

        public Guid? Id {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        public string? Url { 
            get { return _url; }
            set
            {
                _url = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<FavFileModel> FavChildren 
        { 
            get { return _favChildren; }
            set
            {
                _favChildren = value;
                OnPropertyChanged();
            }
        } 

        public FavFileModel Parent {
            get { return _parent; }
            set
            {
                _parent = value;
                OnPropertyChanged();
            }
        }

        public FavFileModel()
        {
            FavChildren = new ObservableCollection<FavFileModel>();
        }
    }
}
