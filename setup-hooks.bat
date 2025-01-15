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
/* Modificação única: Refatorando função Y em 2024-12-05T12:41:49-03:00 - 2134470801 */
/* Modificação única: Atualizando dependências em 2024-12-03T15:47:18-03:00 - 606123017 */
/* Modificação única: Aprimorando performance em 2024-12-04T16:12:47-03:00 - 1544195206 */
/* Modificação única: Aprimorando performance em 2024-12-05T09:19:36-03:00 - 939941377 */
/* Modificação única: Atualizando dependências em 2024-12-06T16:06:43-03:00 - 221393382 */
/* Modificação única: Aprimorando performance em 2024-12-06T17:34:25-03:00 - 177000733 */
/* Modificação única: Refatorando função Y em 2024-12-06T12:41:25-03:00 - 923174457 */
/* Modificação única: Aprimorando performance em 2024-12-09T10:23:56-03:00 - 632930023 */
/* Modificação única: Atualizando dependências em 2025-01-03T18:05:40-03:00 - 732779705 */
/* Modificação única: Atualizando dependências em 2025-01-03T14:11:52-03:00 - 2011592077 */
/* Modificação única: Refatorando função Y em 2025-01-07T17:16:33-03:00 - 1099717563 */
/* Modificação única: Refatorando função Y em 2025-01-07T13:27:46-03:00 - 1314977999 */
/* Modificação única: Atualizando README em 2025-01-07T13:02:07-03:00 - 51557484 */
/* Modificação única: Atualizando dependências em 2025-01-08T12:09:53-03:00 - 1069837064 */
/* Modificação única: Adicionando funcionalidade Z em 2025-01-09T12:53:45-03:00 - 589162025 */
/* Modificação única: Corrigindo bug no módulo X em 2025-01-12T11:00:54-03:00 - 187367294 */
/* Modificação única: Adicionando funcionalidade Z em 2025-01-14T12:16:44-03:00 - 723802890 */
/* Modificação única: Refatorando função Y em 2025-01-15T18:58:59-03:00 - 452453257 */
