
using Microsoft.AspNetCore.Mvc;
using ReviseDotnet.Models.Dto;
using ReviseDotnet.Models.ViewModels;
using ReviseDotnet.Repositories;

namespace ReviseDotnet.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    public readonly IAuthRepository authRepository;
    public AuthController(IAuthRepository authRepository)
    {
        this.authRepository = authRepository;
    }

    [Route("sign-up")]
    [HttpPost]
    public async Task<SignUpResponse> SignUp(SignUpDto dto)
    {
        return await authRepository.SignUp(dto);
    }

    [Route("login")]
    [HttpPost]
    public async Task<LoginResponse> Login([FromBody] LoginDto dto)
    {
        return await authRepository.Login(dto);
    }

    [Route("token/refresh")]
    [HttpPost]
    public async Task<LoginResponse> RefreshToken([FromBody] RefreshTokenDto dto)
    {
        return await authRepository.RefreshToken(dto);
    }

}