// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

#pragma once

#include "MainWindow.g.h"

namespace winrt::Winui3Cef::implementation
{
    struct MainWindow : MainWindowT<MainWindow>
    {
        MainWindow();

        int32_t MyProperty();
        void MyProperty(int32_t value);

        void myButton_Click(Windows::Foundation::IInspectable const& sender, Microsoft::UI::Xaml::RoutedEventArgs const& args);
        void CefView_TemperatureIsBelowFreezing(winrt::Windows::Foundation::IInspectable const& sender, float e);
        void CefView_WebMessageReceived(winrt::Windows::Foundation::IInspectable const& sender, winrt::hstring const& e);
    };
}

namespace winrt::Winui3Cef::factory_implementation
{
    struct MainWindow : MainWindowT<MainWindow, implementation::MainWindow>
    {
    };
}
