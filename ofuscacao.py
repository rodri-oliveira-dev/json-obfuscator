from typing import Union


class OfuscarDados:
    def __init__(self, palavraOfuscacao: str):
        self.palavraOfuscacao = palavraOfuscacao

    def ofuscarDados(self, dados: Union[dict, str, bytes], campos: list[str]):
        '''
        Ofusca os campos indicados de um JSON.

                Parameters:
                        dados  (Union[dict, str, bytes]): Dados a serem ofuscados
                        campos (list[str]): Campos a serem aplicada ofuscacao

                Returns:
                        Um JSON com os campos ofuscados
        '''
        for root in dados.keys():
            for idx, entity in enumerate(dados[root]):
                dados[root][idx] = self.__protect(entity, campos, idx)

        return dados

    def __protect(self, dados: Union[dict, str, bytes], campos: Union[list, bool], idx: int):
        if isinstance(campos, bool) and campos is True:
            return self.palavraOfuscacao
        if isinstance(campos, list):
            for key in campos:
                if key in dados.keys():
                    dados[key] = self.__protect(dados[key], True, idx)
                else:
                    for node in dados.keys():
                        entity = dados[node]
                        if isinstance(entity, dict):
                            dados[node] = self.__protect(entity, campos, idx)
        return dados
