#!/bin/bash

# Verifica se há commits não enviados ou arquivos modificados, adicionados ou excluídos na branch atual
if [[ -n $(git log --name-status --diff-filter=ADM origin/$(git rev-parse --abbrev-ref HEAD)..HEAD) ]]; then
    # Se houver, executa o comando gradlew clean build
    ./gradlew clean build
fi
