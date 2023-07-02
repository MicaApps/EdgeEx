using CommunityToolkit.Mvvm.ComponentModel;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webbrowser_winui3.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public abstract class ViewModelBase:ObservableObject
    {
        public virtual void OnStartup(object parameter)
        {

        }
    }
}
