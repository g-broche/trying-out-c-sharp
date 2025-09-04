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
    private readonly int _expirationMinutes;
    private readonly bool _isCookieSecure;
    private readonly SymmetricSecurityKey _key;
    private readonly SigningCredentials _credentials;
    private readonly JwtSecurityTokenHandler _tokenHandler;
    public JwtService(IConfiguration configuration)
    {
        _secretKey = configuration["Jwt:SecretKey"] ?? throw new ArgumentNullException("Jwt:SecretKey not found in configuration");
        _expirationMinutes = configuration.GetValue<int>("Jwt:ExpirationMinutes", 60);
        _isCookieSecure = configuration.GetValue<bool>("Jwt:Secure", true);
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
            new (ClaimTypes.Email, user.Email),
        };

        var jwtDescriptor = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_expirationMinutes),
            signingCredentials: _credentials
        );

        return _tokenHandler.WriteToken(jwtDescriptor);
    }

    public CookieOptions GenerateCookieOptions()
    {
        return new CookieOptions
        {
            HttpOnly = true,
            Secure = _isCookieSecure,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddMinutes(_expirationMinutes)
        };
    }

    public CookieOptions GenerateExpiredCookieOptions()
    {
        return new CookieOptions
        {
            HttpOnly = true,
            Secure = _isCookieSecure,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(-1)
        };
    }
}