# import json
# import os

# from src.services.criptografar_dados_service import CriptografarDadosService
# from src.services.ofuscar_dados_service import OfuscarDadosService
# from src.shared.certificados_service import CertificadosService


# def iniciar():

#     campos = ['email', 'password']
#     diretorio_chave = os.path.join(
#         os.path.dirname(os.path.abspath(__file__)), 'chaves')

#     certificado_service = CertificadosService(
#         diretorio_chaves=diretorio_chave, criar_diretorio_chaves=True)

#     certificado_service.gerar_chaves(True)

#     criptografar_dados_service = CriptografarDadosService(certificado_service)

#     ofuscar_dados_service = OfuscarDadosService(
#         criptografar_dados_service, campos)

#     data = json.loads(open("app/tests/to_obfuscate_example.json", "rb").read())
#     retorno = ofuscar_dados_service.ofuscar_dados(data)
#     print(retorno)


# if __name__ == '__main__':
# iniciar()

# import subprocess

# with subprocess.Popen(["ping", "google.com"], stdout=subprocess.PIPE) as proc:
#     for line in proc.stdout:
#         print(line.strip())
#     proc.wait()
# proc = None
# print("Programa externo encerrou.")

# import subprocess

# try:
#     proc = subprocess.Popen(["ping", "google.com"], stdout=subprocess.PIPE)
#     for line in proc.stdout:
#         print(line.strip())
#     proc.wait()
#     proc = None
# except subprocess.CalledProcessError as e:
#     print("Erro ao executar subprocesso: ", e)
#     proc = None

# import subprocess
# import threading


# def execute_ping():
#     try:
#         proc = subprocess.Popen(["ping", "google.com"], stdout=subprocess.PIPE)
#         for line in proc.stdout:
#             print(line.strip())
#         proc.wait()
#         proc = None
#     except subprocess.CalledProcessError as e:
#         print("Erro ao executar subprocesso: ", e)


# try:
#     thread1 = threading.Thread(target=execute_ping)
#     thread1.start()
#     thread2 = threading.Thread(target=execute_ping)
#     thread2.start()

#     # Código Python continua executando enquanto o subprocesso está em execução

#     print("Thread separada iniciada.")
#     # thread1.join()
#     print("Thread separada encerrada.")
# except Exception as e:
#     print("Erro ao executar thread: ", e)

# import subprocess
# import threading


# def execute_ping():
#     try:
#         proc = subprocess.Popen(["ping","-t", "google.com"], stdout=subprocess.PIPE)
#         for line in proc.stdout:
#             print(line.decode().strip())
#         proc.wait()
#         proc = None
#     except subprocess.CalledProcessError as e:
#         print("Erro ao executar subprocesso: ", e)
#         proc = None


# try:
#     threads = []

#     for _ in range(3):
#         thread = threading.Thread(target=execute_ping)
#         thread.start()
#         threads.append(thread)

#     # Código Python continua executando enquanto os subprocessos estão em execução

#     for thread in threads:
#         thread.join()

#     print("Todas as threads encerradas.")
# except Exception as e:
#     print("Erro ao executar thread: ", e)

# import contextlib
# import subprocess
# import threading
# from queue import Empty, Queue


# def execute_ping(output_queue):
#     try:
#         proc = subprocess.Popen(["ping", "-t", "google.com"],
#                                 stdout=subprocess.PIPE, bufsize=1)

#         for line in iter(proc.stdout.readline, b''):
#             output_queue.put(line.strip())

#         proc.stdout.close()
#         proc.wait()
#         proc = None
#     except subprocess.CalledProcessError as e:
#         print("Erro ao executar subprocesso: ", e)
#         proc = None


# try:
#     output_queue = Queue()
#     threads = []

#     for _ in range(3):
#         thread = threading.Thread(target=execute_ping, args=(output_queue,))
#         thread.start()
#         threads.append(thread)

#     # Código Python continua executando enquanto os subprocessos estão em execução

#     # while True:
#     #     try:
#     #         line = output_queue.get(timeout=1)
#     #         print(line)
#     #     except Empty:
#     #         pass

#     #     if all(not thread.is_alive() for thread in threads):
#     #         break

#     while True:
#         with contextlib.suppress(Empty):
#             line = output_queue.get(timeout=1)
#             print(line)
#         if all(not thread.is_alive() for thread in threads):
#             break

#     print("Todas as threads encerradas.")
# except Exception as e:
#     print("Erro ao executar thread: ", e)


# input('aperte entre')

import netifaces

# Obtém as informações das interfaces de rede
interfaces = netifaces.interfaces()

# Loop pelas interfaces
for interface in interfaces:
    # Obtém as informações da interface
    info = netifaces.ifaddresses(interface)
    # Verifica se a interface é uma interface Wi-Fi
    if netifaces.AF_INET in info:
        if 'wireless' in info[netifaces.AF_INET][0]['flags']:
            # Obtém o nome da rede Wi-Fi atual
            ssid = info[netifaces.AF_INET][0]['addr']
            print(f"Conectado à rede Wi-Fi: {ssid}")
