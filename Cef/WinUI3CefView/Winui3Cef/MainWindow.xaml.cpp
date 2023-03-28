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

    void MainWindow::myButton_Click(IInspectable const&, RoutedEventArgs const&)
    {
        CefView().ExecuteScript(LR"(alert('Execute script in cef');)");
    }
}


void winrt::Winui3Cef::implementation::MainWindow::CefView_TemperatureIsBelowFreezing(winrt::Windows::Foundation::IInspectable const& sender, float e)
{
    OutputDebugString(L"Event!!!!!!!!!!!!!!!!!!!!!");
}


void winrt::Winui3Cef::implementation::MainWindow::CefView_WebMessageReceived(winrt::Windows::Foundation::IInspectable const& sender, winrt::hstring const& e)
{
    OutputDebugString(e.data());
}
