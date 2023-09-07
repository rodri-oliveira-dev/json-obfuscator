#!/bin/bash

if [[ ! -d ".venv" ]]; then
    python -m venv .venv
fi

# Ativação do venv varia dependendo do sistema
if [[ "$OSTYPE" == "linux-gnu"* ]] || [[ "$OSTYPE" == "darwin"* ]]; then
    source .venv/bin/activate
elif [[ "$OSTYPE" == "win32" ]]; then
    .venv\Scripts\Activate
else
    echo "Unknown OS"
    exit 1
fi

# Instale os requirements
pip install -r requirements*
