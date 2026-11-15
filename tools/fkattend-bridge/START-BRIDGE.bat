@echo off
setlocal
title HozorPlus FKAttend Bridge (x86)
cd /d "%~dp0"

REM ---- default DLL dir: this folder; override with the 1st argument ----
set "DLLDIR=%~dp0"
if not "%~1"=="" set "DLLDIR=%~1"

REM ---- optional overrides: BRIDGE_PORT, BRIDGE_TOKEN ----
set "BRIDGE_PORT=%BRIDGE_PORT%"
if "%BRIDGE_PORT%"=="" set "BRIDGE_PORT=8765"

echo ==========================================================
echo   HozorPlus FKAttend Bridge  (32-bit, READ-ONLY)
echo   DLL folder: %DLLDIR%
echo   Listening:  http://127.0.0.1:%BRIDGE_PORT%   (localhost only)
echo ==========================================================
echo   Keep this window OPEN while HozorPlus uses the device.
echo   Stop: close this window or press Ctrl+C.
echo.

REM FKAttend.dll is 32-bit → MUST use the 32-bit PowerShell (SysWOW64).
"%WINDIR%\SysWOW64\WindowsPowerShell\v1.0\powershell.exe" -NoProfile -ExecutionPolicy Bypass -File "%~dp0fkattend-bridge.ps1" -Port %BRIDGE_PORT% -DllDir "%DLLDIR%"

echo.
echo Bridge stopped.
pause
