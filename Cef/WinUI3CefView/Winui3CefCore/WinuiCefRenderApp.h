#pragma once
#include "../Winui3Cef/CefAppBase.hpp"

class WinuiCefRenderApp : public WinUICefAppBase, CefRenderProcessHandler
{
public:
	WinuiCefRenderApp() {}

	IMPLEMENT_REFCOUNTING(WinuiCefRenderApp);
};

