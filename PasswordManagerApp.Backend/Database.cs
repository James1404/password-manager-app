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

public static class Database
{
    public const int ByteLen = 16;

    public static byte[] GenSalt()
    {
        var salt = new byte[ByteLen];

        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);

        return salt;
    }

    public static byte[] HashPassword(string input, byte[] salt)
    {
        var pkbdf2 = new Rfc2898DeriveBytes(input, salt, 1000, HashAlgorithmName.SHA256);
        var hash = pkbdf2.GetBytes(ByteLen);

        return hash;
    }

    public static byte[] Encrypt(string input, byte[] key, byte[] iv)
    {
        using Aes aesAlgo = Aes.Create();

        aesAlgo.Key = key;
        aesAlgo.IV = iv;

        ICryptoTransform encryptor = aesAlgo.CreateEncryptor(aesAlgo.Key, aesAlgo.IV);

        using MemoryStream msEncrypt = new MemoryStream();
        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
        {
            using StreamWriter swEncrypt = new StreamWriter(csEncrypt);

            swEncrypt.Write(input);
        }

        return msEncrypt.ToArray();
    }

    public static string Decrypt(byte[] input, byte[] key, byte[] iv)
    {
        using Aes aesAlgo = Aes.Create();
        aesAlgo.Key = key;
        aesAlgo.IV = iv;

        ICryptoTransform decryptor = aesAlgo.CreateDecryptor(aesAlgo.Key, aesAlgo.IV);

        using MemoryStream msDecrypt = new MemoryStream(input);
        using CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
        using StreamReader swDecrypt = new StreamReader(csDecrypt);

        return swDecrypt.ReadToEnd();
    }

    static void AsmKey()
    {
        const string KeyName = "Key1";
        var cspp = new CspParameters() { KeyContainerName = KeyName };
        var rsa = new RSACryptoServiceProvider(cspp) { PersistKeyInCsp = true };
    }

    public static string Prettify(this byte[] b) => string.Join(".", b);
}
