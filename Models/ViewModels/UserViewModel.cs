namespace ReviseDotnet.Models.ViewModels;

public class SignUpResponse : LoginResponse
{
    
}

public class LoginResponse
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}