using DAO.Data;
using DAO2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO2
{
    public class UnitOfWork : IUnitOfWork
    {
        public IPokemonRepository Pokemon{ get; set; }
        public ICategoryRepository Category { get; set; }
        public ICountryRepository Country { get; set; }
        public IOwnerRepository Owner { get; set; } 
        public IReviewsRepository Reviews { get; set; }


        private readonly ApplicationDbContext _db;

        public UnitOfWork (ApplicationDbContext db)
        {
            _db = db;
            Pokemon= new PokemonRepository (_db);
            Category = new CategoryRepository (_db);   
            Country = new CountryRepository (_db);  
            Owner = new OwnerRepository (_db);  
            Reviews = new ReviewsRepository (_db);  


        }   

        public bool Save()
        {
           var result = _db.SaveChanges();
           bool good = result > 0 ? true : false ;
            return good;
        }
    }
}
