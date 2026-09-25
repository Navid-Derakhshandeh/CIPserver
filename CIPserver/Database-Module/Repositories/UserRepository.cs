using DataBaseModule.Data;
using DataBaseModule.Interfaces;
using DataBaseModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataBaseModule.Repositories
{

    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(
            AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByUsernameAsync(
            string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(
                    x => x.Username == username);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync();
        }
    }
}
