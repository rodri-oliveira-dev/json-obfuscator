import json
import os

from src.services.criptografar_dados_service import CriptografarDadosService
from src.services.ofuscar_dados_service import OfuscarDadosService
from src.shared.certificados_service import CertificadosService


def iniciar():

    campos = ['email', 'password']
    diretorio_chave = os.path.join(
        os.path.dirname(os.path.abspath(__file__)), 'chaves')

    certificado_service = CertificadosService(
        diretorio_chaves=diretorio_chave, criar_diretorio_chaves=True)

    certificado_service.gerar_chaves(True)

    criptografar_dados_service = CriptografarDadosService(certificado_service)

    ofuscar_dados_service = OfuscarDadosService(
        criptografar_dados_service, campos)

    data = json.loads(open("app/tests/to_obfuscate_example.json", "rb").read())
    retorno = ofuscar_dados_service.ofuscar_dados(data)
    print(retorno)


if __name__ == '__main__':
    iniciar()
