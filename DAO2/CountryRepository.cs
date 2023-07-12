using DAO.Data;
using DAO2.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DAO2
{


    public class CountryRepository : Repository<Country>, ICountryRepository
    {

        private readonly ApplicationDbContext _db; 
        public CountryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;

        }

        public IQueryable<Owner> GetOwnersByCountry(int Id)
        {

            IQueryable<Owner> query = from owner in _db.Owners
                                      where owner.Country.Id == Id
                                      select owner;


            return query;
        }

        public bool ItExists(int Id)
        {
            return _db.Countries.Any(e => e.Id == Id);
        }

        public void Update(Country country)
        {
            if (country is not null)
            {
                Country objDb = _db.Countries.Where(c=> c.Id == country.Id).FirstOrDefault();
                if (objDb != null)
                {
                    if (country.Name is not null)
                    {
                        objDb.Name = country.Name;
                    }
                  
                }
            }
        }
    }
}
