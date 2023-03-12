import contextlib
import subprocess
import threading
from queue import Empty, Queue


def executa_servico(output_queue):
    try:
        proc = subprocess.Popen(["ping", "-t", "google.com"],
                                stdout=subprocess.PIPE, bufsize=1)

        for linha in iter(proc.stdout.readline, b''):
            output_queue.put(linha.strip())

        proc.stdout.close()
        proc.wait()
        proc = None
    except subprocess.CalledProcessError as error_process:
        print("Erro ao executar subprocesso: ", {"Comando": error_process.cmd,
                                                 "CodigoRetorno": error_process.returncode,
                                                 "Saida": error_process.output
                                                 })
        proc = None


def iniciar():
    try:
        output_queue = Queue()
        threads = []

        for _ in range(3):
            thread = threading.Thread(
                target=executa_servico, args=(output_queue,))
            thread.start()
            threads.append(thread)

        while True:
            with contextlib.suppress(Empty):
                line = output_queue.get(timeout=1)
                print(line)
            if all(not thread.is_alive() for thread in threads):
                break

        print("Todas as threads encerradas.")
    except Exception as thread_error:
        print("Erro ao executar thread: ", thread_error)

    input('aperte entre')


if __name__ == '__main__':
    iniciar()
