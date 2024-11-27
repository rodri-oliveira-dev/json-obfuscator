using Bunge.Authentication.Test.IntegrationTests.Database;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Bunge.Authentication.Test.IntegrationTests.Application;

/// <summary>
/// Classe abstrata responsável por configurar o ambiente de testes de integração em uma aplicação ASP.NET Core.
/// Esta classe configura a aplicação para testes, executa migrações e gerencia o ciclo de vida do banco de dados.
/// Implementa <see cref="IClassFixture{TFixture}"/> para compartilhar uma instância de <see cref="WebApplicationFactory{Program}"/> entre os testes.
/// </summary>
public class TestWebApplication : IDisposable
{
    private const string DefaultEnvironment = "TESTELOCAL";

    /// <summary>
    /// Instância do <see cref="WebApplicationFactory{Program}"/> usada para configurar e inicializar o ambiente de teste.
    /// </summary>
    public readonly WebApplicationFactory<Program> Factory;

    /// <summary>
    /// Responsavel pela interação com o banco de dados
    /// </summary>
    public readonly TestDatabaseManager DatabaseManager;

    public TestWebApplication()
    {
        if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")))
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", DefaultEnvironment);
        }

        Factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((context, configBuilder) =>
            {
                configBuilder.Sources.Clear();
                configBuilder.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
                configBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                configBuilder.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
                    .AddEnvironmentVariables();
                var connectionString = GenerateTesteConnectionString();
                var inMemorySettings = new Dictionary<string, string>
                {
                    { "ConnectionString", connectionString } 
                };

                configBuilder.AddInMemoryCollection(inMemorySettings);
            });
        });
        DatabaseManager = new TestDatabaseManager(GetConfiguration(Factory));
        DatabaseManager.ExecuteMigrations();
    }

    private static string GenerateTesteConnectionString()
    {
        var connectionStringBuilder =
            new SqlConnectionStringBuilder("Server=(localdb)\\MSSQLLocalDB;Database=;Integrated Security=true;")
            {
                InitialCatalog = TestDatabaseManager.GenerateRandomDatabaseName()
            };
        return connectionStringBuilder.ConnectionString;
    }

    /// <summary>
    /// Obtém a configuração de aplicação a partir do <see cref="WebApplicationFactory{Program}"/>.
    /// </summary>
    /// <param name="factory">Instância de <see cref="WebApplicationFactory{Program}"/> que contém os serviços configurados.</param>
    /// <returns>Instância de <see cref="IConfiguration"/> que contém as configurações carregadas para a aplicação de teste.</returns>
    private static IConfiguration GetConfiguration(WebApplicationFactory<Program> factory)
    {
        using var scope = factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<IConfiguration>();
    }

    /// <summary>
    /// Método chamado ao final dos testes para executar a limpeza, removendo o banco de dados criado durante os testes.
    /// </summary>
    public void Dispose()
    {
        DatabaseManager.DropTestDatabase();
    }
}
