
import json

from src.ofuscar_dados import ObfuscateJson


def iniciar():

    campos = {'cidade', 'idade'}
    ofuscar_dados_service = ObfuscateJson(campos)

    data = {"nome": "João Silva", "idade": 30, "cidade": "São Paulo"}
    retorno = ofuscar_dados_service.to_json(data)
    print(retorno)


if __name__ == '__main__':
    iniciar()
