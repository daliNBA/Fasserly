using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Fasserly.Migrator
{
    public class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                Console.WriteLine("Database Migration Tool");
                var confirmed = args.Length > 0 && args[0].ToLowerInvariant() == "-force";
                using (var context = new DatabaseContextFactory().CreateDbContext(null))
                {
                    var connection = context.Database.GetDbConnection();
                    var migrations = context.Database.GetPendingMigrations().ToList();
                    Console.WriteLine($"Server: {connection.DataSource}");
                    Console.WriteLine($"Database: {connection.Database}");
                    if (migrations.Count == 0)
                        Console.WriteLine("No pending migrations to apply.");
                    else
                    {
                        Console.WriteLine($"Pending migrations: {string.Join(", ", migrations)}");
                        if (!confirmed)
                        {
                            Console.Write("Apply pending migrations? (Y/N) ");
                            confirmed = Console.ReadKey().Key == ConsoleKey.Y;
                            Console.Write(Environment.NewLine);
                        }
                        if (confirmed)
                        {
                            Console.WriteLine("Applying migrations...");
                            context.Database.Migrate();
                            Console.WriteLine("Migrations applied.");
                            return 1;
                        }
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return -1;
            }
        }
    }
}
