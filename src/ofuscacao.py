from typing import Union


class OfuscarDados:
    '''
    Classe responsavel pela ofuscação de campos de um JSON
    '''

    MASCARA_PADRAO = "********"

    def __init__(self, mascara_ofuscacao: str):
        '''
        Args:
            mascara_ofuscacao (str): valor que sera usado como máscara de ofuscação.
            Se um valor não for informado a [MASCARA_PADRAO] será usada.
        '''
        if mascara_ofuscacao and not mascara_ofuscacao.isspace():
            self.mascara_ofuscacao = mascara_ofuscacao
        else:
            self.mascara_ofuscacao = self.MASCARA_PADRAO

    def ofuscar_dados(self, json: dict, campos: list[str]) -> dict:
        '''
        Ofusca os campos indicados de um JSON.

                Parameters:
                        json   (dict):      Dados a serem ofuscados
                        campos (list[str]): Campos a serem aplicada a ofuscação

                Returns:
                        Um JSON com os campos ofuscados

                Raises:
                        json   (Exception): JSON não foi informado
                        campos (Exception): Não foram informadas os campos para ofuscação
        '''

        if not json:
            Exception("JSON não foi informado")

        if not campos:
            Exception("Não foram informadas os campos para ofuscação")

        for root in json.keys():
            for idx, entity in enumerate(json[root]):
                json[root][idx] = self.__protect(entity, campos, idx)

        return json

    def __protect(self, dados: dict, campos: Union[list, bool], idx: int):
        if isinstance(campos, bool) and campos is True:
            return self.mascara_ofuscacao
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
