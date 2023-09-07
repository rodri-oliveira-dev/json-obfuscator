import json

def ofuscar(dados, ofusc, in_place=True):
    try:
        if isinstance(ofusc, str):
            ofusc = json.loads(ofusc)
    except json.JSONDecodeError:
        raise ValueError("A entrada não é um JSON válido")

    if not in_place:
        ofusc = json.loads(json.dumps(ofusc))

    dados = [palavra.lower() for palavra in dados]

    def percorrer(obj, visitados):
        if id(obj) in visitados:
            return obj
        visitados.add(id(obj))

        if isinstance(obj, dict):
            for chave, valor in list(obj.items()):
                if isinstance(valor, (dict, list)):
                    obj[chave] = percorrer(valor, visitados)
                elif chave.lower() in dados:
                    obj[chave] = "oculto"
        elif isinstance(obj, list):
            for i in range(len(obj)):
                obj[i] = percorrer(obj[i], visitados)
        return obj

    ofusc = percorrer(ofusc, set())

    return ofusc
