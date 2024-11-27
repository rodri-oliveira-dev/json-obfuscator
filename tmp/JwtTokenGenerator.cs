using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Bunge.Authentication.Test.Helper;

/// <summary>
/// Classe estática responsável por gerar um token JWT para fins de teste.
/// </summary>
public static class JwtTokenGenerator
{
    /// <summary>
    /// Gera um token JWT para uso em testes de integração.
    /// O token contém uma claim para o nome de usuário ("testuser") e o papel ("Administrator").
    /// O token é assinado com uma chave simétrica usando HMAC-SHA256 e expira após 1 hora.
    /// </summary>
    /// <returns>Um token JWT como string.</returns>
    public static string GenerateTestToken()
    {
        // Cria uma instância de JwtSecurityTokenHandler para gerar o token
        var tokenHandler = new JwtSecurityTokenHandler();

        // Define as informações do token, incluindo claims, tempo de expiração e credenciais de assinatura
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "testuser"),   // Claim para o nome do usuário
                new Claim(ClaimTypes.Role, "Administrator") // Claim para o papel do usuário
            }),
            Expires = DateTime.UtcNow.AddHours(1), // Tempo de expiração do token (1 hora a partir da geração)
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-256-bit-secret")), // Chave simétrica para assinatura
                SecurityAlgorithms.HmacSha256Signature // Algoritmo de assinatura (HMAC-SHA256)
            )
        };

        // Gera o token usando as informações configuradas no token descriptor
        var token = tokenHandler.CreateToken(tokenDescriptor);

        // Retorna o token serializado como uma string
        return tokenHandler.WriteToken(token);
    }
}

