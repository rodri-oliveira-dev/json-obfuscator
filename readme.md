## Sobre o Projeto

Projeto de estudo de python criando um ofuscador de campos de um arquivo JSON

## Para rodar local

1. Crie um ambiente para o projeto. abra o terminal na pasta raiz e digite:

```ps
python -m venv .venv
```

2. Com o ambiente criado restaure as dependencias, use o comando:

```ps
pip install -r requirements.txt
```

## Para rodar teste

Para rodar o teste basico digite:

```ps
pytest
```

Visualizar report:

```ps
pytest --cov=src tests/ --cov-report html
```
