import json

import pytest
from src.services.ofuscar_dados_service import OfuscarDadosService


def test_deve_criptografar_os_campos_email_e_password():

    # Arrange
    mascara = "********"
    campos = ['email', 'password']
    data = json.loads(open("./tests/to_obfuscate_example.json", "rb").read())
    # Act
    ofuscador = OfuscarDadosService(mascara, campos)
    retorno = ofuscador.ofuscar_dados(data)
    # Assert
    assert retorno["entities"][0]["personalInformation"]["email"] == mascara
    assert retorno["entities"][0]["personalInformation"]["password"] == mascara


def test_deve_resultar_em_exception_ao_nao_informar_campos_ofuscacao():
    with pytest.raises(Exception) as e_info:
        # Arrange
        campos = []
        data = json.loads(
            open("./tests/to_obfuscate_example.json", "rb").read())
        # Act
        ofuscador = OfuscarDadosService("")
        ofuscador.ofuscar_dados(data, campos)
        # Assert
        assert str(
            e_info.value) == "Não foram informadas os campos para ofuscação"


def test_deve_resultar_em_exception_ao_nao_informar_json():
    with pytest.raises(Exception) as e_info:
        # Arrange
        campos = ['email', 'password']
        data = []
        # Act
        ofuscador = OfuscarDadosService("")
        ofuscador.ofuscar_dados(data, campos)
        # Assert
        assert str(
            e_info.value) == "JSON não foi informado"
