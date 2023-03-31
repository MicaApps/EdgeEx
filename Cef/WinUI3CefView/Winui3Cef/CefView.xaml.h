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

        bool CanGoBack();
        void GoBack();

        bool CanGoForward();
        void GoForward();

        winrt::Windows::UI::Color DefaultBackgroundColor();
        
        winrt::Windows::Foundation::Uri Source();
        void Source(winrt::Windows::Foundation::Uri value);

        void NavigateToString(winrt::hstring htmlContent);
        void Close();

        winrt::Windows::Foundation::IAsyncOperation<winrt::hstring> ExecuteScriptAsync(winrt::hstring javascriptCode);



        void UserControl_SizeChanged(winrt::Windows::Foundation::IInspectable const& sender, winrt::Microsoft::UI::Xaml::SizeChangedEventArgs const& e);

        //void CanLoad();

        //winrt::event_token WebMessageReceived(Windows::Foundation::EventHandler<winrt::hstring> const& handler);
        //void WebMessageReceived(winrt::event_token const& token) noexcept;

        //void ExecuteScript(winrt::hstring script);
    private:
        winrt::event<Windows::Foundation::EventHandler<winrt::hstring>> m_webMessageReceivedEvent;
        HWND m_cefHwnd;
        bool m_isInit = true;
        HRGN toHRGN(winrt::Windows::Foundation::Size size);
        winrt::Windows::Foundation::Point getOrigin();
        void createBrowser();
    public:

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
