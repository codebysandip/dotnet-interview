
using Microsoft.EntityFrameworkCore;
using ReviseDotnet.Models.Schemas;

public class SqlLiteDbContext : DbContext {
    public string DbPath { get; set; }

    public DbSet<User> Users { get; set; }
    
    public SqlLiteDbContext() {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "reviseDotnet.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(b => b.Email).IsUnique();
    }
}