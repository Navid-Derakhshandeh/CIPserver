using DataBaseModule.Data;
using DataBaseModule.Interfaces;
using DataBaseModule.Repositories;
using GrpcService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Security.Authentication;
using System.Text;
using ModbusModule;
using ModbusServiceModule = ModbusModule.ModbusService;
using AuthService;

namespace Porgram
{
    public class Program
    {
        static void Main(string[] args)
        {

            const string jwtSecret =
                "THIS_IS_ONLY_FOR_LEARNING_CHANGE_THIS_SECRET_123456789";

            var builder = WebApplication.CreateBuilder(args);


            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(5000, listenOptions =>
                {
                    listenOptions.Protocols =
                        Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
                });
            });

            // --------------------------------------------------
            // gRPC
            // --------------------------------------------------

            builder.Services.AddGrpc();

            // --------------------------------------------------
            // SQLite
            // --------------------------------------------------

            builder.Services.AddDbContext<AppDbContext>(
                options =>
                    options.UseSqlite(
                        "Data Source=auth.db"));

            // --------------------------------------------------
            // Repository
            // --------------------------------------------------

            builder.Services.AddScoped<
                IUserRepository,
                UserRepository>();

            // --------------------------------------------------
            // Password
            // --------------------------------------------------

            builder.Services.AddSingleton<
                PasswordHasher>();
            // --------------------------------------------------
            // Modbus
            // --------------------------------------------------
            builder.Services.AddSingleton(
                new ModbusServiceModule(
                "127.0.0.1",
                 502));
                Console.WriteLine("gRPC Modbus Server running");
            
            // --------------------------------------------------
            // JWT
            // --------------------------------------------------

            builder.Services.AddSingleton(
                new JwtTokenService(jwtSecret));

            // --------------------------------------------------
            // JWT Authentication
            // --------------------------------------------------

            builder.Services
                .AddAuthentication(
                    JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey =
                                true,

                            IssuerSigningKey =
                                new SymmetricSecurityKey(
                                    Encoding.UTF8.GetBytes(
                                        jwtSecret)),

                            ValidateIssuer =
                                false,

                            ValidateAudience =
                                false,

                            ValidateLifetime =
                                true,

                            ClockSkew =
                                TimeSpan.Zero
                        };
                });

            // --------------------------------------------------
            // Authorization
            // --------------------------------------------------

            builder.Services.AddAuthorization();

            var app = builder.Build();

            // --------------------------------------------------
            // Create SQLite database
            // --------------------------------------------------

            using (var scope =
                   app.Services.CreateScope())
            {
                var db =
                    scope.ServiceProvider
                        .GetRequiredService<AppDbContext>();

                db.Database.EnsureCreatedAsync();
            }

            // --------------------------------------------------
            // Authentication middleware
            // --------------------------------------------------

            app.UseAuthentication();

            app.UseAuthorization();

            // --------------------------------------------------
            // gRPC
            // --------------------------------------------------

            app.MapGrpcService<AuthGrpcService>();
            app.MapGrpcService<ModbusGrpc.ModbusGrpcService>();


            // --------------------------------------------------
            // Console
            // --------------------------------------------------

            Console.WriteLine();
            Console.WriteLine(
                "====================================");

            Console.WriteLine(
                "       gRPC AUTH SERVER");

            Console.WriteLine(
                "====================================");

            Console.WriteLine();

            Console.WriteLine(
                "Address : http://localhost:5000");

            Console.WriteLine(
                "Database: auth.db");

            Console.WriteLine();

            Console.WriteLine(
                "Waiting for clients...");

            Console.WriteLine();

            app.Run(
                "http://0.0.0.0:5000");
        }
    }
}