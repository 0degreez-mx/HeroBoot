@echo off
setlocal enabledelayedexpansion

:: Relaunch as admin if not already elevated
>nul 2>&1 net session
if %errorlevel% neq 0 (
    echo Requesting admin privileges...
    powershell -Command "Start-Process -FilePath '%~f0' -Verb RunAs"
    exit /b
)

echo Adding HeroBoot to system startup...

:: Get paths
set "BAT_PATH=%~dp0"
set "CMD_PATH=%BAT_PATH%HeroBoot.exe"
set "TEMPLATE_XML=%BAT_PATH%template.xml"
set "FINAL_XML=%BAT_PATH%HeroBootTask.xml"

set "SELECTED_TEMPLATE=%TEMPLATE_XML%"
:: Replace {{COMMAND_PATH}} in the selected XML template
powershell -Command "(Get-Content '%SELECTED_TEMPLATE%') -replace '\{\{COMMAND_PATH\}\}', '\"%CMD_PATH%\"' | Set-Content -Encoding Unicode '%FINAL_XML%'"

:: Create scheduled task
schtasks /create /tn "HeroBootTask" /xml "%FINAL_XML%" /f

echo.
echo Added successfully. HeroBoot will now start at boot.