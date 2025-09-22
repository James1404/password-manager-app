namespace PasswordManagerApp.TUI;

using PasswordManagerApp.Backend;
using Spectre.Console;

class Program
{
    static void Main(string[] args)
    {
        using var db = new PasswordManagerContext();

        AnsiConsole.WriteLine($"Database Path: {db.DbPath}");

        string[] test_passwords = [
            "Password",
            "password",
            "password1",
            "password2",
            "password3",
            "password123",
            "password9",
            "passwordd",
            "passwor",
            "JAME",
            "jame",
            "a",
            "b",
            "c",
            "d",
        ];

        foreach (var p in test_passwords)
        {
            var (hash, salt) = Database.Encrypt(p);
            var prettyHash = string.Join("", hash);
            var prettySalt = string.Join("", salt);
            Console.WriteLine($"Password: {p}, Hash: {prettyHash}, Salt {prettySalt}");
        }
    }
}
