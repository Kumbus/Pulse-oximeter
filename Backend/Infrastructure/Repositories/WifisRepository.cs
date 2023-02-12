using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class WifisRepository : IWifisRepository
    {
        private readonly DatabaseContext _databaseContext;
        public WifisRepository(DatabaseContext databaseContext)
        { 
            _databaseContext = databaseContext;
        }
        public async Task AddWifi(Wifi wifi)
        {
            await _databaseContext.Wifis.AddAsync(wifi);
            await _databaseContext.SaveChangesAsync();

        }

        public async Task<Wifi> CheckWifi(string name, string password)
        {
            return await _databaseContext.Wifis.SingleOrDefaultAsync(w => w.Name == name && w.Password == password);
        }

        public async Task<Wifi> GetWifi(string name)
        {
            return await _databaseContext.Wifis.SingleOrDefaultAsync(w => w.Name == name);
        }
    }
}
