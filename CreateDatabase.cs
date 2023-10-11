using ReviseDotnet.Helpers;
using ReviseDotnet.Models.Schemas;

namespace ReviseDotnet;

public static class DbCreate {
    public static void CreateDatabase(WebApplication app) {
        var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetService<SqlLiteDbContext>();
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
        var email = "sandip12081992@gmail.com";
        var user = db.Users.Where(u => u.Email.Equals(email)).FirstOrDefault();

        if (user == null || String.IsNullOrEmpty(user.Id)) {
            var password = PasswordHelper.HashPassword("Sandip@123");
            user = new User {
                Email = "sandip12081992@gmail.com",
                FullName = "Sandip Jaiswal",
                PasswordHash = password,
                Role = Role.Admin
            };
            db.Add(user);
            db.SaveChanges();
        }

        Console.WriteLine($"User: {user.FullName}");
    }
}