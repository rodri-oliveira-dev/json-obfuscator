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

        // Configura a rota /test-retry para retornar 404 na primeira tentativa
        server.Given(
            Request.Create()
                .WithPath("/test-retry")
                .UsingGet()
        )
        .InScenario("Retry Scenario") // Nomeia o cenário
        .WhenStateIs("Started") // Estado inicial do cenário
        .WillSetStateTo("Second Attempt") // Define o próximo estado após essa resposta
        .RespondWith(
            Response.Create()
                .WithStatusCode(404)
                .WithBody("{\"message\": \"Not Found - First Attempt\"}")
        );

        // Configuração para a segunda tentativa
        server.Given(
            Request.Create()
                .WithPath("/test-retry")
                .UsingGet()
        )
        .InScenario("Retry Scenario") // Mesmo cenário
        .WhenStateIs("Second Attempt") // Estado após a primeira resposta
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
