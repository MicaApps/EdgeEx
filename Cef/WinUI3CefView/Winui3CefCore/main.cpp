#include <include/cef_app.h>
#include <Windows.h>
#include "../Winui3Cef/CefAppBase.hpp"
#include "WinuiCefRenderApp.h"
#include "WinuiCefOtherApp.h"

int WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nShowCmd)
{
	CefEnableHighDPISupport();

	CefMainArgs mainArgs{ hInstance };
	
	CefRefPtr<CefApp> app;
	auto commandLine = CefCommandLine::CreateCommandLine();
	commandLine->InitFromString(GetCommandLineW());
	switch (WinUICefAppBase::GetProcessType(commandLine))
	{
		case WinUICefAppBase::ProcessType::Renderer: app = new WinuiCefRenderApp();	break;
		case WinUICefAppBase::ProcessType::Other:	app = new WinuiCefOtherApp();	break;
		default: throw std::runtime_error{ "Unknown process" };
	}
	return CefExecuteProcess(mainArgs, app, nullptr);
}