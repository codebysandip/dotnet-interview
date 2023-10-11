
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReviseDotnet.Models.Schemas;

public class User : BaseSchema
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public string Id { get; set; }

    public string FullName { get; set; }

    public string Email { get; set; }

    public string PasswordHash { get; set; }

    public Role Role { get; set; }

}

public enum Role
{
    Admin = 1,
    User = 2
}
