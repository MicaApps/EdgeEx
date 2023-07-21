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
        public event EventHandler<UriNavigatedEventArg> UriNavigatedEvent;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event EventHandler<SizeChangedEventArgs> SizeChangedEvent;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event EventHandler<UriNavigatedMessageEventArg> UriNavigatedMessageEvent;
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
        public void ChangeWindowBackdrop(WindowBackdrop oldMode, WindowBackdrop newMode, Color tintColor, Color fallbackColor, float tintOpacity, MicaKind kind)
        {
            WindowBackdropChangedEvent?.Invoke(this, new WindowBackdropChangedEventArg(oldMode, newMode, tintColor, fallbackColor, tintOpacity, kind));
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
        public void FrameStatus(object sender, string persistenceId, bool canBack, bool canForward, bool canRefresh)
        {
            FrameStatusEvent?.Invoke(sender, new FrameStatusEventArg(persistenceId,canBack, canForward, canRefresh)); 
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void NavigateUri(Uri uri)
        {
            UriNavigatedEvent?.Invoke(this, new UriNavigatedEventArg(uri));
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void SendUriNavigatedMessage(object sender, string persistenceId, string tabItemName, Uri uri,string title, IconSource icon)
        {
            UriNavigatedMessageEvent?.Invoke(sender, new UriNavigatedMessageEventArg(persistenceId, tabItemName, uri, title, icon));
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void SizeChanged(SizeChangedEventArgs args)
        {
            SizeChangedEvent?.Invoke(this, args);
        } 
    }
}
