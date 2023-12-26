# function GetCefDir()
# {
#     $dir = Get-ChildItem -Filter "cef_binary_*"
#     return $dir[0].FullName
# }

winget install Kitware.CMake
curl.exe 'https://cef-builds.spotifycdn.com/cef_binary_120.1.10%2Bg3ce3184%2Bchromium-120.0.6099.129_windows64.tar.bz2' --output cef.tar.bz2
.\bzip2.exe -d .\cef.tar.bz2
tar -xf ".\cef.tar" --directory CefBase
cd CefBase
$cefDir = $(Get-ChildItem -Filter "cef_binary_*")[0].FullName
$buildDir = "$cefDir\build"

mkdir $buildDir
cd $buildDir

$cmake = "$env:programfiles\CMake\bin\cmake.exe"
& ${cmake} ".."
& ${cmake} "--build" "."

cd $cefDir
cd ..
Rename-Item $cefDir cef_source