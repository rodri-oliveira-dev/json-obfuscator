import splunklib.client as client
import splunklib.results as results
import time
import json

# Configurações
HOST = "seu_host_splunk"
PORT = 8089
USERNAME = "seu_usuario"
PASSWORD = "sua_senha"
SEARCH_QUERY = 'search seu_indice aqui earliest="2023-09-07T16:02:00.000" latest="2023-09-17T16:02:00.000"'

# Conexão com o Splunk
service = client.connect(host=HOST, port=PORT, username=USERNAME, password=PASSWORD)

# Enviando a query
job = service.jobs.create(SEARCH_QUERY)

# Esperando o job ser finalizado
while True:
    while not job.is_ready():
        pass
    if job["isDone"] == "1":
        break
    time.sleep(2)

# Pegando os resultados
results_stream = job.results(output_mode="json")

data = [result for result in results.ResultsReader(results_stream)]
with open("splunk_results.json", "w") as outfile:
    json.dump(data, outfile)

# Finalizando o job
job.cancel()

print("Dados salvos em 'splunk_results.json'")
