@echo off
set arg1=%1
set arg2=%2
set arg3=%3
echo =================================
echo CloudMoe DotNet Core Build Script
echo =================================
if defined arg1 (
    if "%arg1%"=="-?" (
        goto usage
    )
    if "%arg1%"=="-h" (
        goto usage
    )
    if "%arg1%"=="--help" (
        goto usage
    )
    if "%arg1%"=="/?" (
        goto usage
    )
    if "%arg1%"=="/h" (
        goto usage
    )
    if "%arg1%"=="/help" (
        goto usage
    )
    if defined arg2 (
        set PublishSingleFile=false
        set PublishTrimmed=false
        if "%arg3%"=="s" (
            set PublishSingleFile=true
            echo 111
        )
        if "%arg3%"=="t" (
            set PublishTrimmed=true
        )
        if "%arg3%"=="st" (
            set PublishSingleFile=true
            set PublishTrimmed=true
        )
        if "%arg3%"=="ts" (
            set PublishSingleFile=true
            set PublishTrimmed=true
        )
    ) else (
        goto usage
    )
) else (
    set PublishSingleFile=true
    set PublishTrimmed=true
    set arg1=win-x86
    set arg2=release
)
echo Build Information
echo =================================
echo SingleFile: %PublishSingleFile%
echo Trimmed: %PublishTrimmed%
echo Runtime: %arg1%
echo Configuration: %arg2%
echo =================================
::@echo on
dotnet publish -r %arg1% -c %arg2% /p:PublishSingleFile=%PublishSingleFile% /p:PublishTrimmed=%PublishTrimmed%
echo =================================
echo Build Done
echo =================================
goto end
::pause
:usage
echo Please read usage first
echo =================================
echo Usage:
echo build.bat ^<runtime^>^{^(win^|linux^|linux-musl^|rhel^|tizen^|osx^)-^(x86^|x64^|arm^|arm64^)^} ^<configuration^>^{release^|debug^} ^[s^|t^|st^|ts^]^{s:SingleFile^|t:Trimmed^}
echo Default example:
echo build.bat win-x86 release st
echo =================================
:end
