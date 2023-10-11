using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ReviseDotnet.Exceptions;
using ReviseDotnet.Helpers;
using ReviseDotnet.Models.Dto;
using ReviseDotnet.Models.Schemas;
using ReviseDotnet.Models.ViewModels;

namespace ReviseDotnet.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly SqlLiteDbContext db;
    private readonly IConfiguration _config;

    public AuthRepository(SqlLiteDbContext dbContext, IConfiguration config)
    {
        db = dbContext;
        _config = config;
    }

    public async Task<LoginResponse> Login(LoginDto dto)
    {
        var user = await db.Users.Where(u => u.Email == dto.Email).FirstOrDefaultAsync();

        if (user == null)
        {
            // [TODO] Create a documentation for error codes
            throw new BadRequestException("User not found", 10001);
        }

        // [TODO] Implement max retry
        if (PasswordHelper.VerifyPassword(dto.Password, user.PasswordHash) == false)
        {
            throw new BadRequestException("Password doesn't match", 10002);
        }

        var createdToken = CreateToken(user);
        return new LoginResponse
        {
            Token = createdToken.Item1,
            RefreshToken = createdToken.Item2
        };
    }

    public async Task<SignUpResponse> SignUp(SignUpDto dto)
    {
        var user = new User
        {
            Email = dto.Email,
            FullName = dto.FullName,
            PasswordHash = PasswordHelper.HashPassword(dto.Password)
        };
        db.Users.Add(user);
        var result = await db.SaveChangesAsync();
        if (result == 0)
        {
            throw new InternalServerException("Unable to save user in database", 10003);
        }
        user = db.Users.Where(u => u.Email == dto.Email).FirstOrDefault();
        var createdToken = CreateToken(user);

        return new SignUpResponse
        {
            Token = createdToken.Item1,
            RefreshToken = createdToken.Item2
        };
    }

    public async Task<LoginResponse> RefreshToken(RefreshTokenDto dto)
    {
        SecurityToken validatedToken;
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var user = tokenHandler.ValidateToken(dto.RefreshToken, new TokenValidationParameters
            {
                ValidIssuer = _config.GetValue<string>("Jwt:Issuer"),
                ValidAudience = _config.GetValue<string>("Jwt:Audience"),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetValue<string>("Jwt:Key"))),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true

            }, out validatedToken);
            var email = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
            var userData = await db.Users.FirstOrDefaultAsync(x => x.Email == email.Value);
            if (userData == null)
            {
                throw new BadRequestException("User not found", 10001);
            }

            var createdToken = CreateToken(userData);
            return new LoginResponse
            {
                Token = createdToken.Item1,
                RefreshToken = createdToken.Item2
            };
        }
        catch
        {

            throw new BadRequestException("Invalid or Expired Token", 10004);
        }
    }

    private Tuple<string, string> CreateToken(User user)
    {
        var issuer = _config.GetValue<string>("Jwt:Issuer");
        var audience = _config.GetValue<string>("Jwt:Audience");
        var key = Encoding.ASCII.GetBytes(_config.GetValue<string>("Jwt:Key"));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("sub", user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("role", user.Role.ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(60),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
        };

        var refreshTokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
                new[] {
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("sub", user.Id),
                }
             ),
            Expires = DateTime.UtcNow.AddMinutes(120),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);
        var stringToken = tokenHandler.WriteToken(token);


        var refreshToken = tokenHandler.CreateToken(tokenDescriptor);
        var jwtRefreshToken = tokenHandler.WriteToken(token);
        var stringRefreshToken = tokenHandler.WriteToken(token);
        return new Tuple<string, string>(stringToken, stringRefreshToken);
    }
}