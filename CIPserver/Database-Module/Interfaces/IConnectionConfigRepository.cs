using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBaseModule.Models;

namespace DataBaseModule.Interfaces
{
    public  interface IConnectionConfigRepository
    {
        Task<ConnectionConfig> GetByIdAsync(int id);
        Task<List<ConnectionConfig>> GetAllAsync();
        Task AddAsync(ConnectionConfig config);
        Task UpdateAsync(ConnectionConfig config);
        Task DeleteAsync(int id);

    }
}
