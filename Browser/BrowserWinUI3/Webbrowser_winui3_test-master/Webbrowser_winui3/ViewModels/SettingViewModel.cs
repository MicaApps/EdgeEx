
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI;
using CommunityToolkit.WinUI.Helpers;
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Webbrowser_winui3.Helpers;
using Webbrowser_winui3.Models;
using Windows.ApplicationModel;
using Windows.Storage;

namespace Webbrowser_winui3.ViewModels
{
    public class SettingViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _IsBingCbChecked;
        public bool IsBingCbChecked
        {
            get { return _IsBingCbChecked; }
            set
            {
                _IsBingCbChecked = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsBingCbChecked)));
            }
        } 
        private bool _IsAcrylic;
        public bool IsAcrylic
        {
            get { return _IsAcrylic; }
            set
            {
                _IsAcrylic = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsAcrylic)));
            }
        }
        private int _ComboBox_SelectIndex = 0;
        public int ComboBox_SelectIndex
        {
            get { return _ComboBox_SelectIndex; }
            set
            {
                _ComboBox_SelectIndex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ComboBox_SelectIndex)));
            }
        }
        private int _ComboBox_EngineSelectIndex = 0;
        public int ComboBox_EngineSelectIndex
        {
            get { return _ComboBox_EngineSelectIndex; }
            set
            {
                _ComboBox_EngineSelectIndex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ComboBox_EngineSelectIndex)));
            }
        }
        private string _versionDescription;
        public string VersionDescription
        {
            get { return GetVersionDescription(); }
            set
            {
                _versionDescription = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VersionDescription)));
            }
        }
        private static string GetVersionDescription()
        {
            Version version;

            if (RuntimeHelper.IsMSIX)
            {
                var packageVersion = Package.Current.Id.Version;

                version = new(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
            }
            else
            {
                version = Assembly.GetExecutingAssembly().GetName().Version!;
            }

            return $"{"AppDisplayName".GetLocalized()} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }
        public ICommand ComboBox_SelectionChangedCommand;
        public ICommand ComboBox_SelectionEngineChangedCommand;
        public ICommand SettingInitCommand;
        public ICommand ThemeInitCommand;
        public ICommand CheckBox_ClickCommand;
        public ICommand CheckBox1_ClickCommand;
        public SettingViewModel()
        {
            CheckBox1_ClickCommand = new RelayCommand<object>(async (param) =>
            {
                if (param != null)
                {
                    if ((param as CheckBox).IsChecked == true)
                    {
                        App._MainWindow.Backdrop = new AcrylicSystemBackdrop();
                        ApplicationData.Current.LocalSettings.Values["IsAcrylicOrMica"] = "True";
                    }
                    else
                    {
                        App._MainWindow.Backdrop = new MicaSystemBackdrop();
                        ApplicationData.Current.LocalSettings.Values["IsAcrylicOrMica"] = "False";
                    }
                }
            });
            CheckBox_ClickCommand = new RelayCommand<object>(async (param) =>
            {
                if (param != null)
                {
                    if ((param as CheckBox).IsChecked == true)
                    {
                        MainViewModel._MainPage.g_bing.Background = new ImageBrush() { ImageSource = await MainViewModel.GetBingImage() };
                        ApplicationData.Current.LocalSettings.Values["IsBingBackground"] = "True";
                    }
                    else
                    {
                        MainViewModel._MainPage.g_bing.Background = null;
                        ApplicationData.Current.LocalSettings.Values["IsBingBackground"] = "False";
                    }
                }
            });
            ComboBox_SelectionEngineChangedCommand = new RelayCommand<object>(async (param) =>
            {
                if (param != null)
                {
                    var index = ((ComboBox)param).SelectedIndex;
                    if (index > -1)
                    {
                        ApplicationData.Current.LocalSettings.Values["SearchEngine"] = MainViewModel._EngineSource[index].Url;
                    }
                }
            });
            ComboBox_SelectionChangedCommand = new RelayCommand<object>(async (param) =>
            {
                if (param != null)
                {
                    if (MainViewModel._RequestedThemeList != null)
                    {
                        var theme = (ElementTheme)Enum.Parse(typeof(ElementTheme), ((param as ComboBox).SelectedItem as ComboBoxItem).Content.ToString());
                        SaveRequestedTheme(theme);
                        foreach (var line in MainViewModel._RequestedThemeList)
                        {
                            await SetRequestedThemeAsync(line, theme);
                        }
                    }
                }
            });
            SettingInitCommand = new RelayCommand<object[]>((param) =>
            {
                MainViewModel.InitSearchEngine();
                (param[0] as ComboBox).ItemsSource = MainViewModel._EngineSource;
                ComboBox_SelectIndex = GetSavedRequestedTheme() == ElementTheme.Default ? 0 : GetSavedRequestedTheme() == ElementTheme.Light ? 1 : GetSavedRequestedTheme() == ElementTheme.Dark ? 2 : 0;
                IsBingCbChecked = ApplicationData.Current.LocalSettings.Values.ContainsKey("IsBingBackground") ? ApplicationData.Current.LocalSettings.Values["IsBingBackground"].ToString() == "True" ? true : false : false;
                IsAcrylic = ApplicationData.Current.LocalSettings.Values.ContainsKey("IsAcrylicOrMica") ? ApplicationData.Current.LocalSettings.Values["IsAcrylicOrMica"].ToString() == "False" ? false : true : true;
                var SearchEnginet = ApplicationData.Current.LocalSettings.Values.ContainsKey("SearchEngine") ? ApplicationData.Current.LocalSettings.Values["SearchEngine"].ToString() : MainViewModel._EngineSource[0].Url;
                ComboBox_EngineSelectIndex = MainViewModel._EngineSource.IndexOf(MainViewModel._EngineSource.First(o => o.Url == SearchEnginet)); ;
            });
            ThemeInitCommand = new RelayCommand<object>(async (param) =>
            {
                if (MainViewModel._RequestedThemeList != null)
                {
                    foreach (var line in MainViewModel._RequestedThemeList)
                    {
                        await SetRequestedThemeAsync(line, GetSavedRequestedTheme());
                    }
                }
            });
        }
        /// <summary>
        /// 获得应用运行时改变的主题
        /// </summary>
        /// <returns></returns>
        public ElementTheme GetSavedRequestedTheme()
        {
            var Theme = ApplicationData.Current.LocalSettings.Values.ContainsKey("ElementTheme") ? ApplicationData.Current.LocalSettings.Values["ElementTheme"].ToString() : "Dark";
            return (ElementTheme)Enum.Parse(typeof(ElementTheme), Theme == "" ? "Dark" : Theme);
        }
        /// <summary>
        /// 应用运行时改变主题
        /// </summary>
        /// <returns></returns>
        public async Task SetRequestedThemeAsync(Grid root, ElementTheme Theme)
        {
            if (root is FrameworkElement rootElement)
            {
                rootElement.RequestedTheme = Theme;
            }

            await Task.CompletedTask;
        }
        /// <summary>
        /// 保存应用运行时改变主题
        /// </summary>
        /// <returns></returns>
        public void SaveRequestedTheme(ElementTheme Theme)
        {
            ApplicationData.Current.LocalSettings.Values["ElementTheme"] = Theme.ToString();
        }
    }
}
