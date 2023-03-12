import os
from dataclasses import dataclass
from typing import Any

import rsa
from src.domain_exceptions.chave_cripitografia_exception import \
    ChaveCriptografiaException


@dataclass(frozen=True)
class Chaves:
    chave_publica: Any
    chave_privada: Any


class CertificadosService:

    @property
    def caminho_chave_publica(self):
        return self.__chave_publica

    @property
    def caminho_chave_privada(self):
        return self.__chave_privada

    def __init__(self,
                 diretorio_chaves: str,
                 criar_diretorio_chaves: bool):
        if (diretorio_chaves is None or diretorio_chaves.count == 0):
            raise ChaveCriptografiaException('Diretório é obrigatório')
        if not os.path.exists(diretorio_chaves):
            if criar_diretorio_chaves:
                os.makedirs(diretorio_chaves)
            else:
                raise ChaveCriptografiaException('Diretório inexistente')

        self.__diretorio_chaves = diretorio_chaves
        self.__chave_publica = f'{self.__diretorio_chaves}/chave-publica.pem'
        self.__chave_privada = f'{self.__diretorio_chaves}/chave-privada.pem'

    def gerar_chaves(self, salvar_chaves: bool) -> Chaves:
        if os.path.exists(self.__chave_publica) and \
                os.path.exists(self.__chave_privada):
            return self.carregar_chaves(
                self.__chave_publica, self.__chave_privada)

        (chave_publica, chave_privada) = rsa.newkeys(1024)

        if salvar_chaves:
            with open(self.__chave_publica, 'wb') as p:
                p.write(chave_publica.save_pkcs1('PEM'))
            with open(self.__chave_privada, 'wb') as p:
                p.write(chave_privada.save_pkcs1('PEM'))

        return Chaves(chave_publica, chave_privada)

    def carregar_chaves(self,
                        caminho_chave_publica: str,
                        caminho_chave_privada: str) -> Chaves:
        if not os.path.exists(caminho_chave_publica):
            raise ChaveCriptografiaException(
                f"Não foi possível encontrar a chave publica no caminho : '{caminho_chave_publica}'")
        if not os.path.exists(caminho_chave_privada):
            raise ChaveCriptografiaException(
                f"Não foi possível encontrar a chave privada no caminho : '{caminho_chave_privada}'")

        chave_publica: Any = None
        chave_privada: Any = None
        with open(caminho_chave_publica, 'rb') as ch_pu:
            chave_publica = rsa.PublicKey.load_pkcs1(ch_pu.read())

        with open(caminho_chave_privada, 'rb') as ch_pr:
            chave_privada = rsa.PrivateKey.load_pkcs1(ch_pr.read())

        return Chaves(chave_publica, chave_privada)
