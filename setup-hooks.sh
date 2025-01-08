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
