#include <include/cef_app.h>
#include <Windows.h>
#include "../Winui3Cef/CefAppBase.hpp"
#include "WinuiCefRenderApp.h"
#include "WinuiCefOtherApp.h"
#include <include/cef_version.h>

int WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nShowCmd)
{
#if CEF_VERSION_MAJOR <= 110
	//see this commit:https://bitbucket.org/chromiumembedded/cef/commits/f3b570cf8e3a633f585f5a9cd19c6f7fff42266e
	CefEnableHighDPISupport();
#endif

	CefMainArgs mainArgs{ hInstance };
	
	CefRefPtr<CefApp> app;
	auto commandLine = CefCommandLine::CreateCommandLine();
	commandLine->InitFromString(GetCommandLineW());
	switch (WinUICefAppBase::GetProcessType(commandLine))
	{
		case WinUICefAppBase::ProcessType::Renderer: app = new WinuiCefRenderApp();	break;
		case WinUICefAppBase::ProcessType::Other:	app = new WinuiCefOtherApp();	break;
		default: exit(-1);
	}
	return CefExecuteProcess(mainArgs, app, nullptr);
}