@echo off
setlocal EnableDelayedExpansion
title HozorPlus - Installer
cd /d "%~dp0"

REM ---- resolve install folder (parent of this script) ----
for %%I in ("%~dp0.") do set "ROOT=%%~fI"

REM ---- app version comes from package.json (single source of truth — never hardcode) ----
set "APPVER="
for /f "delims=" %%v in ('node -p "require('./package.json').version"') do set "APPVER=%%v"
if not defined APPVER set "APPVER=?"

REM ---- SQLite URL with forward slashes and %20 for spaces ----
set "DBURL=file:%ROOT%\db\custom.db"
set "DBURL=!DBURL:\=/!"
set "DBURL=!DBURL: =%%20!"
set "DATABASE_URL=!DBURL!"

echo ==========================================================
echo   HozorPlus  -  Attendance Management System  v!APPVER!
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
echo [1/8] Checking Node.js ...
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
    echo   [!] Node.js v!NODEMAJ! detected - version 20 or newer is required.
    echo       Please update Node.js from https://nodejs.org and run again.
    pause
    exit /b 1
)
for /f "delims=" %%v in ('node -v') do echo   [OK] Node.js %%v

REM ================== 2) packages ==================
echo.
echo [2/8] Installing npm packages  (5-10 minutes, one time only) ...
REM --ignore-scripts=false: even if the machine's npm policy disables install
REM scripts, the native dependencies (SWC, esbuild, sharp, Prisma engines) MUST
REM be allowed to install properly.
call npm install --no-audit --no-fund --ignore-scripts=false
if errorlevel 1 goto :fail

echo.
echo [2/8] Verifying native dependencies ...
node scripts\verify-native.js
if errorlevel 1 (
    echo   [!] A critical native dependency is missing - see the message above.
    echo       Fix the npm scripts policy and run INSTALL.bat again.
    goto :fail
)

REM ================== 3) prisma client ==================
echo.
echo [3/8] Generating Prisma client ...
call npx prisma generate
if errorlevel 1 goto :fail

REM ================== 4) database schema ==================
echo.
echo [4/8] Creating SQLite database schema ...
if not exist "%ROOT%\db" mkdir "%ROOT%\db"
call npx prisma db push --skip-generate
if errorlevel 1 goto :fail

REM ================== 5) .env (PRESERVE existing variables) ==================
echo.
echo [5/8] Writing .env (existing variables and secrets are preserved) ...
node scripts\env-ensure.js "DATABASE_URL=!DBURL!"
if errorlevel 1 goto :fail

REM ================== 6) database init ==================
REM Phase 1 rules:
REM   * database content is detected from REAL row counts - NEVER from the
REM     file size (a small file is NOT an empty database)
REM   * an existing database is KEPT by default
REM   * reset/demo-seed requires explicit confirmation and takes a backup first
echo.
echo [6/8] Initializing database ...
set "DB_STATE="
for /f "usebackq delims=" %%L in (`call npx tsx scripts\db-admin.ts status 2^>^&1 ^| findstr /B "DB_STATE="`) do set "%%L"
if not defined DB_STATE (
    echo   [ERROR] Database status could not be read.
    goto :fail
)
echo   Database state: !DB_STATE!

if "!DB_STATE!"=="DATA" (
    echo.
    echo   An existing database was found ^(persons / punches / attendance^).
    echo   Default: your data is KEPT - nothing is deleted.
    set "ASK="
    set /p "ASK=   Reset the database and load fresh DEMO data? [y/N]: "
    if /i "!ASK!"=="y" (
        call npx tsx scripts\db-admin.ts seed --yes
        if errorlevel 1 goto :fail
        set "DBMODE=DEMO"
    ) else (
        echo   Keeping the existing data untouched.
        set "DBMODE=KEEP"
    )
) else (
    echo.
    echo   Choose the initial database content:
    echo     [1] Empty database  ^(recommended - schema ready, no sample data^)
    echo     [2] Demo data       ^(sample organization for trying the software^)
    set "DBMODE="
    set /p "DBMODE=   Enter 1 or 2 [default 1]: "
    if "!DBMODE!"=="2" (
        call npx tsx scripts\db-admin.ts seed --yes
        if errorlevel 1 goto :fail
        set "DBMODE=DEMO"
    ) else (
        call npx tsx scripts\db-admin.ts ensure-admin
        if errorlevel 1 goto :fail
        set "DBMODE=EMPTY"
    )
)

REM ================== 7) build (serialized, lock-guarded) ==================
echo.
echo [7/8] Building production bundle  (a few minutes) ...
node scripts\build-with-lock.js
if errorlevel 1 goto :fail

REM ================== 8) done ==================
echo.
echo ==========================================================
echo   INSTALLATION COMPLETE  -  HozorPlus v!APPVER!
echo ==========================================================
echo.
echo   1.  Double-click  START.bat  to run the application
echo   2.  The browser opens at  http://localhost:3000
if "!DBMODE!"=="DEMO" (
    echo   3.  Demo login:  admin / admin123      ^(more in README.md^)
) else if "!DBMODE!"=="EMPTY" (
    echo   3.  First login: open the file  db\FIRST-LOGIN.txt
    echo       ^(a random admin password was generated for you^)
) else (
    echo   3.  Login with your existing user accounts ^(data was preserved^)
)
echo.
echo   To stop the app: close the server window or run STOP.bat
echo   On first run Windows Firewall may ask for permission -
echo   click "Allow access" if you want LAN access from other PCs.
echo.
echo   Novin Pardaz devices (NP756+, NP746, NP770 ...):
echo     - register the device here with its SERIAL NUMBER
echo     - on the device: Communication menu -> Server address = THIS PC's IP
echo       Server port = 5005   (allow port 5005 in Windows Firewall)
echo     - attendance logs then stream in automatically, real-time.
echo.
echo   NP756+ via the official FK623 SDK (recommended, read-only):
echo     - the 7 vendor SDK DLLs (FK623Attend.dll + companions) are
echo       PRE-INSTALLED in  tools\fkattend-bridge\bin  - nothing to copy.
echo     - just add the device (IP + port 4238 + network password).
echo       The connection service starts and manages itself - no scripts,
echo       no windows, no tokens. See README.md for details.
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
echo     - if a previous installation is RUNNING, close it first
pause
exit /b 1
