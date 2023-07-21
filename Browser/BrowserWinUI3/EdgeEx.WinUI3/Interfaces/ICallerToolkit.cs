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
        event EventHandler<UriNavigatedEventArg> UriNavigatedEvent;
        /// <summary>
        /// Uri Navigated Message Event
        /// </summary>
        event EventHandler<UriNavigatedMessageEventArg> UriNavigatedMessageEvent;
        /// <summary>
        /// Size Changed Event
        /// </summary>
        event EventHandler<SizeChangedEventArgs> SizeChangedEvent;
        /// <summary>
        /// Frame Status Event
        /// </summary>
        event EventHandler<FrameStatusEventArg> FrameStatusEvent;
        event EventHandler<FrameOperationEventArg> FrameOperationEvent;
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
        /// Send NavigatedUri Message
        /// </summary>
        void SendUriNavigatedMessage(object sender, string persistenceId, string tabItemName, Uri uri, string title, IconSource icon);
        /// <summary>
        /// Frame Status
        /// </summary>
        void FrameStatus(object sender,string persistenceId,bool canBack, bool canForward, bool canRefresh);

        void FrameOperate(object sender, string persistenceId, string tabItemName, FrameOperation operation);
    }
}
