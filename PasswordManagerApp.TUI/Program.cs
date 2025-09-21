namespace PasswordManagerApp.TUI;

using PasswordManagerApp.Backend.Database;

class Program
{
    static void Main(string[] args)
    {
        using (var db = new PasswordManagerContext())
        {
            Console.WriteLine($"Database Path: {db.DbPath}");
        }
    }
}
