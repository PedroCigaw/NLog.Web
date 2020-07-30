@echo off

rem fallback if not passed
set version_prefix=4.9.2.1
set version_suffix=
set version_build=%version_prefix%

rem call :read_params %*

rem msbuild NLog.Web.sln /t:restore,rebuild /p:configuration=release /verbosity:minimal
rem IF ERRORLEVEL 1 EXIT /B 1

rem msbuild src\NLog.Web /t:rebuild,pack /p:configuration=release /verbosity:minimal /p:IncludeSymbols=true /p:SymbolPackageFormat=snupkg /p:VersionPrefix=%version_prefix% /p:FileVersion=%version_build% /p:VersionSuffix=%version_suffix%
rem IF ERRORLEVEL 1 EXIT /B 1

msbuild src\NLog.Web.AspNetCore /t:rebuild,pack /p:configuration=release /verbosity:minimal /p:IncludeSymbols=true /p:SymbolPackageFormat=snupkg /p:VersionPrefix=%version_prefix% /p:FileVersion=%version_build% /p:VersionSuffix=%version_suffix%
IF ERRORLEVEL 1 EXIT /B 1

rem read pass parameters by name
:read_params
if not %1/==/ (
    if not "%__var%"=="" (
        if not "%__var:~0,1%"=="-" (
            endlocal
            goto read_params
        )
        endlocal & set %__var:~1%=%~1
    ) else (
        setlocal & set __var=%~1
    )
    shift
    goto read_params
)
exit /B