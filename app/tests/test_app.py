import pytest
import json

from src.ofuscar_dados import ofuscar


def test_ofuscar_string():
    dados = ['senha']
    ofusc = '{"nome": "João", "senha": "1234"}'
    resultado = ofuscar(dados, ofusc, in_place=False)
    assert resultado == {'nome': 'João', 'senha': 'oculto'}

def test_ofuscar_dict():
    dados = ['senha']
    ofusc = {"nome": "João", "senha": "1234"}
    resultado = ofuscar(dados, ofusc, in_place=False)
    assert resultado == {'nome': 'João', 'senha': 'oculto'}

def test_ofuscar_nested():
    dados = ['senha', 'cartao']
    ofusc = '{"nome": "João", "senha": "1234", "cartao": {"numero": "1111-2222-3333-4444", "cvv": "123"}}'
    resultado = ofuscar(dados, ofusc, in_place=False)
    assert resultado == {'nome': 'João', 'senha': 'oculto', 'cartao': {'numero': 'oculto', 'cvv': 'oculto'}}

def test_ofuscar_list():
    dados = ['senha', 'cartao']
    ofusc = '[{"nome": "João", "senha": "1234", "cartao": {"numero": "1111-2222-3333-4444", "cvv": "123"}}]'
    resultado = ofuscar(dados, ofusc, in_place=False)
    assert resultado == [{'nome': 'João', 'senha': 'oculto', 'cartao': {'numero': 'oculto', 'cvv': 'oculto'}}]

def test_ofuscar_invalid_json():
    dados = ['senha']
    ofusc = '{"nome": "João", "senha": "1234"'
    with pytest.raises(ValueError):
        ofuscar(dados, ofusc, in_place=False)

def test_ofuscar_unsupported_type():
    dados = ['senha']
    ofusc = '{"nome": "João", "senha": set([1, 2, 3])}'
    ofusc = json.loads(json.dumps(eval(ofusc)))
    resultado = ofuscar(dados, ofusc, in_place=False)
    assert resultado == {'nome': 'João', 'senha': set([1, 2, 3])}
