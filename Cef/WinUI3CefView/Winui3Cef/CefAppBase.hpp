#pragma once
#include <include/cef_app.h>


namespace Constant
{
	constexpr auto ProcessType = "type";
	constexpr auto RendererProcess = "renderer";
}

class WinUICefAppBase : public CefApp
{
public:
	WinUICefAppBase() {}
	enum class ProcessType
	{
		Browser,
		Renderer,
		Other
	};

	static ProcessType GetProcessType(CefRefPtr<CefCommandLine> commandLine)
	{
		if (!commandLine->HasSwitch(Constant::ProcessType))
			return ProcessType::Browser;

		if (commandLine->GetSwitchValue(Constant::ProcessType) == Constant::RendererProcess)
			return ProcessType::Renderer;

		return ProcessType::Other;
	}

	DISALLOW_COPY_AND_ASSIGN(WinUICefAppBase);
};
