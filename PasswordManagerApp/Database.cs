using Microsoft.EntityFrameworkCore;

namespace PasswordManagerApp.Backend.Database;

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
    public string Email { get; set; }
    public byte[] HashedPassword { get; set; }
    public byte[] Salt { get; set; }
}

public class Item
{
    public int ItemId { get; set; }

    public byte[] Name { get; set; }
    public byte[] Url { get; set; }
    public byte[] Username { get; set; }
    public byte[] Password { get; set; }

    public int AccountId { get; set; }
    public Account Account { get; set; }
}

class Database
{

}
