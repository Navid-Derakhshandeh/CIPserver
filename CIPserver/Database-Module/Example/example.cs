using DataBaseModule.Data;
using DataBaseModule.Interfaces;
using DataBaseModule.Models;
using DataBaseModule.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Example
{
    public class Database_Example
    {
        public static async Task database_example()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlite("Data Source=device.db");

            using var context = new AppDbContext(optionsBuilder.Options);

            // Create database
            await context.Database.EnsureCreatedAsync();

            IConnectionConfigRepository repository =
                new ConnectionConfigRepository(context);


            // ========================================
            // ADD
            // ========================================

            var modbus = await repository.GetByIdAsync(1);

            if (modbus == null)
            {
                await repository.AddAsync(new ConnectionConfig
                {
                    Id = 1,
                    Protocol = "Modbus",
                    Ip = "127.0.0.1",
                    Port = 502
                });
            }


            var profinet = await repository.GetByIdAsync(2);

            if (profinet == null)
            {
                await repository.AddAsync(new ConnectionConfig
                {
                    Id = 2,
                    Protocol = "Profinet",
                    Ip = "127.0.0.1",
                    Port = 34964
                });
            }


            // ========================================
            // GET ALL
            // ========================================

            Console.WriteLine("All configurations:");

            var configs = await repository.GetAllAsync();

            foreach (var config in configs)
            {
                Console.WriteLine(
                    $"ID={config.Id}, " +
                    $"Protocol={config.Protocol}, " +
                    $"Ip={config.Ip}, " +
                    $"Port={config.Port}");
            }


            // ========================================
            // GET BY ID
            // ========================================

            Console.WriteLine();
            Console.WriteLine("Get Modbus Configuration:");

            var config1 = await repository.GetByIdAsync(1);

            if (config1 != null)
            {
                Console.WriteLine($"Protocol={config1.Protocol}");
                Console.WriteLine($"Ip={config1.Ip}");
                Console.WriteLine($"Port={config1.Port}");
            }


            // ========================================
            // UPDATE
            // ========================================

            Console.WriteLine();
            Console.WriteLine("Modbus updating ...");

            if (config1 != null)
            {
                config1.Ip = "127.0.0.1";
                config1.Port = 502;

                await repository.UpdateAsync(config1);
            }


            // ========================================
            // DELETE
            // ========================================

            Console.WriteLine();
            Console.WriteLine("Deleting Profinet ...");

            await repository.DeleteAsync(2);


            // ========================================
            // GET ALL AGAIN
            // ========================================

            Console.WriteLine();
            Console.WriteLine("Configurations after update/delete:");

            configs = await repository.GetAllAsync();

            foreach (var config in configs)
            {
                Console.WriteLine(
                    $"ID={config.Id}, " +
                    $"Protocol={config.Protocol}, " +
                    $"IP={config.Ip}, " +
                    $"Port={config.Port}");
            }
        }
    }
}