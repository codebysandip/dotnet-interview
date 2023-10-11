using System.ComponentModel.DataAnnotations;

namespace ReviseDotnet.Models.Dto;

public class SignUpDto
{
    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; }

    [Required]
    // [TODO] Create a Custom validtor for Password
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d@!$&]{8,15}$")]
    public string Password { get; set; }

    [Required]
    [Compare("Password", ErrorMessage = "Password and Confirm Password doesn't match")]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d@!$&]{8,15}$")]
    public string ConfirmPassword { get; set; }

    [Required]
    [MaxLength(50)]
    public string FullName { get; set; }
}

public class LoginDto
{
    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; }

    [Required]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d@!$&]{8,15}$")]
    public string Password { get; set; }

}

public class RefreshTokenDto
{
    [Required]
    // [TODO] Create a custom validator to check token is a jwt token
    public string RefreshToken { get; set; }
}