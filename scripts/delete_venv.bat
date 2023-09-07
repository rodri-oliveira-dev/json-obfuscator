@echo off

rem Get the OS type
for /f "delims=" %%i in ('uname -a 2^>nul') do set "OS_TYPE=%%i"

if not defined OS_TYPE (
    rmdir /s /q .venv
    exit /b
)

echo Unknown OS
