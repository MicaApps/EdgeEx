#include "pch.h"
#include "Utils.h"
#include <include/cef_task.h>

namespace Utils
{
	CefThreadKind Utils::GetCefThreadKind()
	{
		static_assert(TID_UI == 0);
		static_assert(TID_RENDERER == 6);
		for (auto tid = 0; tid <= TID_RENDERER; ++tid)
		{
			if(CefCurrentlyOn(static_cast<CefThreadId>(tid)))
				return static_cast<CefThreadKind>(tid);
		}
		return CefThreadKind::Other;
	}

}