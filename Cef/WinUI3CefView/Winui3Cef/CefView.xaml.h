// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

#pragma once

#include "winrt/Microsoft.UI.Xaml.h"
#include "winrt/Microsoft.UI.Xaml.Markup.h"
#include "winrt/Microsoft.UI.Xaml.Controls.Primitives.h"
#include "CefView.g.h"
#include <wingdi.h>

class BrowserClient;
namespace winrt::Winui3Cef::implementation
{
    struct CefView : CefViewT<CefView>
    {
        CefView();

        void UserControl_SizeChanged(winrt::Windows::Foundation::IInspectable const& sender, winrt::Microsoft::UI::Xaml::SizeChangedEventArgs const& e);

        void CanLoad();

        winrt::event_token WebMessageReceived(Windows::Foundation::EventHandler<winrt::hstring> const& handler);
        void WebMessageReceived(winrt::event_token const& token) noexcept;

        void ExecuteScript(winrt::hstring script);
    private:
        winrt::event<Windows::Foundation::EventHandler<winrt::hstring>> m_webMessageReceivedEvent;
        HWND m_cefHwnd;
        HRGN toHRGN(winrt::Windows::Foundation::Size size);
        winrt::Windows::Foundation::Point getOrigin();
    public:
        void createBrowser();
        void UserControl_Loaded(winrt::Windows::Foundation::IInspectable const& sender, winrt::Microsoft::UI::Xaml::RoutedEventArgs const& e);
        winrt::Windows::Foundation::Size m_size{};
        BrowserClient* m_client{};
    };
}

namespace winrt::Winui3Cef::factory_implementation
{
    struct CefView : CefViewT<CefView, implementation::CefView>
    {
    };
}
