@echo off
setlocal
title HozorPlus FK623 Bridge - DEBUG ONLY (x86)
cd /d "%~dp0"

REM ============================================================
REM  v1.5.0: THIS WINDOW IS NEEDED ONLY FOR DEBUGGING.
REM
REM  Normal operation needs NO manual steps at all: HozorPlus
REM  starts, hides, supervises and stops its bridge by itself
REM  (docs/BRIDGE-SUPERVISION.md). The user never sees a token.
REM
REM  Use this launcher ONLY to observe the bridge console while
REM  investigating a problem (token is printed here ONCE and is
REM  different from the app-managed one).
REM ============================================================

REM ---- default DLL dir: .\bin (standard install location); override with the 1st argument ----
set "DLLDIR=%~dp0bin"
if not exist "%DLLDIR%" set "DLLDIR=%~dp0"
if not "%~1"=="" set "DLLDIR=%~1"

REM ---- optional overrides: BRIDGE_PORT, BRIDGE_TOKEN ----
set "BRIDGE_PORT=%BRIDGE_PORT%"
if "%BRIDGE_PORT%"=="" set "BRIDGE_PORT=8765"

echo ==========================================================
echo   HozorPlus FK623 Bridge  (32-bit, READ-ONLY) - DEBUG ONLY
echo   DLL folder: %DLLDIR%
echo   Listening:  http://127.0.0.1:%BRIDGE_PORT%   (localhost only)
echo ==========================================================
echo   NOTE: the app does NOT read this instance - it manages
echo   its own hidden bridge. Close this window when done.
echo.

REM FK623Attend.dll is 32-bit → MUST use the 32-bit PowerShell (SysWOW64).
"%WINDIR%\SysWOW64\WindowsPowerShell\v1.0\powershell.exe" -NoProfile -ExecutionPolicy Bypass -File "%~dp0fkattend-bridge.ps1" -Port %BRIDGE_PORT% -DllDir "%DLLDIR%"

echo.
echo Bridge stopped.
pause
