from typing import List

from src.domain_exceptions.ofuscar_dados_exception import OfuscarDadosException
from src.services.criptografar_dados_service import CriptografarDadosService


class OfuscarDadosService:
    MASCARA_PADRAO = "********"

    def __init__(self,
                 criptografar_dados: CriptografarDadosService,
                 campos_para_ofuscar: List[str]):

        self.__criptografar_dados = criptografar_dados
        self.__campos_para_ofuscar = campos_para_ofuscar

    def ofuscar_dados(self, dados_para_ofuscar: dict) -> dict:
        if not dados_para_ofuscar:
            raise OfuscarDadosException("Dados não informados")

        if not self.__campos_para_ofuscar:
            raise OfuscarDadosException(
                "Não foram informadas os campos para ofuscação")

        for root in dados_para_ofuscar:
            for idx, entity in enumerate(dados_para_ofuscar[root]):
                dados_para_ofuscar[root][idx] = self.__criptografar(entity)

        return dados_para_ofuscar

    def __criptografar(self, dados: dict) -> dict:
        for campo, valor_campo in dados.items():
            if isinstance(valor_campo, dict):
                dados[campo] = self.__criptografar(valor_campo)
            elif isinstance(valor_campo, str):
                if campo in self.__campos_para_ofuscar and campo in dados:
                    dados[campo] = self.__criptografar_dados.criptografar(
                        valor_campo)
        return dados
