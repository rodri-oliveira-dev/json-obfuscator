using System;
using WireMock.Server;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

class Program
{
    static void Main(string[] args)
    {
        // Inicia o servidor WireMock na porta 8080
        var server = WireMockServer.Start(8080);

        Console.WriteLine("WireMock está rodando na porta: " + server.Ports[0]);

        // Configura a rota /test-retry para retornar respostas sequenciais
        server.Given(
            Request.Create()
                .WithPath("/test-retry")
                .UsingGet()
        )
        .InScenario("Retry Scenario") // Nomeia o cenário
        .WhenScenarioStateIs("Started") // Estado inicial do cenário
        .WillSetStateTo("Second Attempt") // Altera o estado após a resposta
        .RespondWith(
            Response.Create()
                .WithStatusCode(404)
                .WithBody("{\"message\": \"Not Found - First Attempt\"}")
        );

        // Configuração para a segunda requisição
        server.Given(
            Request.Create()
                .WithPath("/test-retry")
                .UsingGet()
        )
        .InScenario("Retry Scenario") // Mesmo cenário
        .WhenScenarioStateIs("Second Attempt") // Estado após a primeira resposta
        .RespondWith(
            Response.Create()
                .WithStatusCode(200)
                .WithBody("{\"message\": \"Success - Second Attempt\"}")
        );

        Console.WriteLine("Pressione qualquer tecla para encerrar o servidor...");
        Console.ReadKey();

        server.Stop();
    }
}
