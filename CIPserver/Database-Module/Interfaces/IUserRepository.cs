using DataBaseModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseModule.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(
            string username);

        Task AddAsync(User user);
    }
}
