// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

#include "pch.h"
#include "CefView.xaml.h"
#if __has_include("CefView.g.cpp")
#include "CefView.g.cpp"
#endif

#include <include/cef_sandbox_win.h>
#include <include/cef_command_line.h>
#include <thread>
#include <include/cef_app.h>

#include "Env.hpp"
#include <microsoft.ui.xaml.window.h>
#pragma comment(lib, "cef_sandbox.lib")

#include "Global.h"
import internal;
#include <include/wrapper/cef_helpers.h>
#include <include/cef_parser.h>
#include <winrt/Windows.UI.h>
#include "Utils.h"

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

static auto GetCefCommandLine()
{
    auto commandLine = CefCommandLine::CreateCommandLine();
    commandLine->InitFromString(GetCommandLineW());
    return commandLine;
}

auto GetHwnd(winrt::Microsoft::UI::Xaml::Window& window)
{
    HWND hwnd{};
    window.as<IWindowNative>()->get_WindowHandle(&hwnd);
    return hwnd;
}

static CefSettings GetSettings()
{
    CefSettings settings;
    settings.no_sandbox = true;
    CefString(&settings.browser_subprocess_path) = Env::RenderProcessPath();
    CefString(&settings.resources_dir_path) = Env::ResourcePath();
    CefString(&settings.locales_dir_path) = Env::LocalesPath();
    settings.external_message_pump = false;
    settings.multi_threaded_message_loop = true;
    CefString(&settings.cache_path) = Env::CachePath();
    CefString s;
    return settings;
}

namespace winrt::Winui3Cef::implementation
{
    CefView::CefView()
    {
        InitializeComponent();
    }

    bool CefView::CanGoBack()
    {
        return m_client->m_host->GetBrowser()->CanGoBack();
    }

    void CefView::GoBack()
    {
        //CefRefPtr<CefBrowser> browser{ m_client->m_browser };
        m_client->m_host->GetBrowser()->GoBack();
        //browser->GoBack();
    }

    bool CefView::CanGoForward()
    {
        return m_client->m_host->GetBrowser()->CanGoForward();
    }

    void CefView::GoForward()
    {
        m_client->m_host->GetBrowser()->GoForward();
    }

    winrt::Windows::UI::Color CefView::DefaultBackgroundColor()
    {
        return winrt::Windows::UI::Colors::White();
    }

    winrt::Windows::Foundation::Uri CefView::Source()
    {
        auto source = m_client->m_frame->GetURL();
        return winrt::Windows::Foundation::Uri{ source.c_str() };
    }

    void CefView::Source(winrt::Windows::Foundation::Uri value)
    {
        m_client->m_host->GetBrowser()->GetMainFrame()->LoadURL(value.ToString().data());
    }

    static std::string getDataURI(const std::string& data, const std::string& mime_type) {
        return "data:" + mime_type + ";base64," +
            CefURIEncode(CefBase64Encode(data.data(), data.size()), false)
            .ToString();
    }

    void CefView::NavigateToString(winrt::hstring htmlContent)
    {
        m_client->m_host->GetBrowser()->GetMainFrame()->LoadURL(getDataURI(winrt::to_string(htmlContent.data()), "text/html"));
    }

    void CefView::Close()
    {

    }

    winrt::Windows::Foundation::IAsyncOperation<winrt::hstring> CefView::ExecuteScriptAsync(winrt::hstring javascriptCode)
    {
        m_client->m_host->GetBrowser()->GetMainFrame()->ExecuteJavaScript(
            javascriptCode.data(),
            m_client->m_frame->GetURL(),
            0
        );
        co_return L"";
    }

    void CefView::createBrowser()
    {
        CefWindowInfo windowInfo;
        CefRect rect{ 0,0,(int)m_size.Width, (int)m_size.Height };
        m_cefHwnd == NULL ?
            windowInfo.SetAsPopup(NULL, "Winui3Cef") :
            windowInfo.SetAsChild(m_cefHwnd, rect);
        windowInfo.ex_style = WS_EX_TOPMOST;
        CefBrowserSettings browserSettings;

        m_client = new BrowserClient();
        CefBrowserHost::CreateBrowser(
            windowInfo,
            m_client,
            "https://www.baidu.com",
            browserSettings,
            nullptr,
            CefRequestContext::GetGlobalContext()
        );

    }

    void CefView::UserControl_Loaded(winrt::Windows::Foundation::IInspectable const& sender, winrt::Microsoft::UI::Xaml::RoutedEventArgs const& e)
    {
        Global::g_view = *this;
        CefMainArgs mainArgs{ GetModuleHandle(nullptr) };
        CefSettings settings{ GetSettings() };
        BrowserApp* app = new BrowserApp();
        CefInitialize(mainArgs, settings, app, nullptr) ?
            OutputDebugString(L"Cef initialized\n") :
            OutputDebugString(L"Cef initialize failed!!!!!!!!!!!!!!!!!!!!!\n");

        m_cefHwnd = CreateWindowEx(WS_EX_LAYERED, L"Static", L"", WS_VISIBLE | WS_CHILD, 0, 0, m_size.Width, m_size.Height,
            GetHwnd(Global::s_window), nullptr, nullptr, nullptr);

        createBrowser();
    }


    void CefView::UserControl_SizeChanged(
        winrt::Windows::Foundation::IInspectable const& sender, 
        winrt::Microsoft::UI::Xaml::SizeChangedEventArgs const& e)
    {
        auto size = e.NewSize();
        auto dpi = GetDpiForWindow(GetDesktopWindow());
        m_size.Width = size.Width * dpi / 96;
        m_size.Height = size.Height * dpi / 96;
        //SetWindowRgn(m_cefHwnd, toHRGN(m_size), true);
        //
        if (m_client && m_client->m_host)
        {
            auto handle = m_client->m_host->GetWindowHandle();
            //SetWindowPos(m_cefHwnd, NULL, getOrigin().X, getOrigin().Y, m_size.Width, m_size.Height, 0);
            SetWindowPos(handle, NULL, getOrigin().X, getOrigin().Y, m_size.Width, m_size.Height, SWP_NOOWNERZORDER);

        }
        std::thread{
            [this] {
                std::this_thread::sleep_for(std::chrono::seconds{1});
                //SetWindowPos(GetHwnd(Global::s_window), 0, 0, 0, 0, 0, SWP_NOSIZE);
                SetWindowPos(m_cefHwnd, NULL, 0, 0, 0, 0, SWP_NOSIZE);
                m_webMessageReceivedEvent(*this, L"some text");
            }
        }.detach();
    }

    //void CefView::CanLoad()
    //{

    //}

    //winrt::event_token CefView::WebMessageReceived(Windows::Foundation::EventHandler<winrt::hstring> const& handler)
    //{
    //    return m_webMessageReceivedEvent.add(handler);
    //}

    //void CefView::WebMessageReceived(winrt::event_token const& token) noexcept
    //{
    //    m_webMessageReceivedEvent.remove(token);
    //}

    //void CefView::ExecuteScript(winrt::hstring script)
    //{   
    //    if (m_client && m_client->m_frame)
    //    {
    //        m_client->m_frame->ExecuteJavaScript(script.data(), m_client->m_frame->GetURL(), 0);
    //    }
    //}

    HRGN CefView::toHRGN(winrt::Windows::Foundation::Size size)
    {
        auto const origin = getOrigin();
        return CreateRectRgn(
            origin.X, origin.Y,
            origin.X + size.Width, origin.Y + size.Width
        );
    }

    winrt::Windows::Foundation::Point CefView::getOrigin()
    {
        return winrt::Windows::Foundation::Point{};
    }

}



