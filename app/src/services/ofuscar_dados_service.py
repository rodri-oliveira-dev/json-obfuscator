import json

from src.exceptions.ofuscar_dados_exception import OfuscarDadosException
from src.services.criptografar_dados_service import CriptografarDadosService


class OfuscarDadosService:
    MASCARA_PADRAO = "********"

    def __init__(self,
                 criptografar_dados: CriptografarDadosService,
                 campos: list):

        self.__criptografar_dados = criptografar_dados
        self.__campos = campos

    def ofuscar_dados(self, json_t: dict) -> dict:
        if not json_t:
            raise OfuscarDadosException("JSON não foi informado")

        if not self.__campos:
            raise OfuscarDadosException(
                "Não foram informadas os campos para ofuscação")

        for root in json_t:
            for idx, entity in enumerate(json_t[root]):
                json_t[root][idx] = self.__protect(entity)

        return json_t

    def __protect(self, dados: dict):
        for key in self.__campos:
            if key in dados:
                dados[key] = self.__criptografar_dados.criptografar(dados[key])
            else:
                for node, entity in dados.items():
                    if isinstance(entity, dict):
                        dados[node] = self.__protect(entity)
        return dados


if __name__ == "__main__":

    teste = CriptografarDadosService()

    crip = teste.criptografar('teste 1')
    decrip = teste.descriptografar(crip)

    # Arrange
    mascara = "********"
    campos = ['email', 'password']
    data = json.loads(open("app/tests/to_obfuscate_example.json", "rb").read())
    # Act
    ofuscador = OfuscarDadosService(mascara, campos)
    retorno = ofuscador.ofuscar_dados(data)
