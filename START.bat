@echo off
setlocal
title HozorPlus Server
cd /d "%~dp0"

if not exist ".next\standalone\server.js" (
    echo [ERROR] The application is not installed yet.
    echo         Run INSTALL.bat first.
    pause
    exit /b 1
)
if not exist ".env" (
    echo [ERROR] .env not found. Run INSTALL.bat first.
    pause
    exit /b 1
)

node scripts\start-windows.js
echo.
echo HozorPlus stopped.
pause
