#!/bin/bash
if [[ "$OSTYPE" == "linux-gnu"* ]]; then
    source .venv/bin/activate
elif [[ "$OSTYPE" == "darwin"* ]]; then
    source .venv/bin/activate
elif [[ "$OSTYPE" == "win32" ]]; then
    .venv\Scripts\Activate
else
    echo "Unknown OS"
fi
