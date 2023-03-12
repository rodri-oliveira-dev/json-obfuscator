import base64

import rsa
from src.domain_exceptions.ofuscar_dados_exception import OfuscarDadosException
from src.shared.certificados_service import CertificadosService


class CriptografarDadosService:

    def __init__(self, certificados_service: CertificadosService) -> None:
        self.__certificados_service = certificados_service
        self.__chaves = self.__certificados_service.carregar_chaves(
            self.__certificados_service.caminho_chave_publica,
            self.__certificados_service.caminho_chave_privada)

    def criptografar(self, valor: str) -> str:
        if valor is None or not valor:
            return ""
        return base64.b64encode(
            rsa.encrypt(valor.encode(), self.__chaves.chave_publica)
        ).decode()

    def descriptografar(self, valor: str) -> str:
        if valor is None or not valor:
            return ""
        try:
            var_real = base64.b64decode(valor)
            return rsa.decrypt(var_real,
                               self.__chaves.chave_privada).decode('ascii')
        except Exception as ex:
            raise OfuscarDadosException("aaa") from ex
