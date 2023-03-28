module;
#include "pch.h"
#include "include/cef_app.h"
#include "include/cef_browser_process_handler.h"
#include "include/cef_browser.h"
#include "CefAppBase.hpp"
export module internal;


export class BrowserApp : 
	public WinUICefAppBase, 
	public CefBrowserProcessHandler
{
	IMPLEMENT_REFCOUNTING(BrowserApp);
public:
	virtual CefRefPtr<CefBrowserProcessHandler> GetBrowserProcessHandler() override
	{
		return this;
	}
};

export class BrowserClient :
	public CefClient,
	public CefDisplayHandler,
	public CefLifeSpanHandler
{
	IMPLEMENT_REFCOUNTING(BrowserClient);
public:
	BrowserClient() {}

	virtual CefRefPtr<CefDisplayHandler> GetDisplayHandler() override
	{
		return this;
	}

	virtual CefRefPtr<CefLifeSpanHandler> GetLifeSpanHandler() override
	{
		return this;
	}

	virtual bool OnAutoResize(CefRefPtr<CefBrowser> browser, const CefSize& new_size) override
	{
		return false;
	}

	virtual void OnAfterCreated(CefRefPtr<CefBrowser> browser) override
	{
		m_browser = browser;
		m_host = m_browser->GetHost();
		CefWindowInfo info{};
		info.bounds.width = 800;
		info.bounds.height = 600;
		CefBrowserSettings settings{};
		m_host->ShowDevTools(info, nullptr, settings, {});
		OutputDebugString(L"Browser created\n");
		m_frame = m_browser->GetMainFrame();
	}

	CefRefPtr<CefBrowser> m_browser{ nullptr };
	CefRefPtr<CefBrowserHost> m_host{ nullptr };
	CefRefPtr<CefFrame> m_frame{ nullptr };
};