using EdgeEx.WinUI3.Args;
using EdgeEx.WinUI3.Enums;
using EdgeEx.WinUI3.Interfaces;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using WinUIEx;

namespace EdgeEx.WinUI3.Toolkits
{
    public class CallerToolkit : ICallerToolkit
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event EventHandler<WindowBackdropChangedEventArg> WindowBackdropChangedEvent;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event EventHandler<UriNavigatedEventArg> AddressUriNavigatedEvent;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event EventHandler<SizeChangedEventArgs> SizeChangedEvent;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event EventHandler<FrameStatusEventArg> FrameStatusEvent;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event EventHandler<FrameOperationEventArg> FrameOperationEvent;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event EventHandler<FavoriteEventArg> FavoriteEvent;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event EventHandler<UriNavigatedMessageEventArg> UriNavigatedStartingEvent;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event EventHandler<UriNavigatedMessageEventArg> UriNavigationCompletedEvent;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event EventHandler<LoadingEventArg> LoadingEvent;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event EventHandler<LoadingProgressEventArg> LoadingProgressEvent;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void ChangeWindowBackdrop(WindowBackdrop oldMode, WindowBackdrop newMode, Color tintColor, Color fallbackColor, float tintOpacity, MicaKind kind)
        {
            WindowBackdropChangedEvent?.Invoke(this, new WindowBackdropChangedEventArg(oldMode, newMode, tintColor, fallbackColor, tintOpacity, kind));
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Favorite(object sender, string persistenceId, string tabItemName, bool isFavorite, string uri,string title, string folderId)
        {
            FavoriteEvent?.Invoke(sender,new FavoriteEventArg(persistenceId, tabItemName, isFavorite, uri, title, folderId));
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void FrameOperate(object sender, string persistenceId, string tabItemName, FrameOperation operation)
        {
            FrameOperationEvent?.Invoke(sender, new FrameOperationEventArg(persistenceId, tabItemName, operation));
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void FrameStatus(object sender, string persistenceId, string tabItemName, bool canBack, bool canForward, bool canRefresh)
        {
            FrameStatusEvent?.Invoke(sender, new FrameStatusEventArg(persistenceId, tabItemName, canBack, canForward, canRefresh)); 
        }

        public void Loading(object sender, bool isLoading, string title)
        {
            LoadingEvent?.Invoke(sender, new LoadingEventArg(isLoading, title));
        }

        public void LoadingProgress(object sender, double progress)
        {
            LoadingProgressEvent?.Invoke(sender,new LoadingProgressEventArg(progress));
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void NavigateUri(Uri uri)
        {
            AddressUriNavigatedEvent?.Invoke(this, new UriNavigatedEventArg(uri));
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void SizeChanged(SizeChangedEventArgs args)
        {
            SizeChangedEvent?.Invoke(this, args);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void UriNavigatedStarting(object sender, string persistenceId, string tabItemName, Uri uri)
        {
            UriNavigatedStartingEvent?.Invoke(sender, new UriNavigatedMessageEventArg(persistenceId, tabItemName, uri, null, null));
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void UriNavigationCompleted(object sender, string persistenceId, string tabItemName, Uri uri, string title, IconSource icon)
        {
            UriNavigationCompletedEvent?.Invoke(sender, new UriNavigatedMessageEventArg(persistenceId, tabItemName, uri, title, icon));
        }
    }
}
