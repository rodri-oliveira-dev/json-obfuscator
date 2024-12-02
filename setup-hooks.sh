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
