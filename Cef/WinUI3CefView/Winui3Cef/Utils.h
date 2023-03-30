#pragma once
#include <include/internal/cef_types.h>

namespace Utils
{
	enum class CefThreadKind
	{
		UI = TID_UI,
		FileBackground = TID_FILE_BACKGROUND,
		FileUserVisible = TID_FILE_USER_VISIBLE,
		FileUserBlocking = TID_FILE_USER_BLOCKING,
		ProcessLauncher = TID_PROCESS_LAUNCHER,
		IO = TID_IO,
		Renderer = TID_RENDERER,
		Other
	};

	CefThreadKind GetCefThreadKind();
}