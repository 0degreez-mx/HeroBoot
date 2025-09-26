@echo off

:: Relaunch as admin if not already elevated
>nul 2>&1 net session
if %errorlevel% neq 0 (
    echo Requesting admin privileges...
    powershell -Command "Start-Process -FilePath '%~f0' -Verb RunAs"
    exit /b
)

echo Removing HeroBoot from startup...

echo Deleting HeroBootTask task...
schtasks /delete /tn "HeroBootTask" /f
echo HeroBoot removed successfully.