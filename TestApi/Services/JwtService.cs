using System.Security.Claims;
using TestApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace TestApi.Services;

public class JwtService
{
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _expirationHours;
    private readonly SymmetricSecurityKey _key;
    private readonly SigningCredentials _credentials;
    private readonly JwtSecurityTokenHandler _tokenHandler;
    public JwtService(IConfiguration configuration)
    {
        _secretKey = configuration["Jwt:SecretKey"] ?? throw new ArgumentNullException("Jwt:SecretKey not found in configuration");
        _expirationHours = configuration.GetValue<int>("Jwt:ExpirationHours", 24);
        _issuer = configuration["Jwt:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer not found in configuration");
        _audience = configuration["Jwt:Audience"] ?? throw new ArgumentNullException("Jwt:Audience not found in configuration");
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        _credentials = new SigningCredentials(_key, SecurityAlgorithms.Aes256CbcHmacSha512);
        _tokenHandler = new JwtSecurityTokenHandler();
    }
    public string GenerateJWT(User user)
    {

        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, user.Id.ToString()),
            new (ClaimTypes.GivenName, user.FirstName),
            new (ClaimTypes.Surname, user.LastName),
            new (ClaimTypes.Email, user.Email),
            new ("created_at", user.CreatedAt.ToString("O")),
            new ("updated_at", user.UpdatedAt.ToString("O"))
        };

        var jwtDescriptor = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.Now.AddHours(_expirationHours),
            signingCredentials: _credentials
        );

        return _tokenHandler.WriteToken(jwtDescriptor);
    }
}