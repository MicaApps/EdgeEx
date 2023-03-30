// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

#include "pch.h"
#include "MainWindow.xaml.h"
#if __has_include("MainWindow.g.cpp")
#include "MainWindow.g.cpp"
#endif

using namespace winrt;
using namespace Microsoft::UI::Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace winrt::Winui3Cef::implementation
{
    MainWindow::MainWindow()
    {
        InitializeComponent();
    }
}


void winrt::Winui3Cef::implementation::MainWindow::NavigateButton_Click(winrt::Windows::Foundation::IInspectable const& sender, winrt::Microsoft::UI::Xaml::RoutedEventArgs const& e)
{
    CefView().Source(winrt::Windows::Foundation::Uri{ urlText().Text() });
}


void winrt::Winui3Cef::implementation::MainWindow::GoForwardButton_Click(winrt::Windows::Foundation::IInspectable const& sender, winrt::Microsoft::UI::Xaml::RoutedEventArgs const& e)
{
    if (CefView().CanGoForward())
        CefView().GoForward();
}


void winrt::Winui3Cef::implementation::MainWindow::GoBackwardButton_Click(winrt::Windows::Foundation::IInspectable const& sender, winrt::Microsoft::UI::Xaml::RoutedEventArgs const& e)
{
    if (CefView().CanGoBack())
        CefView().GoBack();
}


void winrt::Winui3Cef::implementation::MainWindow::ExecuteScriptButton_Click(winrt::Windows::Foundation::IInspectable const& sender, winrt::Microsoft::UI::Xaml::RoutedEventArgs const& e)
{
    CefView().ExecuteScriptAsync(
        L"alert('execute script')"
    );
}
