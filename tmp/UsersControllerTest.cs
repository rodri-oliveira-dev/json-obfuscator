using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Bunge.Authentication.Domain.DTO.Base;
using Bunge.Authentication.Test.Helper;
using Bunge.Authentication.Test.IntegrationTests.Application;
using Bunge.Authentication.Test.IntegrationTests.Database;
using Bunge.Shared.Contracts.Request;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Bunge.Authentication.Test.IntegrationTests;

public class UsersControllerTest : IClassFixture<TestWebApplication>
{
    private readonly TestWebApplication _testWebApplication;

    public UsersControllerTest(TestWebApplication testWebApplication)
    {
        _testWebApplication = testWebApplication;
    }

    [Fact(DisplayName = "Deve retornar sucesso e conteúdo correto ao buscar usuário existente")]
    public async Task Get_EndpointsReturnSuccessAndCorrectContentType()
    {
        // Arrange
        const string UserMail = "oliveira.rodri.ext@bunge.com";
        const string ProviderName = "azureciandt";
        const string AppClientId = "6pg1h3974mr2f3f69k43d013jl";
        const string Url = $"/Users/GetDataUserToToken?userMail={UserMail}&providerName={ProviderName}&appClientID={AppClientId}";
        
        var query = await TestDatabaseManager.CarregaScriptSql("Get_EndpointsReturnSuccessAndCorrectContentType.sql");
        await _testWebApplication.DatabaseManager.InsertData(query);

        var client = _testWebApplication.Factory.CreateClient();
        // Act
        var response = await client.GetAsync(Url);

        // Assert
        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var userData = JsonSerializer.Deserialize<UserTokenDto>(responseBody,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(userData);
            Assert.Equal(UserMail, userData!.UserLogin);
        }
    }

    [Fact(DisplayName = "Deve retornar NotFound ao buscar usuário inexistente")]
    public async Task Get_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        const string InvalidUserMail = "invalido@bunge.com";
        const string ProviderName = "azureciandt";
        const string AppClientId = "6pg1h3974mr2f3f69k43d013jl";

        const string Url = $"/Users/GetDataUserToToken?userMail={InvalidUserMail}&providerName={ProviderName}&appClientID={AppClientId}";
        var client = _testWebApplication.Factory.CreateClient();

        // Act
        var response = await client.GetAsync(Url);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact(DisplayName = "Deve retornar PreconditionFailed ao informar dados invalidos")]
    public async Task Post_ReturnsPreconditionFailed_WhenUserIsInvalid()
    {
        // Arrange
        const string Url = $"/Users";
        var payload = new UsersContractRequest();
        var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

        
        var token = JwtTokenGenerator.GenerateTestToken();
        var client = _testWebApplication.Factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        client.DefaultRequestHeaders.Add("userLanguage", "pt-BR");
        // Act
        var response = await client.PostAsync(Url,content);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.PreconditionFailed, response.StatusCode);
    }
}
