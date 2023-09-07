#!/bin/bash
if [[ "$OSTYPE" == "linux-gnu"* ]]; then
    rm -rf .venv
elif [[ "$OSTYPE" == "darwin"* ]]; then
    rm -rf .venv
elif [[ "$OSTYPE" == "win32" ]]; then
    rmdir /s /q .venv
else
    echo "Unknown OS"
fi
