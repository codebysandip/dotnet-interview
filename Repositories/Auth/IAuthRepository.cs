
using ReviseDotnet.Models.Dto;
using ReviseDotnet.Models.ViewModels;

namespace ReviseDotnet.Repositories
{
    public interface IAuthRepository
    {
        Task<LoginResponse> Login(LoginDto dto);
        Task<SignUpResponse> SignUp(SignUpDto dto);
        Task<LoginResponse> RefreshToken(RefreshTokenDto dto);
    }
}