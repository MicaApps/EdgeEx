#pragma once

#include <winrt/Microsoft.UI.Xaml.h>
#include "CefView.g.h"

struct Global
{
	inline static winrt::Microsoft::UI::Xaml::Window s_window{ nullptr };
	inline static winrt::Winui3Cef::CefView g_view{ nullptr };
};

