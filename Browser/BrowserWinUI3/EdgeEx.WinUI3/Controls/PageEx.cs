using EdgeEx.WinUI3.Args;
using EdgeEx.WinUI3.Helpers;
using EdgeEx.WinUI3.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUIEx;

namespace EdgeEx.WinUI3.Controls;
public class PageEx : Page
{
    protected readonly ICallerToolkit caller = App.Current.Services.GetService<ICallerToolkit>();
    /// <summary>
    /// TabItem Id
    /// </summary>
    protected string TabItemName { get; set; }
    /// <summary>
    /// Uri
    /// </summary>
    protected Uri NavigateUri { get; set; }
    /// <summary>
    /// Window Id
    /// </summary>
    protected string PersistenceId { get; set; }
    public PageEx() : base() { }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        Frame.Loaded += (sender, e) =>
        {
            InitPersistenceId();
        };
        if (e.Parameter is NavigatePageArg args)
        {
            TabItemName = args.TabItemName;
            NavigateUri = args.NavigateUri;
        }
        caller.SizeChangedEvent += Caller_SizeChangedEvent;
        caller.FrameOperationEvent += Caller_FrameOperationEvent;
    }
    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        caller.SizeChangedEvent -= Caller_SizeChangedEvent;
        caller.FrameOperationEvent -= Caller_FrameOperationEvent;
    }
    /// <summary>
    /// get window id
    /// </summary>
    private void InitPersistenceId()
    {
        WindowEx window = WindowHelper.GetWindowForElement(this);
        PersistenceId = window.PersistenceId;
        Debug.WriteLine(PersistenceId);
    }
    protected virtual void Caller_SizeChangedEvent(object sender, SizeChangedEventArgs e) { }
    protected virtual void Caller_FrameOperationEvent(object sender, FrameOperationEventArg e) { }
}