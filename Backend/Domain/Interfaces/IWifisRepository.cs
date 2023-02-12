using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IWifisRepository
    {
        public Task AddWifi(Wifi wifi);
        public Task<Wifi> GetWifi(string name);
        public Task<Wifi> CheckWifi(string name, string password);
    }
}
