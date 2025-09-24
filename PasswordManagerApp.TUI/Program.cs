using System.Security.Cryptography;
using PasswordManagerApp.Backend;
using Spectre.Console;

namespace PasswordManagerApp.TUI;

class Program
{
    static readonly byte[] Key = {
        162, 240, 90, 217, 220, 99, 181, 109, 89, 83, 98, 129, 247, 226, 223, 136,
        0, 137, 35, 83, 113, 164, 224, 29, 5, 63, 159, 108, 181, 229, 102, 199
    };

    static readonly byte[] IV = {
        121, 178, 227, 16, 192, 98, 24, 104, 172, 236, 218, 120, 53, 178, 47, 54
    };

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

            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "0",
            "11",
        ];

        var table = new Table();

        table.AddColumn(new TableColumn("[italic red]Password[/]").RightAligned());
        table.AddColumn("[italic blue]Hash[/]");
        table.AddColumn("[italic blue]Salt[/]");

        foreach (var p in test_passwords)
        {
            var salt = Database.GenSalt();
            var hash = Database.HashPassword(p, salt);

            table.AddRow($"[bold]{p}[/]", hash.Prettify(), salt.Prettify());
        }

        table.MinimalHeavyHeadBorder();

        AnsiConsole.Write(table);

        using (Aes aes = Aes.Create())
        {
            aes.Key = Key;
            aes.IV = IV;

            AnsiConsole.MarkupLine($"Key: {aes.Key.Prettify()}, IV: {aes.IV.Prettify()}");

            string input = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur malesuada rhoncus tortor et lacinia. Sed quis lacinia urna. Etiam ultricies a ex sit amet rutrum. Aenean placerat volutpat efficitur. Sed eget sagittis augue. In tempor tellus vitae dolor vehicula, sed tincidunt ipsum porta. Nam at nibh dignissim, condimentum eros nec, tempus metus. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Aliquam id volutpat odio, et vehicula tortor. Aliquam vitae enim ut nunc blandit faucibus vitae nec ipsum. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis euismod efficitur sodales.";

            AnsiConsole.MarkupLine($"Original: [blue italic]{input}[/]");

            byte[] encrypted = Database.Encrypt(input, aes.Key, aes.IV);
            AnsiConsole.MarkupLine($"Encrypted: [red italic]{encrypted.Prettify()}[/]");

            string decrypted = Database.Decrypt(encrypted, aes.Key, aes.IV);
            AnsiConsole.MarkupLine($"Decrypted: [green italic]{decrypted}[/]");

            System.Diagnostics.Debug.Assert(input == decrypted, "Input does not match with decrypted version");
        }
    }
}
