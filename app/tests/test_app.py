import pytest
from src.ofuscar_dados import ObfuscateJson


def test_empty_names():
    censura = ObfuscateJson(set())
    obj = {"nome": "João Silva", "idade": 30, "cidade": "São Paulo"}
    expected = '{"cidade": "São Paulo", "idade": 30, "nome": "João Silva"}'
    assert censura.to_json(obj) == expected


def test_censor_name():
    censura = ObfuscateJson({"nome"})
    obj = {"nome": "João Silva", "idade": 30, "cidade": "São Paulo"}
    expected = '{"cidade": "São Paulo", "idade": 30, "nome": "oculto"}'
    assert censura.to_json(obj) == expected


def test_censor_nested():
    censura = ObfuscateJson({"endereco"})
    obj = {"nome": "João Silva", "idade": 30, "cidade": "São Paulo",
           "endereco": {"rua": "Av. Paulista", "numero": 1000}}
    expected = '{"cidade": "São Paulo", "endereco": {"numero": 1000, "rua": "oculto"}, "idade": 30, "nome": "João Silva"}'
    assert censura.to_json(obj) == expected


def test_censor_list():
    censura = ObfuscateJson({"nome"})
    obj = [{"nome": "João Silva", "idade": 30},
           {"nome": "Maria Santos", "idade": 25}]
    expected = '[{"idade": 30, "nome": "oculto"}, {"idade": 25, "nome": "oculto"}]'
    assert censura.to_json(obj) == expected
