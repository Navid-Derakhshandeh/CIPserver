using Security.Authentication;
using DataBaseModule;
using DataBaseModule.Interfaces;
using DataBaseModule.Models;
using AuthService;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Grpc.AspNetCore.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcService
{ 
    public class AuthGrpcService
        : AuthService.AuthService.AuthServiceBase
    {
        private readonly IUserRepository _users;

        private readonly PasswordHasher _passwordHasher;

        private readonly JwtTokenService _jwt;

        public AuthGrpcService(
            IUserRepository users,
            PasswordHasher passwordHasher,
            JwtTokenService jwt)
        {
            _users = users;
            _passwordHasher = passwordHasher;
            _jwt = jwt;
        }

        // --------------------------------------------------
        // SIGNUP
        // --------------------------------------------------

        public override async Task<SignupResponse> Signup(
            SignupRequest request,
            ServerCallContext context)
        {
            Console.WriteLine();

            Console.WriteLine(
                $"Signup request: {request.Username}");

            if (string.IsNullOrWhiteSpace(
                    request.Username))
            {
                return new SignupResponse
                {
                    Success = false,
                    Message =
                        "Username is required"
                };
            }

            if (string.IsNullOrWhiteSpace(
                    request.Password))
            {
                return new SignupResponse
                {
                    Success = false,
                    Message =
                        "Password is required"
                };
            }

            var existingUser =
                await _users.GetByUsernameAsync(
                    request.Username);

            if (existingUser != null)
            {
                return new SignupResponse
                {
                    Success = false,
                    Message =
                        "Username already exists"
                };
            }

            var result =
                _passwordHasher.HashPassword(
                    request.Password);

            var user = new User
            {
                Username =
                    request.Username,

                PasswordHash =
                    result.Hash,

                PasswordSalt =
                    result.Salt
            };

            await _users.AddAsync(user);

            Console.WriteLine(
                $"User created: {user.Username}");

            return new SignupResponse
            {
                Success = true,
                Message =
                    "User created successfully"
            };
        }

        // --------------------------------------------------
        // LOGIN
        // --------------------------------------------------

        public override async Task<LoginResponse> Login(
            LoginRequest request,
            ServerCallContext context)
        {
            Console.WriteLine();

            Console.WriteLine(
                $"Login request: {request.Username}");

            // ----------------------------------------------
            // 1. Find username
            // ----------------------------------------------

            var user =
                await _users.GetByUsernameAsync(
                    request.Username);

            if (user == null)
            {
                Console.WriteLine(
                    "Username not found.");

                return InvalidLogin();
            }

            Console.WriteLine(
                "Username found.");

            // ----------------------------------------------
            // 2. Verify password
            // ----------------------------------------------

            bool valid =
                _passwordHasher.VerifyPassword(
                    request.Password,
                    user.PasswordHash,
                    user.PasswordSalt);

            if (!valid)
            {
                Console.WriteLine(
                    "Password is invalid.");

                return InvalidLogin();
            }

            Console.WriteLine(
                "Password is valid.");

            // ----------------------------------------------
            // 3. Create JWT
            // ----------------------------------------------

            string token =
                _jwt.CreateToken(user);

            Console.WriteLine(
                "JWT created.");

            return new LoginResponse
            {
                Success = true,

                Message =
                    "Login successful",
                Token = token
            };
        }

        // --------------------------------------------------
        // PROTECTED METHOD
        // --------------------------------------------------

        [Authorize]

        public override Task<UserInfoResponse>
    GetUserInfo(
        UserInfoRequest request,
        ServerCallContext context)
        {
            var httpContext =
                context.GetHttpContext();


            string? userId =
                httpContext.User.FindFirst(
                    System.Security.Claims.ClaimTypes.NameIdentifier)
                ?.Value;


            string? username =
                httpContext.User.FindFirst(
                    System.Security.Claims.ClaimTypes.Name)
                ?.Value;


            Console.WriteLine();

            Console.WriteLine(
                "Authenticated request received.");

            Console.WriteLine(
                $"User ID: {userId}");

            Console.WriteLine(
                $"Username: {username}");


            return Task.FromResult(
                new UserInfoResponse
                {
                    UserId = userId ?? "",
                    Username = username ?? ""
                });
        }

        private static LoginResponse InvalidLogin()
        {
            return new LoginResponse
            {
                Success = false,

                Message =
                    "Invalid username or password",

                Token = string.Empty
            };
        }
    }
}