global using static Bluebird.Core.Globals;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using System.Diagnostics;
using Windows.Storage;
using Bluebird;
using Windows.System;
using Bluebird.Core;
using Windows.Media.Editing;
using Windows.Media.Playback;
using Microsoft.UI.Xaml.Controls;
using Bluebird.Pages;
using Microsoft.Web.WebView2.Core;
using Windows.ApplicationModel.Background;

namespace Bluebird;

/// <summary>
/// Stellt das anwendungsspezifische Verhalten bereit, um die Standardanwendungsklasse zu ergänzen.
/// </summary>
sealed partial class App : Application
{
    /// <summary>
    /// Initialisiert das Singletonanwendungsobjekt. Dies ist die erste Zeile von erstelltem Code
    /// und daher das logische Äquivalent von main() bzw. WinMain().
    /// </summary>
    public App()
    {
        this.InitializeComponent();
        this.Suspending += OnSuspending;
        LoadSettings();

        this.EnteredBackground += App_EnteredBackground;
        this.LeavingBackground += App_LeavingBackground;
    }

    bool _isInBackgroundMode = false;
    private void App_LeavingBackground(object sender, LeavingBackgroundEventArgs e)
    {
        _isInBackgroundMode = false;
    }
  
    private void App_EnteredBackground(object sender, EnteredBackgroundEventArgs e)
    {
        _isInBackgroundMode = true;
    }

    private void LoadSettings()
    {
        SearchUrl = SettingsHelper.GetSetting("SearchUrl");
        TLD.LoadKnownDomains();
    }

    protected override void OnActivated(IActivatedEventArgs args)
    {
        ProtocolActivatedEventArgs eventArgs = args as ProtocolActivatedEventArgs;
        string URI = eventArgs.Uri.ToString();
        if (URI.Contains("bluebird:")) launchurl = eventArgs.Uri.AbsolutePath;
        else { launchurl = URI; }
        if (args.Kind == ActivationKind.Protocol)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;
                rootFrame.Navigate(typeof(MainPage));

                Window.Current.Activate();

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }
            else
            {
                // Since we already have a window open
                // we create a new tab with the uri
                Core.Globals.MainPageContent.CreateWebTab();
            }
        }
        else
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                rootFrame = new Frame();
                Window.Current.Content = rootFrame;
            }


            string payload = "BlueBirdStartup";
            if (args.Kind == ActivationKind.StartupTask)
            {
                var startupArgs = args as StartupTaskActivatedEventArgs;
                payload = ActivationKind.StartupTask.ToString();
            }

            rootFrame.Navigate(typeof(MainPage), payload);
            Window.Current.Activate();
        }
    }

    /// <summary>
    /// Wird aufgerufen, wenn die Anwendung durch den Endbenutzer normal gestartet wird. Weitere Einstiegspunkte
    /// werden z. B. verwendet, wenn die Anwendung gestartet wird, um eine bestimmte Datei zu öffnen.
    /// </summary>
    /// <param name="e">Details über Startanforderung und -prozess.</param>
    protected override void OnLaunched(LaunchActivatedEventArgs e)
    {
        Frame rootFrame = Window.Current.Content as Frame;

        // Do not repeat app initialization when the Window already has content,
        // just ensure that the window is active
        if (rootFrame == null)
        {
            // Create a Frame to act as the navigation context and navigate to the first page
            rootFrame = new Frame();

            rootFrame.NavigationFailed += OnNavigationFailed;

            if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                //TODO: Load state from previously suspended application
            }

            // Place the frame in the current Window
            Window.Current.Content = rootFrame;
        }

        if (e.PrelaunchActivated == false)
        {
            // On Windows 10 version 1607 or later, this code signals that this app wants to participate in prelaunch
            // Since Bluebird does not run on <1809, no check is required
            TryEnablePrelaunch();

            // TODO: This is not a prelaunch activation. Perform operations which
            // assume that the user explicitly launched the app such as updating
            // the online presence of the user on a social network, updating a
            // what's new feed, etc.

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }
    }

    /// <summary>
    /// Wird aufgerufen, wenn die Navigation auf eine bestimmte Seite fehlschlägt
    /// </summary>
    /// <param name="sender">Der Rahmen, bei dem die Navigation fehlgeschlagen ist</param>
    /// <param name="e">Details über den Navigationsfehler</param>
    void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
    {
        throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
    }

    /// <summary>
    /// Wird aufgerufen, wenn die Ausführung der Anwendung angehalten wird.  Der Anwendungszustand wird gespeichert,
    /// ohne zu wissen, ob die Anwendung beendet oder fortgesetzt wird und die Speicherinhalte dabei
    /// unbeschädigt bleiben.
    /// </summary>
    /// <param name="sender">Die Quelle der Anhalteanforderung.</param>
    /// <param name="e">Details zur Anhalteanforderung.</param>
    private void OnSuspending(object sender, SuspendingEventArgs e)
    {
        var deferral = e.SuspendingOperation.GetDeferral();
        //TODO: Anwendungszustand speichern und alle Hintergrundaktivitäten beenden
        deferral.Complete();
    }

    /// <summary>
    /// This method should be called only when the caller
    /// determines that we're running on a system that
    /// supports CoreApplication.EnablePrelaunch.
    /// </summary>
    private void TryEnablePrelaunch()
    {
        Windows.ApplicationModel.Core.CoreApplication.EnablePrelaunch(true);
    }
}
