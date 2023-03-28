#include <string>
#include <Windows.h>


class Env
{
	inline static std::string s_modulePath = []()
	{
		char path[MAX_PATH]{};
		GetModuleFileNameA(NULL, path, std::size(path));
		return std::string{ path };
	}();
public:
	//return path without trailing "\"
	static std::string CurrentDirPath()
	{
		return s_modulePath.substr(0, s_modulePath.rfind("\\"));
	}

	static std::string RenderProcessPath()
	{
		return CurrentDirPath() + R"(\Winui3CefCore.exe)";
	}

	static std::string ResourcePath()
	{
		return CurrentDirPath() + R"(\Resources)";
	}

	static std::string LocalesPath()
	{
		return CurrentDirPath() + R"(\locales)";
	}

	static std::string CachePath()
	{
		return CurrentDirPath() + R"(\cache)";
	}
};