@echo off

if not exist ".venv\" (
    python -m venv .venv
)

rem Assumindo que estamos em Windows se este script .bat está sendo executado
call .venv\Scripts\Activate

rem Instale os requirements
pip install -r requirements.txt

rem Saia se ocorrer um erro
if %errorlevel% neq 0 (
    echo An error occurred during the installation of the requirements.
    exit /b %errorlevel%
)

echo Done.
