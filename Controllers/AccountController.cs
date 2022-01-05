using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DynamicDbContextWeb.Controllers;
[Route("[controller]")]
[ApiController]
public sealed class AccountController : Controller
{
    public AccountController(Settings settings)
    {
        Settings = settings;
    }

    public Settings Settings
    {
        get;
    }

    [HttpPost(nameof(Login))]
    public async Task<Token> Login([FromBody] LoginCredentials credentials)
    {
        JwtSecurityTokenHandler tokenHandler = new();
        byte[] key = Encoding.UTF8.GetBytes(Settings.JwtToken.SecretKey);

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, credentials.Name),
                new Claim(ClaimTypes.UserData, credentials.Domain)
            }),
            IssuedAt = DateTime.Now,
            Issuer = Settings.JwtToken.Issuer,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature),
            Expires = DateTime.Now.AddYears(20)
        };

        SecurityToken tokenData = tokenHandler.CreateToken(tokenDescriptor);
        string tokenString = tokenHandler.WriteToken(tokenData);

        return new Token
        {
            TokenValue = tokenString
        };
    }
}