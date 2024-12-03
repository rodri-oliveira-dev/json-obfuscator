@echo off
cd %~dp0
REM Define o caminho para a pasta de hooks e a pasta de destino .git/hooks
SET SOURCE_HOOKS_DIR=hooks
SET DESTINATION_HOOKS_DIR=.git\hooks

REM Verifica se a pasta de hooks existe
IF NOT EXIST "%SOURCE_HOOKS_DIR%" (
    echo Diretorio de hooks '%SOURCE_HOOKS_DIR%' nao encontrado.
    exit /b 1
)

REM Verifica se a pasta .git/hooks existe
IF NOT EXIST "%DESTINATION_HOOKS_DIR%" (
    echo Diretorio .git/hooks nao encontrado. Certifique-se de que este script esta no diretorio raiz do repositorio Git.
    exit /b 1
)

REM Copia todos os arquivos da pasta de hooks para .git/hooks
xcopy /Y /I "%SOURCE_HOOKS_DIR%\*" "%DESTINATION_HOOKS_DIR%\"

echo Hooks copiados para .git/hooks.
/* Modificação única: Atualizando dependências em 2024-12-02T11:51:56-03:00 - 1663905552 */
/* Modificação única: Refatorando função Y em 2024-12-03T12:31:58-03:00 - 1948056984 */
