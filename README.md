# EdgeEx
- UWP Edge, reborn
- UWP Edge重生

## Current Development and how to contribute（如何贡献，项目如何进展）
1. Make a GUI shell without dependent on the core of the browser underhood(such as homepage, download, settings page), 
    could be UWP+WinUI2/WinUI3/Xaml Island
    （完成一个不依赖于浏览器底层的界面，比如首页、下载页、设置页，可以使用的框架UWP+WinUI2/WinUI3/Xaml岛）
2. Wrapping up all the features and APIs needed from the browser core
    （封装出所有需要的浏览器内核的外围功能和API）
3. Adapt both webview2 and cef to this set of APIs. WebView2 APIs goes directly into the `Browsers` project.
    （将webview2和cef共同适配到这套API，其中适配WebView2的代码直接写到`Browser`工程中）

## Project structure（文件夹结构）
```
    Browser     --- The main browser client
        BrowserUWP      --- UWP-based Browser       
        BrowserWinUI3   --- WinUI3-based Browser                    *
        BrowserIsland   --- Win32 Xaml Island-based Browser
    Cef         --- Cef Component                    
        CefBase         --- Common base for Cef (include/lib files)
        WinUI2CefView   --- WinUI2 Cef Component  
        WinUI3CefView   --- WinUI3 Cef Component                    *

* - active
```
## Development Pre-requisites（开发环境）
- To develop Cef, run `cef\prepare.ps1` （开发Cef，先运行`cef\prepare.ps1`）
- You should have a good understanding in one of C++, C#, UWP, WinUI3, Cef frameworks. （你应该至少在C++、C#、UWP、WinUI3、Cef框架中至少熟悉一样）

## Current Issues Listing（当前踩坑列表）
### WinUI2 related （WinUI2 UWP相关）
### WinUI3 related（WinUI3相关）
- Current Cef window was created using child window which has a higher z-order in the window structure, 
meaning this cef window will cover anything that overlapps with the xaml content.
（当前Cef窗口使用子窗口的形式创建，但是这种方式会挡住一切跟xaml内容重叠的部分）. 
### Xaml Island related （Xaml Island相关）

## APIs reference

### Control Layer

### Webview2 (Xaml inherited member omitted)
|Api|Necessary|Implement in Cef|
|--|--|--|
`bool CanGoBack{get;}                                                                `|*|*
`void GoBack()                                                                       `|*|*
`bool CanGoForward{get;}                                                             `|*|*
`void GoForward()                                                                    `|*|*
`Windows.UI.Color DefaultBackgroundColor{get;}                                       `|*|*
`Windows.Foundation.Uri Source{get;set;}                                             `|*|*
`void NavigateToString(String htmlContent)                                           `|*|*
`void Close()                                                                        `|*|*
`Windows.Foundation.IAsyncOperation<String> ExecuteScriptAsync(String javascriptCode)`|*|*

|Events|Necessary|Implemented in Cef|
|--|--|--|
`CoreProcessFailed      `|
`CoreWebView2Initialized`|
`NavigationStarting     `|
`WebMessageReceived     `|*

### Core Layer
### CoreWebView2
