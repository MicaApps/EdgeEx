# function GetCefDir()
# {
#     $dir = Get-ChildItem -Filter "cef_binary_*"
#     return $dir[0].FullName
# }

winget install Kitware.CMake
curl.exe 'https://cef-builds.spotifycdn.com/cef_binary_110.0.31%2Bg46651e0%2Bchromium-110.0.5481.179_windows64.tar.bz2' --output cef.tar.bz2
tar -xf .\cef.tar.bz2 --directory CefBase
cd CefBase
$cefDir = $(Get-ChildItem -Filter "cef_binary_*")[0].FullName
$buildDir = "$cefDir\build"

mkdir $buildDir
cd $buildDir

$cmake = "$env:programfiles\CMake\bin\cmake.exe"
& ${cmake} ".."
& ${cmake} "--build" "."