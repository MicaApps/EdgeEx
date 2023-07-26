using EdgeEx.WinUI3.Args;
using EdgeEx.WinUI3.Enums;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using WinUIEx;

namespace EdgeEx.WinUI3.Interfaces
{
    public interface ICallerToolkit
    {
        /// <summary>
        /// Window Backdrop Changed Event
        /// </summary>
        event EventHandler<WindowBackdropChangedEventArg> WindowBackdropChangedEvent;
        /// <summary>
        /// Uri Navigated Event
        /// </summary>
        event EventHandler<UriNavigatedEventArg> AddressUriNavigatedEvent;
        /// <summary>
        /// Uri Navigated Starting Event
        /// </summary>
        event EventHandler<UriNavigatedMessageEventArg> UriNavigatedStartingEvent;
        /// <summary>
        /// Uri Navigated Completed Event
        /// </summary>
        event EventHandler<UriNavigatedMessageEventArg> UriNavigationCompletedEvent;
        /// <summary>
        /// Size Changed Event
        /// </summary>
        event EventHandler<SizeChangedEventArgs> SizeChangedEvent;
        /// <summary>
        /// Frame Status Event
        /// </summary>
        event EventHandler<FrameStatusEventArg> FrameStatusEvent;
        /// <summary>
        /// Frame Operation Event
        /// </summary>
        event EventHandler<FrameOperationEventArg> FrameOperationEvent;
        /// <summary>
        /// Frame Favorite Event
        /// </summary>
        event EventHandler<FavoriteEventArg> FavoriteEvent;
        event EventHandler<LoadingEventArg> LoadingEvent;
        event EventHandler<LoadingProgressEventArg> LoadingProgressEvent;
        /// <summary>
        /// Change Window Backdrop  
        /// </summary>
        void ChangeWindowBackdrop(WindowBackdrop oldMode, WindowBackdrop newMode, Color tintColor, Color fallbackColor, float tintOpacity, MicaKind kind);

        /// <summary>
        /// Navigate By Uri
        /// </summary>
        void NavigateUri(Uri uri);
        /// <summary>
        /// Size Changed
        /// </summary>
        void SizeChanged(SizeChangedEventArgs args);
        /// <summary>
        /// Send NavigatedUri Starting
        /// </summary>
        void UriNavigatedStarting(object sender, string persistenceId, string tabItemName, Uri uri);
        /// <summary>
        /// Send NavigatedUri Completed
        /// </summary>
        void UriNavigationCompleted(object sender, string persistenceId, string tabItemName, Uri uri, string title, IconSource icon);
        /// <summary>
        /// Frame Status
        /// </summary>
        void FrameStatus(object sender,string persistenceId, string tabItemName, bool canBack, bool canForward, bool canRefresh);
        /// <summary>
        /// Frame Operate
        /// </summary>
        void FrameOperate(object sender, string persistenceId, string tabItemName, FrameOperation operation);
        /// <summary>
        /// Frame Favorite
        /// </summary>
        void Favorite(object sender, string persistenceId, string tabItemName, bool isFavorite, string uri,string title,string folderId);

        void Loading(object sender, bool isLoading,string title);
        void LoadingProgress(object sender, double progress);
    }
}
