using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Security.Authentication
{
    public class PasswordHasher
    {
        public (string Hash, string Salt)
            HashPassword(string password)
        {
            byte[] salt =
                RandomNumberGenerator.GetBytes(32);

            byte[] passwordBytes =
                Encoding.UTF8.GetBytes(password);

            byte[] input =
                new byte[
                    salt.Length +
                    passwordBytes.Length];

            Buffer.BlockCopy(
                salt,
                0,
                input,
                0,
                salt.Length);

            Buffer.BlockCopy(
                passwordBytes,
                0,
                input,
                salt.Length,
                passwordBytes.Length);

            byte[] hash =
                SHA256.HashData(input);

            return (
                Convert.ToBase64String(hash),
                Convert.ToBase64String(salt)
            );
        }

        public bool VerifyPassword(
            string password,
            string storedHash,
            string storedSalt)
        {
            byte[] salt =
                Convert.FromBase64String(
                    storedSalt);

            byte[] passwordBytes =
                Encoding.UTF8.GetBytes(password);

            byte[] input =
                new byte[
                    salt.Length +
                    passwordBytes.Length];

            Buffer.BlockCopy(
                salt,
                0,
                input,
                0,
                salt.Length);

            Buffer.BlockCopy(
                passwordBytes,
                0,
                input,
                salt.Length,
                passwordBytes.Length);

            byte[] hash =
                SHA256.HashData(input);

            byte[] expectedHash =
                Convert.FromBase64String(
                    storedHash);

            return CryptographicOperations
                .FixedTimeEquals(
                    hash,
                    expectedHash);
        }
    }
}
