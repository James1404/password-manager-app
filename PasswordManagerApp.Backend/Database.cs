using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace PasswordManagerApp.Backend;

public class PasswordManagerContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Item> Items { get; set; }

    public string DbPath { get; }

    public PasswordManagerContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "pma.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={DbPath}");
    }
}

public class Account
{
    public int AccountId { get; set; }
    public required string Email { get; set; }
    public required byte[] HashedPassword { get; set; }
    public required byte[] Salt { get; set; }
}

public class Item
{
    public int ItemId { get; set; }

    public required byte[] Name { get; set; }
    public required byte[] Url { get; set; }
    public required byte[] Username { get; set; }
    public required byte[] Password { get; set; }

    public int AccountId { get; set; }
    public required Account Account { get; set; }
}

public class Database
{
    public static (byte[] hash, byte[] salt) Encrypt(string input)
    {
        const int size = 25;
        using var rng = RandomNumberGenerator.Create();

        var salt = new byte[size];
        rng.GetBytes(salt);

        var pkbdf2 = new Rfc2898DeriveBytes(input, salt, 1000, HashAlgorithmName.SHA256);
        var hash = pkbdf2.GetBytes(size);

        return (hash, salt);
    }

    public static string Decrypt(byte[] input, byte[] key)
    {
        return "";
    }
}
