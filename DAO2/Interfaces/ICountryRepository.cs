using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace DAO2.Interfaces
{
    public interface ICountryRepository : IRepository<Country> 
    {
        void Update(Country country);
        bool ItExists(int Id);
        IQueryable<Owner> GetOwnersByCountry(int Id); 

    }
}
