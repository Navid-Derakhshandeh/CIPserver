using DataBaseModule.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DataBaseModule.Data;
using DataBaseModule.Models;
using DataBaseModule.Repositories;

namespace DataBaseModule.Repositories
{
    public class ConnectionConfigRepository : IConnectionConfigRepository
    {
        private readonly AppDbContext _context;
        public ConnectionConfigRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ConnectionConfig?> GetByIdAsync(int id)
        {
            return await _context.connectionConfigs.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<List<ConnectionConfig>> GetAllAsync()
        {
            return await _context.connectionConfigs.ToListAsync();
        }
        public async Task AddAsync(ConnectionConfig config)
        {
            await _context.connectionConfigs.AddAsync(config);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(ConnectionConfig config)
        {
            _context.connectionConfigs.Update(config);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var config = await GetByIdAsync(id);
            if (config == null)
                return;
            _context.connectionConfigs.Remove(config);
            await _context.SaveChangesAsync();
        }
    }
}
