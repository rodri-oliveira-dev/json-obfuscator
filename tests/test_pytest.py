import json
import pytest
from src.ofuscacao import OfuscarDados


def test_deve_criptografar_os_campos_email_e_password():

    # Arrange
    mascara = "********"
    campos = ['email', 'password']
    data = json.loads(open("./tests/to_obfuscate_example.json", "rb").read())
    # Act
    ofuscador = OfuscarDados(mascara)
    retorno = ofuscador.ofuscar_dados(data, campos)
    # Assert
    assert retorno["entities"][0]["personalInformation"]["email"] == mascara
    assert retorno["entities"][0]["personalInformation"]["password"] == mascara


def test_deve_usar_mascara_padrao_quando_nao_informado():

    # Arrange
    campos = ['email', 'password']
    data = json.loads(open("./tests/to_obfuscate_example.json", "rb").read())
    # Act
    ofuscador = OfuscarDados("")
    retorno = ofuscador.ofuscar_dados(data, campos)
    # Assert
    assert retorno["entities"][0]["personalInformation"]["email"] == OfuscarDados.MASCARA_PADRAO
    assert retorno["entities"][0]["personalInformation"]["password"] == OfuscarDados.MASCARA_PADRAO


def test_deve_resultar_em_exception_ao_nao_informar_campos_ofuscacao():
    with pytest.raises(Exception) as e_info:
        # Arrange
        campos = []
        data = json.loads(
            open("./tests/to_obfuscate_example.json", "rb").read())
        # Act
        ofuscador = OfuscarDados("")
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
        ofuscador = OfuscarDados("")
        ofuscador.ofuscar_dados(data, campos)
        # Assert
        assert str(
            e_info.value) == "JSON não foi informado"
