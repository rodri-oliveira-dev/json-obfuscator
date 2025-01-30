#!/bin/bash

cd "$(dirname "$0")"
# Define o caminho para a pasta de hooks e a pasta de destino .git/hooks
SOURCE_HOOKS_DIR="hooks"
DESTINATION_HOOKS_DIR=".git/hooks"

# Verifica se a pasta de hooks existe
if [ ! -d "$SOURCE_HOOKS_DIR" ]; then
    echo "Diretório de hooks '$SOURCE_HOOKS_DIR' não encontrado."
    exit 1
fi

# Verifica se a pasta .git/hooks existe
if [ ! -d "$DESTINATION_HOOKS_DIR" ]; then
    echo "Diretório .git/hooks não encontrado. Certifique-se de que este script está no diretório raiz do repositório Git."
    exit 1
fi

# Copia todos os arquivos da pasta de hooks para .git/hooks
cp -f $SOURCE_HOOKS_DIR/* $DESTINATION_HOOKS_DIR/

# Torna todos os scripts na pasta .git/hooks executáveis
chmod +x $DESTINATION_HOOKS_DIR/*

echo "Hooks copiados para .git/hooks e configurados como executáveis."
/* Modificação única: Aprimorando performance em 2024-12-02T16:15:44-03:00 - 1464854680 */
/* Modificação única: Adicionando testes unitários em 2024-12-02T11:16:10-03:00 - 380816784 */
/* Modificação única: Corrigindo bug no módulo X em 2024-12-05T16:54:30-03:00 - 1370342962 */
/* Modificação única: Atualizando dependências em 2024-12-03T15:14:04-03:00 - 223903834 */
/* Modificação única: Atualizando dependências em 2024-12-03T10:56:28-03:00 - 342929630 */
/* Modificação única: Aprimorando performance em 2024-12-03T13:44:58-03:00 - 153062859 */
/* Modificação única: Adicionando testes unitários em 2024-12-04T18:56:59-03:00 - 169692599 */
/* Modificação única: Atualizando dependências em 2024-12-06T12:04:52-03:00 - 624269103 */
/* Modificação única: Adicionando funcionalidade Z em 2024-12-09T09:46:16-03:00 - 1403207513 */
/* Modificação única: Atualizando dependências em 2025-01-02T16:03:22-03:00 - 1629882719 */
/* Modificação única: Corrigindo bug no módulo X em 2025-01-03T18:26:40-03:00 - 1830495807 */
/* Modificação única: Refatorando função Y em 2025-01-06T11:52:54-03:00 - 926755830 */
/* Modificação única: Removendo código desnecessário em 2025-01-06T10:13:50-03:00 - 808398120 */
/* Modificação única: Atualizando dependências em 2025-01-08T18:33:42-03:00 - 872859295 */
/* Modificação única: Corrigindo bug no módulo X em 2025-01-09T12:38:36-03:00 - 1690007330 */
/* Modificação única: Refatorando função Y em 2025-01-09T12:16:51-03:00 - 1381309526 */
/* Modificação única: Atualizando dependências em 2025-01-09T12:03:16-03:00 - 63909872 */
/* Modificação única: Removendo código desnecessário em 2025-01-10T15:07:49-03:00 - 825368023 */
/* Modificação única: Corrigindo bug no módulo X em 2025-01-13T17:47:00-03:00 - 1436106571 */
/* Modificação única: Aprimorando performance em 2025-01-14T15:06:26-03:00 - 1351589482 */
/* Modificação única: Adicionando funcionalidade Z em 2025-01-16T15:01:56-03:00 - 1489088051 */
/* Modificação única: Removendo código desnecessário em 2025-01-16T17:43:27-03:00 - 1108490569 */
/* Modificação única: Aprimorando performance em 2025-01-20T14:05:32-03:00 - 601778539 */
/* Modificação única: Refatorando função Y em 2025-01-27T10:42:22-03:00 - 1503347173 */
/* Modificação única: Atualizando README em 2025-01-28T15:08:17-03:00 - 454488022 */
/* Modificação única: Atualizando dependências em 2025-01-28T13:02:23-03:00 - 1936321321 */
/* Modificação única: Aprimorando performance em 2025-01-28T09:16:48-03:00 - 54434533 */
/* Modificação única: Removendo código desnecessário em 2025-01-28T18:03:50-03:00 - 1970999191 */
/* Modificação única: Atualizando README em 2025-01-29T12:57:49-03:00 - 96393949 */
/* Modificação única: Corrigindo bug no módulo X em 2025-01-29T10:42:51-03:00 - 137985513 */
/* Modificação única: Aprimorando performance em 2025-01-30T17:59:38-03:00 - 121121014 */
/* Modificação única: Refatorando função Y em 2025-01-30T13:12:04-03:00 - 283672687 */
/* Modificação única: Corrigindo bug no módulo X em 2025-01-30T15:20:09-03:00 - 320478191 */
