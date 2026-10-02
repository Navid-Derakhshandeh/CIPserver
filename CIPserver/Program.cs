using AuthService;
using DataBaseModule.Data;
using DataBaseModule.Interfaces;
using DataBaseModule.Repositories;
using GrpcService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using ModbusModule;
using Security.Authentication;
using ServerExample;
using System.Security.Cryptography;
using System.Text;
using zeromq_module;
using ZeroMqService;
using ModbusServiceModule = ModbusModule.ModbusService;

namespace Porgram
{
    public class Program
    {
        static void Main(string[] args)
        {
            string secretFile = "jwt.secret";
            string jwtSecret;
            if (File.Exists(secretFile))
            {
                jwtSecret =
                    File.ReadAllText(secretFile);
            }
            else
            {
                jwtSecret =
                    Convert.ToBase64String(
                        RandomNumberGenerator.GetBytes(64));
                File.WriteAllText(
                    secretFile,
                    jwtSecret);
            }
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.Configure<HostOptions>(
            options =>
            {
                options.BackgroundServiceExceptionBehavior =
                    BackgroundServiceExceptionBehavior.Ignore;
            });
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
            builder.Services.AddSingleton<AuthorizedClientManager>();
            // --------------------------------------------------
            // Modbus
            // --------------------------------------------------
            builder.Services.AddSingleton<ModbusServiceModule>(sp =>
            {
                return new ModbusServiceModule(
                    "127.0.0.1",
                    502);
            });
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
            //---------------------------------------------------
            // ZeroMQ
            //--------------------------------------------------
            builder.Services.AddHostedService<ZeroMqBackgroundService>();
            // --------------------------------------------------
            // Build Application
            // --------------------------------------------------
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