@echo off
title HozorPlus - Stop
echo Stopping HozorPlus ...
taskkill /f /t /fi "WINDOWTITLE eq HozorPlus Server*" >nul 2>nul
taskkill /f /t /fi "WINDOWTITLE eq HozorPlus - Installer*" >nul 2>nul
echo Done.
timeout /t 2 >nul
