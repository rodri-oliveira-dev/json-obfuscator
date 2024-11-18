using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        // Solicitar o caminho do arquivo
        Console.WriteLine("Digite o caminho do arquivo:");
        string caminhoArquivo = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(caminhoArquivo) || !File.Exists(caminhoArquivo))
        {
            Console.WriteLine("Arquivo não encontrado ou caminho inválido.");
            return;
        }

        // Termo a buscar
        string termoABuscar = "return new MensagemRetornoDTO";

        try
        {
            // Ler todas as linhas do arquivo
            string[] linhas = File.ReadAllLines(caminhoArquivo);
            bool encontrouLinhas = false;

            // Buscar pelo termo em cada linha
            for (int i = 0; i < linhas.Length; i++)
            {
                if (linhas[i].Contains(termoABuscar, StringComparison.OrdinalIgnoreCase))
                {
                    encontrouLinhas = true;
                    Console.WriteLine($"Linha {i + 1}: {linhas[i].Trim()}");
                }
            }

            if (!encontrouLinhas)
            {
                Console.WriteLine("Nenhuma linha encontrada com o termo buscado.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao processar o arquivo: {ex.Message}");
        }
    }
}