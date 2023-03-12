import string


class OfuscarDadosException(Exception):
    def __init__(self, message: string):
        self.message = message
        super().__init__(self.message)
