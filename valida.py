import json
from pprint import pprint

from ofuscacao import OfuscarDados

data = json.loads(open("to_obfuscate_example.json", "rb").read())

settings = json.loads(open("settings.json", "rb").read())
campos = ['email', 'password', 'aaaa']

ofuscador = OfuscarDados("************")

retorno = ofuscador.ofuscarDados(data, campos)

pprint(retorno, width=100)
