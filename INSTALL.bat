@echo off
setlocal EnableDelayedExpansion
title HozorPlus - Installer
cd /d "%~dp0"

REM ---- resolve install folder (parent of this script) ----
for %%I in ("%~dp0.") do set "ROOT=%%~fI"

REM ---- SQLite URL with forward slashes and %20 for spaces ----
set "DBURL=file:%ROOT%\db\custom.db"
set "DBURL=!DBURL:\=/!"
set "DBURL=!DBURL: =%%20!"
set "DATABASE_URL=!DBURL!"

echo ==========================================================
echo   HozorPlus  -  Attendance Management System  v1.1.0
echo   Windows Installer
echo ==========================================================
echo.
echo   Install folder: %ROOT%
echo.

if not exist "%ROOT%\package.json" (
    echo [ERROR] package.json not found in this folder.
    echo         Extract the FULL zip archive first, then run INSTALL.bat.
    pause
    exit /b 1
)

echo   NOTE: a folder path WITHOUT spaces is recommended
echo         ^(e.g. C:\HozorPlus^). Internet is required once for setup.
echo.

REM ================== 1) Node.js ==================
echo [1/7] Checking Node.js ...
where node >nul 2>nul
if errorlevel 1 (
    echo.
    echo   Node.js was not found on this computer.
    echo   Trying automatic installation with winget ...
    where winget >nul 2>nul
    if errorlevel 1 (
        echo.
        echo   [!] winget is not available either.
        echo       Please download and install Node.js LTS from:
        echo       https://nodejs.org
        echo       then run INSTALL.bat again.
        echo.
        pause
        exit /b 1
    )
    winget install -e --id OpenJS.NodeJS.LTS --accept-package-agreements --accept-source-agreements
    where node >nul 2>nul
    if errorlevel 1 (
        echo.
        echo   [OK] Node.js was installed, but PATH needs a fresh console:
        echo        CLOSE this window and run INSTALL.bat once more.
        echo.
        pause
        exit /b 0
    )
)

for /f "tokens=1 delims=." %%a in ('node -p process.versions.node') do set "NODEMAJ=%%a"
if !NODEMAJ! LSS 20 (
    echo   [!] Node.js v!NODEMAJ! detected — version 20 or newer is required.
    echo       Please update Node.js from https://nodejs.org and run again.
    pause
    exit /b 1
)
for /f "delims=" %%v in ('node -v') do echo   [OK] Node.js %%v

REM ================== 2) packages ==================
echo.
echo [2/7] Installing npm packages  (5-10 minutes, one time only) ...
call npm install --no-audit --no-fund
if errorlevel 1 goto :fail

REM ================== 3) prisma client ==================
echo.
echo [3/7] Generating Prisma client ...
call npx prisma generate
if errorlevel 1 goto :fail

REM ================== 4) database ==================
echo.
echo [4/7] Creating SQLite database ...
if not exist "%ROOT%\db" mkdir "%ROOT%\db"
call npx prisma db push --skip-generate
if errorlevel 1 goto :fail

REM ================== 5) .env ==================
echo.
echo [5/7] Writing .env ...
> "%ROOT%\.env" echo DATABASE_URL=!DBURL!
echo   [OK] !DBURL!

REM ================== 6) demo data ==================
echo.
echo [6/7] Demo data ...
set "SEED=yes"
if exist "%ROOT%\db\custom.db" (
    for %%F in ("%ROOT%\db\custom.db") do set "DBSIZE=%%~zF"
    if !DBSIZE! GTR 100000 (
        set "SEED=skip"
        echo   An existing database was found ^(!DBSIZE! bytes^).
        set /p "ASK=   Reset it and load fresh demo data? [y/N]: "
        if /i "!ASK!"=="y" set "SEED=yes"
    )
)
if "!SEED!"=="yes" (
    call npx tsx scripts\seed.ts
    if errorlevel 1 goto :fail
) else (
    echo   Skipped — keeping existing data.
)

REM ================== 7) build ==================
echo.
echo [7/7] Building production bundle  (a few minutes) ...
call npm run build
if errorlevel 1 goto :fail

echo.
echo ==========================================================
echo   INSTALLATION COMPLETE
echo ==========================================================
echo.
echo   1.  Double-click  START.bat  to run the application
echo   2.  The browser opens at  http://localhost:3000
echo   3.  Login:  admin / admin123      ^(more in README.md^)
echo.
echo   To stop the app: close the server window or run STOP.bat
echo   On first run Windows Firewall may ask for permission —
echo   click "Allow access" if you want LAN access from other PCs.
echo.
echo   Novin Pardaz devices (NP756+, NP746, NP770 ...):
echo     - register the device here with its SERIAL NUMBER
echo     - on the device: Communication menu -> Server address = THIS PC's IP
echo       Server port = 5005   (allow port 5005 in Windows Firewall)
echo     - attendance logs then stream in automatically, real-time.
echo.
pause
exit /b 0

:fail
echo.
echo   [ERROR] Installation failed. See the messages above.
echo   Common fixes:
echo     - check your internet connection
echo     - extract to a short path without spaces ^(C:\HozorPlus^)
echo     - make sure Node.js 20+ is installed ^(node -v^)
pause
exit /b 1
