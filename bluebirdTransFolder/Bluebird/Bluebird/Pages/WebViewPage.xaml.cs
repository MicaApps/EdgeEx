using Bluebird.Core;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.ExtendedExecution;
using Windows.ApplicationModel.ExtendedExecution.Foreground;
using System.ServiceModel.Channels;
using System.Security.Authentication.ExtendedProtection;
using Windows.ApplicationModel.Background;
using Windows.Services.Maps;
using Windows.UI.Xaml.Documents;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Shapes;
using Bluebird.Shared;
using System.IO;
using System.Threading.Tasks;

namespace Bluebird.Pages;

public sealed partial class WebViewPage : Page
{
    public static event System.EventHandler<EnteredBackgroundEventArgs> EnteredBackground;
    public static event System.EventHandler<LeavingBackgroundEventArgs> ExitBackground;

    public WebViewPage()
    {
        this.InitializeComponent();

        WebViewControl.EnsureCoreWebView2Async();

        EnteredBackground += WebViewPage_EnteredBackground;
        ExitBackground += WebViewPage_ExitBackground;
    }

    //private async void returnFileContent() disabled for now until it works better
    //{
     //   try
       // {
         //   // Try to get the file
           // StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync("ProfileCore.rash");
            // Read the content of the file
            //string folderName = await FileIO.ReadTextAsync(file);
            // Try to get the folder with the name from the file
            //StorageFolder folder = await ApplicationData.Current.LocalFolder.GetFolderAsync(folderName);

            //CoreWebView2Environment webView2Environment = null;

            //string installPath = folder.ToString();

//           CoreWebView2EnvironmentOptions options = new CoreWebView2EnvironmentOptions();

  //          var env = await CoreWebView2Environment.CreateWithOptionsAsync("", installPath, options);
            
    //    }
      //  catch (FileNotFoundException)
        //{
            // If an exception is thrown, the file does not exist
          //  System.Diagnostics.Debug.WriteLine("File does not exist");
        //}
        //catch (IOException)
       // {
            // If an exception is thrown, the folder does not exist
         //   System.Diagnostics.Debug.WriteLine("Folder does not exist");
       // }
    //}


  
    private void WebViewPage_ExitBackground(object sender, LeavingBackgroundEventArgs e)
    {
        
    }

    private void WebViewPage_EnteredBackground(object sender, EnteredBackgroundEventArgs e)
    {
       
    }

    #region WebViewEvents
    private void WebViewControl_CoreWebView2Initialized(WebView2 sender, CoreWebView2InitializedEventArgs args)
    {
        // WebViewEvents
        sender.CoreWebView2.NavigationStarting += CoreWebView2_NavigationStarting;
        sender.CoreWebView2.NavigationCompleted += CoreWebView2_NavigationCompleted;
        sender.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
        sender.CoreWebView2.ContextMenuRequested += CoreWebView2_ContextMenuRequested;
        sender.CoreWebView2.DocumentTitleChanged += CoreWebView2_DocumentTitleChanged;
        sender.CoreWebView2.FaviconChanged += CoreWebView2_FaviconChanged;
        // Apply WebView2 settings
        
        ApplyWebView2Settings();
        if (launchurl != null)
        {
            WebViewControl.Source = new Uri(launchurl);
            launchurl = null;
        }
    }

    private void ApplyWebView2Settings()
    {
        if (SettingsHelper.GetSetting("DisableJavaScript") is "true")
        {
            WebViewControl.CoreWebView2.Settings.IsScriptEnabled = false;
        }
        if (SettingsHelper.GetSetting("DisableSwipeNav") is "true")
        {
            WebViewControl.CoreWebView2.Settings.IsSwipeNavigationEnabled = false;
        }
        if (SettingsHelper.GetSetting("DisableGenAutoFill") is "true")
        {
            WebViewControl.CoreWebView2.Settings.IsGeneralAutofillEnabled = false;
        }
        if (SettingsHelper.GetSetting("DisableWebMess") is "true")
        {
            WebViewControl.CoreWebView2.Settings.IsWebMessageEnabled = false;
        }
        if (SettingsHelper.GetSetting("DisablePassSave") is "true")
        {
            WebViewControl.CoreWebView2.Settings.IsPasswordAutosaveEnabled = false;
        }
    }

    private void CoreWebView2_NavigationStarting(CoreWebView2 sender, CoreWebView2NavigationStartingEventArgs args)
    {
        MainPageContent.LoadingRing.IsActive = true;
    }
    private void CoreWebView2_NavigationCompleted(CoreWebView2 sender, CoreWebView2NavigationCompletedEventArgs args)
    {
        MainPageContent.LoadingRing.IsActive = false;
    }

    private void CoreWebView2_FaviconChanged(CoreWebView2 sender, object args)
    {
        MainPageContent.SelectedTab.IconSource = IconHelper.ConvFavURLToIconSource(sender.FaviconUri);
    }

    private void CoreWebView2_NewWindowRequested(CoreWebView2 sender, CoreWebView2NewWindowRequestedEventArgs args)
    {
        launchurl = args.Uri;
        MainPageContent.CreateWebTab();
        args.Handled = true;
    }

    private void CoreWebView2_ContextMenuRequested(CoreWebView2 sender, CoreWebView2ContextMenuRequestedEventArgs args)
    {
        IList<CoreWebView2ContextMenuItem> menuList = args.MenuItems;
        //MainPageContent.JsonListView.ItemsSource = menuList;
        for (int index = 0; index < menuList.Count; index++)
        {
            if (menuList[index].Name == "openLinkInNewWindow" || menuList[index].Name == "print" || menuList[index].Name == "emoji" || menuList[index].Name == "webCapture")
            {
                menuList.RemoveAt(index);
            }
        }
    }

    private void CoreWebView2_DocumentTitleChanged(CoreWebView2 sender, object args)
    {
        MainPageContent.SelectedTab.Header = sender.DocumentTitle;
    }
    #endregion
}
