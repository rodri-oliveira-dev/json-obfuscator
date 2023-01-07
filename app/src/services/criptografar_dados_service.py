import rsa
from src.exceptions.ofuscar_dados_exception import OfuscarDadosException
from src.shared.certificados_service import CertificadosService


class CriptografarDadosService:

    def __init__(self, certificados_service: CertificadosService) -> None:
        self.__certificados_service = certificados_service

    def criptografar(self, valor: str) -> str:
        if valor is None or not valor:
            return ""
        chave_publica = self.__certificados_service.carregar_chaves(
            self.__certificados_service.caminho_chave_publica)
        return rsa.encrypt(valor.encode('ascii'), chave_publica)

    def descriptografar(self, valor: str) -> str:
        if valor is None or not valor:
            return ""
        try:
            chave_privada = self.__certificados_service.carregar_chaves(
                self.__certificados_service.caminho_chave_privada)
            return rsa.decrypt(valor, chave_privada).decode('ascii')
        except Exception as ex:
            raise OfuscarDadosException("aaa") from ex
